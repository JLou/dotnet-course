
-- Création de la base de données
CREATE DATABASE SystemeNotes;
GO

USE SystemeNotes;
GO

-- Table Etudiant
CREATE TABLE Etudiant (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nom NVARCHAR(100) NOT NULL
);

-- Table Matiere
CREATE TABLE Matiere (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nom NVARCHAR(100) NOT NULL
);

-- Table Note
CREATE TABLE Note (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Valeur DECIMAL(4,2) NOT NULL,
    DateNote DATE NOT NULL,
    EtudiantId INT FOREIGN KEY REFERENCES Etudiant(Id),
    MatiereId INT FOREIGN KEY REFERENCES Matiere(Id)
);

-- Insertion des étudiants
INSERT INTO Etudiant (Nom) VALUES
('Alice Dupont'),
('Jean Martin'),
('Sophie Durand'),
('Lucas Bernard'),
('Emma Petit'),
('Thomas Leroy'),
('Chloé Moreau'),
('Nathan Robert'),
('Léa Simon'),
('Julien Fontaine');

-- Insertion des matières
INSERT INTO Matiere (Nom) VALUES
('Anglais'),
('.NET'),
('SQL');

-- Génération de ~200 notes aléatoires
DECLARE @i INT = 0;

WHILE @i < 200
BEGIN
    INSERT INTO Note (Valeur, DateNote, EtudiantId, MatiereId)
    VALUES (
        FLOOR(RAND() * 21), -- Note entre 0 et 20
        DATEADD(DAY, -ABS(CHECKSUM(NEWID()) % 365), GETDATE()), -- Date aléatoire dans l'année
        (SELECT TOP 1 Id FROM Etudiant ORDER BY NEWID()), -- Étudiant aléatoire
        (SELECT TOP 1 Id FROM Matiere ORDER BY NEWID()) -- Matière aléatoire
    );
    SET @i = @i + 1;
END;
