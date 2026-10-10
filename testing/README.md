# Festival Planner — atelier de tests

Le Chroma Festival publie sa programmation dans un fichier texte. Cette application lit ce
fichier, permet d'explorer le programme et indique quels concerts de votre sélection vous devrez
manquer, soit parce que deux artistes jouent en même temps, soit parce que vous n'aurez pas le
temps de traverser le site.

L'application fonctionne. **Elle ne contient aucun test.** À vous de les écrire.

```
testing/
├── FestivalPlanner.slnx              la solution
├── global.json                       configuration du lanceur de tests (ne pas supprimer)
└── src/
    ├── FestivalPlanner/              la bibliothèque que vous allez tester
    │   ├── FestivalSet.cs            un artiste, une scène, un début et une fin
    │   ├── LineupParser.cs           fonctionnalité 1 — lire la programmation
    │   ├── Lineup.cs                 fonctionnalité 2 — explorer le programme
    │   └── ClashDetector.cs          fonctionnalité 3 — détecter les conflits
    └── FestivalPlanner.Cli/          une démonstration exécutable
        ├── Program.cs
        └── lineup.txt                programmation du samedi : 12 concerts, 3 scènes
```

## 1. Lancer l'application

```bash
dotnet run --project src/FestivalPlanner.Cli
dotnet run --project src/FestivalPlanner.Cli -- --at 23:15 --walk 25
```

L'option `--at` permet de savoir quels artistes jouent à une heure donnée. L'option `--walk`
définit le nombre de minutes nécessaires pour passer d'une scène à une autre. Essayez plusieurs
valeurs avant d'écrire le moindre test : vous devez d'abord comprendre ce que l'application est
censée faire.

## 2. Créer le projet de tests

Depuis ce dossier :

```bash
# une seule fois par machine
dotnet new install TUnit.Templates

dotnet new TUnit -n FestivalPlanner.Tests -o tests/FestivalPlanner.Tests
dotnet add tests/FestivalPlanner.Tests reference src/FestivalPlanner
dotnet sln add tests/FestivalPlanner.Tests
dotnet test
```

Le modèle génère plusieurs exemples (`BasicTests.cs`, `Calculator.cs`, `DataDrivenTests.cs`,
`DependencyInjectionTests.cs`, `HooksAndLifecycle.cs` et un dossier `Data/`). Commencez par lire
`DataDrivenTests.cs` : c'est un bon aide-mémoire pour les tests paramétrés. Supprimez ensuite tous
ces exemples et repartez d'un fichier vide.

Remarquez que le projet de tests référence `src/FestivalPlanner`, c'est-à-dire la bibliothèque, et
**non** l'application en ligne de commande. Celle-ci sert uniquement de démonstration ; les
comportements intéressants à tester se trouvent dans la bibliothèque.

## 3. Commandes utiles

```bash
dotnet build                                              # compiler toute la solution
dotnet test                                               # exécuter tous les tests
dotnet test --list-tests                                  # afficher les tests découverts
dotnet test --treenode-filter "/*/*/LineupParserTests/*"  # exécuter une classe de tests
dotnet test --treenode-filter "/*/*/*/Duration_IsTheTimeOnStage"
```

> Les options `--filter-method` et `--filter-class` proviennent de VSTest et ne fonctionnent
> **pas** ici : TUnit utilise Microsoft.Testing.Platform. C'est également pour cette raison que ce
> dossier contient un fichier `global.json` qui sélectionne ce lanceur. Sans lui, `dotnet test`
> refuse d'exécuter les tests avec le SDK .NET 10.

## 4. Écrire des tests avec TUnit

Les méthodes de test sont `async`, portent l'attribut `[Test]` et utilisent des assertions
attendues avec `await`.

```csharp
namespace FestivalPlanner.Tests;

public class FestivalSetTests
{
    private static readonly DateOnly Day = new(2026, 7, 11);

    [Test]
    public async Task Duration_IsTheTimeOnStage()
    {
        var volta = LineupParser.Parse("Mainstage;Volta;21:15;22:30", Day);

        await Assert.That(volta.Duration).IsEqualTo(TimeSpan.FromMinutes(75));
    }
}
```

Un test paramétré permet d'exécuter la même méthode avec plusieurs cas. Chaque cas est affiché
séparément dans le rapport :

```csharp
[Test]
[Arguments("21:14", false)]
[Arguments("21:15", true)]
[Arguments("22:29", true)]
[Arguments("22:30", false)]
public async Task IsPlayingAt_ExcludesTheEndOfTheSet(string moment, bool expected)
{
    var volta = LineupParser.Parse("Mainstage;Volta;21:15;22:30", Day);
    var at = Day.ToDateTime(TimeOnly.Parse(moment));

    await Assert.That(volta.IsPlayingAt(at)).IsEqualTo(expected);
}
```

