# Solution de référence

Ce dossier contient la suite de tests complète attendue pour l'atelier.

Le projet n'est volontairement pas ajouté à `FestivalPlanner.slnx`, afin que la solution remise
aux étudiants continue à ne contenir aucun projet de tests. Pour exécuter la correction :

```bash
dotnet test --project solution/FestivalPlanner.Tests/FestivalPlanner.Tests.csproj
```

Les tests couvrent :

- les invariants et les bornes temporelles de `FestivalSet` ;
- les cas valides, invalides, paramétrés et le passage à minuit de `LineupParser` ;
- les recherches, tris, collections et règles d'ajout de `Lineup` ;
- les chevauchements, temps de marche et types de conflits de `ClashDetector`.

Le cas paramétré où l'intervalle disponible est exactement égal au temps de marche est notamment
présent. Il détecte immédiatement la mutation pédagogique de `>=` vers `>` dans
`ClashDetector.CanWatchBoth`.

Avec cette mutation présente dans le projet étudiant, le résultat attendu est donc :

```text
61 tests réussis, 1 test échoué
CanWatchBoth_DependsOnTheWalkingTime(15, True)
```

Après avoir remplacé `>` par `>=`, les 62 tests doivent réussir.
