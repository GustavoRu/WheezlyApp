USE master;
ALTER DATABASE WheezlyDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE IF EXISTS WheezlyDB;
CREATE DATABASE WheezlyDB;
USE WheezlyDB;

--creo una tabla usuarios ya que en el enunciado que se debe llevar registro de quien cambia el estado
-- asumo que no es el propio comprador quien cambia el estado, sino el usuario del sistema
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT UK_Users_Username UNIQUE (Username),
    CONSTRAINT UK_Users_Email UNIQUE (Email)
);

CREATE TABLE Makes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT UK_Makes_Name UNIQUE (Name)
);

CREATE TABLE Models (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    MakeId INT NOT NULL,
    Name NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Models_Makes FOREIGN KEY (MakeId) REFERENCES Makes(Id),
    CONSTRAINT UK_Models_MakeId_Name UNIQUE (MakeId, Name)
);

CREATE TABLE SubModels (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ModelId INT NOT NULL,
    Name NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_SubModels_Models FOREIGN KEY (ModelId) REFERENCES Models(Id),
    CONSTRAINT UK_SubModels_ModelId_Name UNIQUE (ModelId, Name)
);

CREATE TABLE ZipCodes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ZipCode NVARCHAR(10) NOT NULL,
    CONSTRAINT UK_ZipCodes_ZipCode UNIQUE (ZipCode)
);

CREATE TABLE Cars (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Year INT NOT NULL,
    MakeId INT NOT NULL,
    ModelId INT NOT NULL,
    SubModelId INT NOT NULL,
    ZipCodeId INT NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    
    -- llaves foraneas
    CONSTRAINT FK_Cars_Makes FOREIGN KEY (MakeId) REFERENCES Makes(Id),
    CONSTRAINT FK_Cars_Models FOREIGN KEY (ModelId) REFERENCES Models(Id),
    CONSTRAINT FK_Cars_SubModels FOREIGN KEY (SubModelId) REFERENCES SubModels(Id),
    CONSTRAINT FK_Cars_ZipCodes FOREIGN KEY (ZipCodeId) REFERENCES ZipCodes(Id)
);


CREATE TABLE Buyers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT UK_Buyers_Email UNIQUE (Email)
);

CREATE TABLE BuyerZipCodes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BuyerId INT NOT NULL,
    ZipCodeId INT NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_BuyerZipCodes_Buyers FOREIGN KEY (BuyerId) REFERENCES Buyers(Id),
    CONSTRAINT FK_BuyerZipCodes_ZipCodes FOREIGN KEY (ZipCodeId) REFERENCES ZipCodes(Id),
    CONSTRAINT UK_BuyerZipCodes_Buyer_ZipCode UNIQUE (BuyerId, ZipCodeId)
);

CREATE TABLE Quotes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CarId INT NOT NULL,
    BuyerId INT NOT NULL,
    CurrentAmount DECIMAL(10, 2) NOT NULL,
    IsCurrentQuote BIT NOT NULL DEFAULT 0,
    ZipCodeId INT NOT NULL,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_Quotes_Cars FOREIGN KEY (CarId) REFERENCES Cars(Id),
    CONSTRAINT FK_Quotes_Buyers FOREIGN KEY (BuyerId) REFERENCES Buyers(Id),
    CONSTRAINT FK_Quotes_ZipCodes FOREIGN KEY (ZipCodeId) REFERENCES ZipCodes(Id)
)

CREATE TABLE Status (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    RequiresDate BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT UK_Status_Name UNIQUE (Name)
);

CREATE TABLE StatusHistory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CarId INT NOT NULL,
    StatusId INT NOT NULL,
    ChangedByUserId INT NOT NULL,
    StatusDate DATETIME2,
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CONSTRAINT FK_StatusHistory_Users FOREIGN KEY (ChangedByUserId) REFERENCES Users(Id),
    CONSTRAINT FK_StatusHistory_Cars FOREIGN KEY (CarId) REFERENCES Cars(Id),
    CONSTRAINT FK_StatusHistory_Status FOREIGN KEY (StatusId) REFERENCES Status(Id),
    CONSTRAINT CHK_StatusHistory_PickedUpDate CHECK (
        (StatusDate IS NOT NULL AND StatusId = 3) OR  -- Para Picked Up (Id=3)
        (StatusId IN (1, 2))                          -- Para otros estados
    )
)

INSERT INTO Makes (Name) VALUES ('Toyota'), ('BMW'), ('Volkswagen');
INSERT INTO Models (MakeId, Name) VALUES (1, 'Corolla'), (2, 'X5'), (3, 'Gol');
INSERT INTO SubModels (ModelId, Name) VALUES (1, 'LE'), (2, 'X5'), (3, 'Trendline');

INSERT INTO Status (Name, RequiresDate) VALUES 
    ('Pending Acceptance', 0),
    ('Accepted', 0),
    ('Picked Up', 1);

INSERT INTO Users (Username, Email) VALUES 
    ('admin', 'admin@wheezly.com'),
    ('operator1', 'operator1@wheezly.com'),
    ('operator2', 'operator2@wheezly.com');    

-- 1. Primero insertamos algunos ZipCodes
INSERT INTO ZipCodes (ZipCode) VALUES 
    ('90210'),
    ('10001'),
    ('60601');

-- 2. Insertamos algunos Buyers
INSERT INTO Buyers (Name, Email) VALUES
    ('AutoMax Corp', 'purchases@automax.com'),
    ('CarBuyers Inc', 'deals@carbuyers.com'),
    ('Elite Auto', 'buy@eliteauto.com');

-- 3. Relacionamos Buyers con ZipCodes
INSERT INTO BuyerZipCodes (BuyerId, ZipCodeId) VALUES
    (1, 1), -- AutoMax cubre 90210
    (1, 2), -- AutoMax también cubre 10001
    (2, 2), -- CarBuyers cubre 10001
    (3, 3); -- Elite Auto cubre 60601

-- 4. Insertamos algunos Cars (usando los Makes, Models y SubModels que ya tenemos)
INSERT INTO Cars (Year, MakeId, ModelId, SubModelId, ZipCodeId) VALUES
    (2020, 1, 1, 1, 1), -- Toyota Corolla LE en 90210
    (2021, 2, 2, 2, 2), -- BMW X5 en 10001
    (2019, 3, 3, 3, 3); -- VW Gol Trendline en 60601

-- 5. Insertamos algunas Quotes
INSERT INTO Quotes (CarId, BuyerId, CurrentAmount, IsCurrentQuote, ZipCodeId) VALUES
    (1, 1, 15000.00, 1, 1),    -- Oferta actual para el Toyota
    (1, 2, 14500.00, 0, 1),    -- Oferta no actual para el Toyota
    (2, 2, 35000.00, 1, 2),    -- Oferta actual para el BMW
    (3, 3, 12000.00, 1, 3);    -- Oferta actual para el VW

-- 6. Insertamos algunos StatusHistory (asumiendo que ya tenemos los Status insertados)
INSERT INTO StatusHistory (CarId, StatusId, ChangedByUserId, StatusDate) VALUES
    (1, 1, 1, NULL),                           -- Toyota en Pending Acceptance
    (2, 2, 2, NULL),                           -- BMW Accepted
    (3, 3, 3, DATEADD(day, -1, GETUTCDATE())); -- VW Picked Up (requiere fecha)    