Lorsque les données ne tiennent plus facilement dans des attributs, fournissez-les avec une
méthode :

```csharp
[Test]
[MethodDataSource(nameof(InvalidLines))]
public async Task TryParse_RejectsInvalidLines(string line)
{
    await Assert.That(LineupParser.TryParse(line, Day, out _)).IsFalse();
}

public static IEnumerable<string> InvalidLines()
{
    yield return "Mainstage;Volta;21:15";
    yield return ";Volta;21:15;22:30";
    yield return "Mainstage;Volta;9pm;22:30";
}
```

Quelques assertions dont vous aurez besoin :

```csharp
await Assert.That(value).IsEqualTo(TimeSpan.FromMinutes(75));
await Assert.That(lineup.Find("Daft Punk")).IsNull();
await Assert.That(canWatchBoth).IsTrue();
await Assert.That(lineup.Stages).IsEquivalentTo(new[] { "Greenhouse", "Mainstage" });
await Assert.That(clashes).Count().IsEqualTo(1);
await Assert.That(() => lineup.Add(set)).Throws<InvalidOperationException>();
await Assert.That(() => LineupParser.ParseMany(lines, Day))
    .Throws<FormatException>()
    .WithMessageContaining("Line 2");
```

Astuce : `lineup.Sets.Select(set => set.Artist)` produit une séquence de noms, beaucoup plus
simple à comparer dans une assertion qu'une séquence de `FestivalSet`.

## 5. Règles à vérifier par les tests

Les règles ci-dessous constituent le contrat attendu de l'application. Pour chaque règle,
écrivez au moins un test qui échouerait si elle n'était plus respectée.

Commencez par les cas normaux, puis ajoutez les cas limites indiqués. Ne cherchez pas seulement à
exécuter toutes les lignes du programme : vérifiez les résultats observables.

### Modèle — `FestivalSet`

#### Règles

1. Le nom de l'artiste et celui de la scène sont obligatoires.
2. Les espaces placés avant ou après ces noms sont supprimés.
3. La fin du concert doit être strictement postérieure à son début.
4. `Duration` est la différence entre l'heure de fin et l'heure de début.
5. L'instant de début appartient au concert ; l'instant de fin n'en fait pas partie.
6. Deux concerts se chevauchent uniquement s'ils partagent une durée strictement positive.

#### Cas à tester

| Cas | Résultat attendu |
|---|---|
| Concert de 21:15 à 22:30 | `Duration` vaut 75 minutes |
| Consultation à 21:14 | `IsPlayingAt` renvoie `false` |
| Consultation à 21:15 exactement | `IsPlayingAt` renvoie `true` |
| Consultation à 22:29 | `IsPlayingAt` renvoie `true` |
| Consultation à 22:30 exactement | `IsPlayingAt` renvoie `false` |
| Premier concert 21:15–22:30, second 22:30–23:30 | Aucun chevauchement |
| Premier concert 21:15–22:30, second 22:29–23:30 | Chevauchement d'une minute |
| Heure de début et heure de fin identiques | `ArgumentException` |
| Heure de fin antérieure à l'heure de début | `ArgumentException` |
| Artiste ou scène vide, ou composé uniquement d'espaces | `ArgumentException` |

Les quatre cas autour de `IsPlayingAt` doivent être réunis dans un **test paramétré**.

### Fonctionnalité 1 — `LineupParser` : lire la programmation

Une ligne respecte le format suivant :

```text
stage;artist;start;end
```

Les heures utilisent obligatoirement le format `HH:mm`.

#### Règles

1. Une ligne contient exactement quatre champs.
2. La scène et l'artiste ne peuvent pas être vides.
3. Les espaces autour de chaque champ sont supprimés.
4. Le festival commence le jour fourni à `Parse` et se poursuit après minuit :
   - une heure comprise entre `06:00` et `23:59` appartient au jour du festival ;
   - une heure comprise entre `00:00` et `05:59` appartient au lendemain.
5. Si l'heure de fin est antérieure à l'heure de début, la fin est placée le lendemain.
6. `Parse` lève une `FormatException` lorsqu'une ligne est invalide.
7. `TryParse` ne lève pas d'exception : il renvoie `false` et produit `null`.
8. `ParseMany` ignore les lignes vides et les commentaires commençant par `#`.
9. Lorsqu'une ligne de `ParseMany` est invalide, le message d'erreur contient son numéro réel,
   commentaires et lignes vides compris.

#### Cas valides à tester avec `[Arguments]`

Pour un festival organisé le **11 juillet 2026** :

