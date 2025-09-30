
-- Création de la table joueur
CREATE TABLE Joueur (
                        Id INT PRIMARY KEY IDENTITY(1,1),
                        Nom NVARCHAR(100) NOT NULL
);

-- Création de la table match
CREATE TABLE Match (
                       Id INT PRIMARY KEY IDENTITY(1,1),
                       Joueur1Id INT NOT NULL,
                       Joueur2Id INT NOT NULL,
                       FOREIGN KEY (Joueur1Id) REFERENCES Joueur(Id),
                       FOREIGN KEY (Joueur2Id) REFERENCES Joueur(Id)
);

-- Création de la table round
CREATE TABLE Round (
                       Id INT PRIMARY KEY IDENTITY(1,1),
                       MatchId INT NOT NULL,
                       CoupJoueur1 NVARCHAR(100),
                       CoupJoueur2 NVARCHAR(100),
                       FOREIGN KEY (MatchId) REFERENCES Match(Id)
);
