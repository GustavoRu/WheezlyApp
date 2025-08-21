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

CREATE TABLE Cars (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Year INT NOT NULL,
    MakeId INT NOT NULL,
    ModelId INT NOT NULL,
    SubModelId INT NOT NULL,
    ZipCodeId INT NOT NULL;
    CreatedDate DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsActive BIT NOT NULL DEFAULT 1,
    
    -- llaves foraneas
    CONSTRAINT FK_Cars_Makes FOREIGN KEY (MakeId) REFERENCES Makes(Id),
    CONSTRAINT FK_Cars_Models FOREIGN KEY (ModelId) REFERENCES Models(Id),
    CONSTRAINT FK_Cars_SubModels FOREIGN KEY (SubModelId) REFERENCES SubModels(Id),
    CONSTRAINT FK_Cars_ZipCodes FOREIGN KEY (ZipCodeId) REFERENCES ZipCodes(Id)
);

INSERT INTO Makes (Name) VALUES ('Toyota'), ('BMW'), ('Volkswagen');
INSERT INTO Models (MakeId, Name) VALUES (1, 'Corolla'), (2, 'X5'), (3, 'Gol');
INSERT INTO SubModels (ModelId, Name) VALUES (1, 'LE'), (2, 'X5'), (3, 'Trendline');

CREATE TABLE ZipCodes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ZipCode NVARCHAR(10) NOT NULL,
    CONSTRAINT UK_ZipCodes_ZipCode UNIQUE (ZipCode)
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
    (StatusId IN (SELECT Id FROM Status WHERE RequiresDate = 1) AND StatusDate IS NOT NULL) OR
    (StatusId IN (SELECT Id FROM Status WHERE RequiresDate = 0))
);

INSERT INTO Status (Name, RequiresDate) VALUES 
    ('Pending Acceptance', 0),
    ('Accepted', 0),
    ('Picked Up', 1);

INSERT INTO Users (Username, Email) VALUES 
    ('admin', 'admin@wheezly.com'),
    ('operator1', 'operator1@wheezly.com'),
    ('operator2', 'operator2@wheezly.com');    