| Début | Fin | Début attendu | Fin attendue |
|---|---|---|---|
| `18:00` | `19:00` | 11 juillet à 18:00 | 11 juillet à 19:00 |
| `23:00` | `00:30` | 11 juillet à 23:00 | 12 juillet à 00:30 |
| `00:15` | `02:00` | 12 juillet à 00:15 | 12 juillet à 02:00 |
| `05:59` | `07:00` | 12 juillet à 05:59 | 12 juillet à 07:00 |
| `06:00` | `07:00` | 11 juillet à 06:00 | 11 juillet à 07:00 |

#### Lignes invalides à tester avec `[MethodDataSource]`

| Ligne | Motif |
|---|---|
| `Mainstage;Volta;21:15` | champ manquant |
| `Mainstage;Volta;21:15;22:30;extra` | champ supplémentaire |
| `;Volta;21:15;22:30` | scène absente |
| `Mainstage;;21:15;22:30` | artiste absent |
| `Mainstage;Volta;9pm;22:30` | mauvais format d'heure |
| `Mainstage;Volta;21:15;25:00` | heure impossible |

### Fonctionnalité 2 — `Lineup` : explorer le programme

Créez une programmation de quatre ou cinq concerts dans une méthode utilitaire et réutilisez-la
dans les tests. Vérifiez le contenu des collections, mais aussi leur ordre lorsque celui-ci fait
partie de la règle.

#### Règles d'ajout

1. `Add` ajoute un concert à la programmation.
2. Deux artistes peuvent jouer en même temps sur des scènes différentes.
3. Deux concerts ne peuvent pas se chevaucher sur la même scène.
4. Deux concerts peuvent s'enchaîner sans intervalle sur la même scène : une fin à 22:30 suivie
   d'un début à 22:30 est valide.
5. Un artiste ne peut apparaître qu'une seule fois dans la programmation.
6. Les comparaisons de scènes et d'artistes ne tiennent pas compte des majuscules.

#### Règles de recherche

| Méthode | Résultat attendu |
|---|---|
| `PlayingAt` | Tous les concerts actifs à l'instant demandé ; collection vide si personne ne joue |
| `ForStage` | Concerts de la scène demandée, du plus tôt au plus tard |
| `Find` | Concert de l'artiste demandé, ou `null` si l'artiste est absent |
| `Stages` | Chaque scène une seule fois, dans l'ordre alphabétique |
| `Headliners` | Dernier concert chronologique de chaque scène |
| `InStartOrder` | Tous les concerts, du plus tôt au plus tard |

Ajoutez au minimum un test où `PlayingAt` renvoie plusieurs artistes et un autre où il renvoie une
collection vide. Utilisez une casse différente dans les appels à `ForStage` et `Find`.

### Fonctionnalité 3 — `ClashDetector` : détecter les conflits

Un spectateur veut voir chaque concert de sa sélection **du début à la fin**.

#### Règles

1. Deux concerts qui se chevauchent sont toujours incompatibles.
2. Deux concerts consécutifs sur la même scène sont compatibles : aucun déplacement n'est requis.
3. Sur deux scènes différentes, les concerts sont compatibles si le temps disponible est
   **supérieur ou égal** au temps de marche.
4. Si le temps disponible est strictement inférieur au temps de marche, il existe un conflit.
5. `GapBetween` renvoie l'intervalle disponible entre deux concerts, quel que soit l'ordre des
   arguments.
6. `GapBetween` renvoie zéro lorsque les concerts se chevauchent.
7. Un temps de marche négatif est refusé avec une `ArgumentOutOfRangeException`.
8. `FindClashes` examine chaque paire une seule fois et renvoie les conflits dans l'ordre
   chronologique.
9. Un conflit dû à un chevauchement porte le type `Overlap` et indique sa durée.
10. Un conflit sans chevauchement, mais avec trop peu de temps pour marcher, porte le type
    `NotEnoughTimeToWalk` et une durée de chevauchement égale à zéro.

#### Test paramétré obligatoire

Utilisez ces deux concerts, situés sur des scènes différentes :

```text
Volta        21:15–22:30  Mainstage
Deep Signal  22:45–23:30  Warehouse
```

Ils sont séparés par exactement 15 minutes.

| Temps de marche | `CanWatchBoth` attendu |
|---:|---|
| 0 minute | `true` |
| 14 minutes | `true` |
| 15 minutes | `true` |
| 16 minutes | `false` |
| 30 minutes | `false` |

Le cas **15 minutes disponibles pour 15 minutes de marche** est le cas limite essentiel : une
égalité doit être acceptée.

## 6. Critères de réussite

Votre projet est terminé lorsque :

- `dotnet test` découvre et exécute tous vos cas paramétrés séparément ;
- chaque règle ci-dessus possède au moins un test ;
- les bornes exactes sont testées, pas seulement des valeurs éloignées de la limite ;
- les tests vérifient les résultats publics sans reproduire l'algorithme interne ;
- une défaillance indique clairement quelle règle n'est plus respectée ;
- tous les tests passent après correction des éventuels défauts découverts.
