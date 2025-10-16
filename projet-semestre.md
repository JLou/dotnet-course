# Projet du semestre

## Attendu

Vous pouvez travaillez en binôme ou seul.

**Deadline : 20 novembre 2025**

Vous devrez développer une application web, composée d'un front-end en blazor,
et d'un back-end en ASP.NET Core.

L'application devra utiliser une base de données (par défaut SQL Server, mais
n'importe quel SGBD est autorisé), et Entity Framework Core pour l'accès aux
données.

Un serveur d'autorisation OAuth/OIDC devra être utilisé pour l'authentification
des utilisateurs et l'autorisation. Vous pouvez utiliser Keycloak comme expliqué
dans le TP, ou un autre serveur d'autorisation de votre choix (Azure AD par
exemple, ou discord).

La solution devra utiliser Aspire pour orchestrer les différents services.

### Contraintes

L'application devra

- Avoir au moins 2 rôles différents (ex: user et admin)
- Avoir au moins 4 pages différentes dont des pages :
  - publiques
  - accessibles uniquement aux utilisateurs authentifiés
  - accessibles uniquement aux utilisateurs avec un rôle spécifique (ex: admin)
- Avoir au moins 4 tables différentes dans la base de données avec des relations
  entre elles
- Avoir des endpoints REST dans le back-end pour accéder aux données, publiques,
  et protégés
- Utiliser Entity Framework Core pour l'accès aux données
- Utiliser Aspire pour orchestrer les différents services
- Avoir une CI qui tourne dans github actions
- Le code devra etre hébergé sur un dépôt github public
- Avoir un fichier README.md à la racine du dépôt github qui explique comment
  lancer l'application

Le code devra respecter les bonnes pratiques, comme le clean code, la séparation
des responsabilités, l'injection de dépendances, etc...

### Présentation

Lors de la dernière séance du semestre, vous devrez présenter votre application
en 10 minutes. 5 minutes de pitch pour présenter votre application, et 5 minutes
de démonstration de l'application. Le format de présentation est libre, mais
vous devez respecter le temps imparti.

La démonstration devra montrer les différentes fonctionnalités de l'application.

### Suggestions

Voici quelques pistes pour améliorer votre projets. Renseignez-vous sur les
librairies suivantes et regardez si elles peuvent vous être utiles.

- [MudBlazor](https://mudblazor.com/) : une librairie de composants UI pour
  Blazor
- [CSharpier](https://csharpier.com/) : un formateur de code C# (équivalent de
  Prettier en JS)
- [FluentValidation](https://fluentvalidation.net/) : une librairie pour la
  validation des données
- [TUnit](https://tunit.dev/docs/getting-started/installation/) : un framework
  de tests moderne
- [SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction)
  : une librairie pour les applications en temps réel

Bien que non obligatoire, vous pouvez également écrire des tests unitaires et/ou
des tests d'intégration pour votre application.

- [Playwright](https://playwright.dev/dotnet/docs/intro) : une librairie pour
  l'automatisation des tests end-to-end
- [xUnit](https://xunit.net/) : un framework de tests unitaires pour .NET
- [Tests d'intégration avec ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests)
  : documentation officielle pour écrire des tests d'intégration pour les
  applications ASP.NET Core

### Contournement des problemes avec Docker ou Aspire

Si vous rencontrez des problèmes avec Docker ou Aspire, vous pouvez contourner
ces problèmes en exécutant les services manuellement sur votre machine. Par
exemple, vous pouvez exécuter Keycloak localement sans Docker, ou utiliser une
base de données locale ou sur internet (en utilisant vos crédits azure par
exemple, mais attention a ne pas leak vos secrets dans votre repo dans ce cas).
