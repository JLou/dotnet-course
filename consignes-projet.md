# Projet du trimestre

## Objectif

Vous devrez développer une application web répondant à un besoin de votre choix.
Vous pouvez travailler **en binôme ou seul**.

L’application devra être composée :

- d’un front-end en **Blazor** ;
- d’un back-end en **ASP.NET Core** ;
- d’une base de données, avec **Entity Framework Core** pour l’accès aux données ;
- d’un serveur d’autorisation **OAuth 2.0 / OpenID Connect** pour
  l’authentification des utilisateurs et la gestion des autorisations ;
- d’une orchestration des différents services avec **Aspire**.

Le choix du sujet est libre : choisissez un besoin qui vous intéresse et imaginez
une application qui y apporte une réponse pertinente.

## Dates importantes

- **Présentation de l’application : le 14 décembre 2026.**
- **Remise finale du projet : le 21 décembre 2026.**

La présentation aura donc lieu une semaine avant la remise finale. Vous pourrez
utiliser cette dernière semaine pour finaliser votre projet et tenir compte des
retours reçus.

## Contraintes techniques

L’application devra :

- Avoir au moins **deux rôles différents**, par exemple utilisateur et
  administrateur.
- Avoir au moins **quatre pages différentes**, dont :
  - des pages publiques ;
  - des pages accessibles uniquement aux utilisateurs authentifiés ;
  - des pages accessibles uniquement aux utilisateurs possédant un rôle
    spécifique.
- Avoir au moins **quatre tables dans la base de données**, avec des relations
  entre elles.
- Exposer des **endpoints REST** dans le back-end, avec des endpoints publics et
  des endpoints protégés.
- Utiliser **Entity Framework Core** pour l’accès aux données.
- Utiliser un serveur d’autorisation **OAuth 2.0 / OpenID Connect**, par exemple
  Keycloak ou Microsoft Entra ID.
- Utiliser **Aspire** pour orchestrer les différents services.
- Disposer d’une **intégration continue avec GitHub Actions**.
- Être hébergée dans un **dépôt GitHub public**.
- Inclure un fichier **README.md à la racine du dépôt**, expliquant les
  prérequis, la configuration nécessaire et les étapes pour lancer l’application.

SQL Server est le choix proposé par défaut, mais vous pouvez utiliser un autre
système de gestion de base de données.

Le code devra respecter les bonnes pratiques : lisibilité,
séparation des responsabilités, injection de dépendances, etc.

## Présentation — 14 décembre

Vous disposerez de **10 minutes**, réparties comme suit :

- **5 minutes de pitch** pour présenter votre application et le besoin auquel
  elle répond ;
- **5 minutes de démonstration** pour montrer son fonctionnement.

### Le pitch : donnez envie d’utiliser votre application

**Il ne s’agit pas d’un exposé sur votre implémentation technique.** L’objectif
n’est pas de parcourir votre code, de détailler vos classes ou d’énumérer les
technologies utilisées.

Présentez votre application comme si vous vous adressiez à de futurs utilisateurs
ou à un client. Votre public doit comprendre :

- **À qui s’adresse votre application ?**
- **Quel problème résout-elle ou à quel besoin répond-elle ?**
- **Comment répond-elle concrètement à ce besoin ?**
- **Quelle valeur apporte-t-elle et pourquoi aurait-on envie de l’utiliser ?**

**Soyez créatifs dans votre présentation.** Le format est libre : mise en
situation, histoire d’un utilisateur, jeu de rôle, lancement de produit, supports
visuels… Choisissez une approche qui met votre application en valeur et rend son
intérêt concret.

La créativité doit servir votre message : une présentation originale doit aussi
rester claire, structurée et respecter le temps imparti.

### La démonstration : montrez la valeur de votre application

Préparez un **scénario d’utilisation cohérent**, plutôt qu’une succession de pages
sans lien entre elles.

La démonstration devra montrer les principales fonctionnalités de l’application,
ainsi que les différences d’accès selon les utilisateurs et leurs rôles.

Privilégiez ce que l’utilisateur peut accomplir avec votre application. Les
détails techniques ne doivent pas prendre le pas sur la démonstration.

**Préparez et répétez votre présentation**, notamment pour vérifier le minutage et
le bon fonctionnement de votre scénario.

## Suggestions

Les bibliothèques et outils suivants peuvent vous être utiles. Leur utilisation
n’est pas obligatoire : choisissez-les en fonction des besoins de votre projet.

- [MudBlazor](https://mudblazor.com/) : une bibliothèque de composants
  d’interface pour Blazor.
- [CSharpier](https://csharpier.com/) : un formateur de code C#, comparable à
  Prettier en JavaScript.
- [FluentValidation](https://fluentvalidation.net/) : une bibliothèque de
  validation des données.
- [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction) :
  une bibliothèque pour les fonctionnalités en temps réel.

Bien que cela ne soit pas obligatoire, vous pouvez également écrire des tests
pour votre application :

- [TUnit](https://tunit.dev/docs/getting-started/installation/) : un framework de
  tests moderne pour .NET.
- [xUnit](https://xunit.net/) : un framework de tests pour .NET.
- [Tests d’intégration avec ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests) :
  la documentation officielle pour tester l’intégration des composants de votre
  application.
- [Playwright](https://playwright.dev/dotnet/docs/intro) : un outil
  d’automatisation des tests de bout en bout.

## En cas de difficultés avec Docker ou Aspire

Si des problèmes avec Docker ou Aspire vous empêchent de lancer l’application,
vous pouvez exécuter les services manuellement pour continuer à travailler et
assurer la démonstration.

Par exemple, vous pouvez lancer votre serveur d’autorisation sans Docker, ou
utiliser une base de données locale ou hébergée.

Ce contournement ne remplace pas la configuration Aspire attendue dans le projet.
Documentez dans le README les difficultés rencontrées et les étapes nécessaires
pour lancer l’application autrement.

**Ne publiez jamais de secrets dans votre dépôt GitHub** : mots de passe, clés
d’API ou chaînes de connexion contenant des identifiants. Utilisez un mécanisme
de configuration adapté, comme les secrets utilisateur .NET, les variables
d’environnement ou les secrets GitHub Actions.
