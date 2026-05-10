BEGIN TRANSACTION;
CREATE TABLE AspNetRoles (
    Id nvarchar(128) NOT NULL,
    Name nvarchar(256) NULL,
    NormalizedName nvarchar(256) NULL,
    ConcurrencyStamp nvarchar(max) NULL,
    CONSTRAINT PK_AspNetRoles PRIMARY KEY (Id)
);

CREATE TABLE AspNetUsers (
    Id nvarchar(128) NOT NULL,
    FullName nvarchar(100) NOT NULL,
    Role nvarchar(50) NOT NULL,
    CreatedAt datetime2 NOT NULL,
    ProfileImageUrl nvarchar(500) NULL,
    IsActive bit NOT NULL,
    UserName nvarchar(256) NULL,
    NormalizedUserName nvarchar(256) NULL,
    Email nvarchar(256) NULL,
    NormalizedEmail nvarchar(256) NULL,
    EmailConfirmed bit NOT NULL,
    PasswordHash nvarchar(max) NULL,
    SecurityStamp nvarchar(max) NULL,
    ConcurrencyStamp nvarchar(max) NULL,
    PhoneNumber nvarchar(max) NULL,
    PhoneNumberConfirmed bit NOT NULL,
    TwoFactorEnabled bit NOT NULL,
    LockoutEnd datetimeoffset NULL,
    LockoutEnabled bit NOT NULL,
    AccessFailedCount int NOT NULL,
    CONSTRAINT PK_AspNetUsers PRIMARY KEY (Id)
);

CREATE TABLE AspNetRoleClaims (
    Id int NOT NULL IDENTITY,
    RoleId nvarchar(128) NOT NULL,
    ClaimType nvarchar(max) NULL,
    ClaimValue nvarchar(max) NULL,
    CONSTRAINT PK_AspNetRoleClaims PRIMARY KEY (Id),
    CONSTRAINT FK_AspNetRoleClaims_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserClaims (
    Id int NOT NULL IDENTITY,
    UserId nvarchar(128) NOT NULL,
    ClaimType nvarchar(max) NULL,
    ClaimValue nvarchar(max) NULL,
    CONSTRAINT PK_AspNetUserClaims PRIMARY KEY (Id),
    CONSTRAINT FK_AspNetUserClaims_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserLogins (
    LoginProvider nvarchar(128) NOT NULL,
    ProviderKey nvarchar(128) NOT NULL,
    ProviderDisplayName nvarchar(max) NULL,
    UserId nvarchar(128) NOT NULL,
    CONSTRAINT PK_AspNetUserLogins PRIMARY KEY (LoginProvider, ProviderKey),
    CONSTRAINT FK_AspNetUserLogins_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserRoles (
    UserId nvarchar(128) NOT NULL,
    RoleId nvarchar(128) NOT NULL,
    CONSTRAINT PK_AspNetUserRoles PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_AspNetUserRoles_AspNetRoles_RoleId FOREIGN KEY (RoleId) REFERENCES AspNetRoles (Id) ON DELETE CASCADE,
    CONSTRAINT FK_AspNetUserRoles_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE AspNetUserTokens (
    UserId nvarchar(128) NOT NULL,
    LoginProvider nvarchar(128) NOT NULL,
    Name nvarchar(128) NOT NULL,
    Value nvarchar(max) NULL,
    CONSTRAINT PK_AspNetUserTokens PRIMARY KEY (UserId, LoginProvider, Name),
    CONSTRAINT FK_AspNetUserTokens_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE Customers (
    Id int NOT NULL IDENTITY,
    FirstName nvarchar(50) NOT NULL,
    LastName nvarchar(50) NOT NULL,
    Email nvarchar(100) NOT NULL,
    Phone nvarchar(20) NULL,
    Company nvarchar(100) NULL,
    Address nvarchar(200) NULL,
    City nvarchar(50) NULL,
    State nvarchar(50) NULL,
    ZipCode nvarchar(10) NULL,
    Notes nvarchar(1000) NULL,
    CreatedAt datetime2 NOT NULL,
    CreatedByUserId nvarchar(128) NOT NULL,
    CONSTRAINT PK_Customers PRIMARY KEY (Id),
    CONSTRAINT FK_Customers_AspNetUsers_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES AspNetUsers (Id) ON DELETE NO ACTION
);

CREATE TABLE Leads (
    Id int NOT NULL IDENTITY,
    ContactName nvarchar(100) NOT NULL,
    Email nvarchar(100) NOT NULL,
    Phone nvarchar(20) NULL,
    Company nvarchar(100) NULL,
    Source nvarchar(30) NOT NULL,
    Status nvarchar(30) NOT NULL,
    EstimatedValue decimal(18,2) NOT NULL,
    Notes nvarchar(1000) NULL,
    CreatedAt datetime2 NOT NULL,
    AssignedToUserId nvarchar(128) NULL,
    CONSTRAINT PK_Leads PRIMARY KEY (Id),
    CONSTRAINT FK_Leads_AspNetUsers_AssignedToUserId FOREIGN KEY (AssignedToUserId) REFERENCES AspNetUsers (Id) ON DELETE SET NULL
);

CREATE TABLE Notifications (
    Id int NOT NULL IDENTITY,
    Title nvarchar(150) NOT NULL,
    Message nvarchar(1000) NOT NULL,
    Type nvarchar(30) NOT NULL,
    IsRead bit NOT NULL,
    CreatedAt datetime2 NOT NULL,
    UserId nvarchar(128) NULL,
    CONSTRAINT PK_Notifications PRIMARY KEY (Id),
    CONSTRAINT FK_Notifications_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE
);

CREATE TABLE TaskItems (
    Id int NOT NULL IDENTITY,
    Title nvarchar(150) NOT NULL,
    Description nvarchar(1000) NULL,
    DueDate datetime2 NULL,
    Priority nvarchar(20) NOT NULL,
    Status nvarchar(20) NOT NULL,
    AssignedToUserId nvarchar(128) NULL,
    CreatedByUserId nvarchar(128) NOT NULL,
    CreatedAt datetime2 NOT NULL,
    CONSTRAINT PK_TaskItems PRIMARY KEY (Id),
    CONSTRAINT FK_TaskItems_AspNetUsers_AssignedToUserId FOREIGN KEY (AssignedToUserId) REFERENCES AspNetUsers (Id) ON DELETE SET NULL,
    CONSTRAINT FK_TaskItems_AspNetUsers_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES AspNetUsers (Id) ON DELETE NO ACTION
);

CREATE TABLE Appointments (
    Id int NOT NULL IDENTITY,
    Title nvarchar(150) NOT NULL,
    Description nvarchar(500) NULL,
    StartTime datetime2 NOT NULL,
    EndTime datetime2 NOT NULL,
    Location nvarchar(200) NULL,
    CustomerId int NULL,
    CreatedByUserId nvarchar(128) NOT NULL,
    CreatedAt datetime2 NOT NULL,
    CONSTRAINT PK_Appointments PRIMARY KEY (Id),
    CONSTRAINT FK_Appointments_AspNetUsers_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES AspNetUsers (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Appointments_Customers_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE SET NULL
);

CREATE TABLE Invoices (
    Id int NOT NULL IDENTITY,
    InvoiceNumber nvarchar(20) NOT NULL,
    CustomerId int NOT NULL,
    IssueDate datetime2 NOT NULL,
    DueDate datetime2 NOT NULL,
    TotalAmount decimal(18,2) NOT NULL,
    Status nvarchar(30) NOT NULL,
    Notes nvarchar(1000) NULL,
    CreatedByUserId nvarchar(128) NOT NULL,
    CreatedAt datetime2 NOT NULL,
    CONSTRAINT PK_Invoices PRIMARY KEY (Id),
    CONSTRAINT FK_Invoices_AspNetUsers_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES AspNetUsers (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Invoices_Customers_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE NO ACTION
);

CREATE TABLE UploadedDocuments (
    Id int NOT NULL IDENTITY,
    FileName nvarchar(255) NOT NULL,
    OriginalFileName nvarchar(255) NOT NULL,
    ContentType nvarchar(100) NOT NULL,
    FileSize bigint NOT NULL,
    UploadedAt datetime2 NOT NULL,
    CustomerId int NULL,
    UploadedByUserId nvarchar(128) NOT NULL,
    CONSTRAINT PK_UploadedDocuments PRIMARY KEY (Id),
    CONSTRAINT FK_UploadedDocuments_AspNetUsers_UploadedByUserId FOREIGN KEY (UploadedByUserId) REFERENCES AspNetUsers (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_UploadedDocuments_Customers_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE SET NULL
);

CREATE TABLE Payments (
    Id int NOT NULL IDENTITY,
    InvoiceId int NOT NULL,
    Amount decimal(18,2) NOT NULL,
    PaymentDate datetime2 NOT NULL,
    PaymentMethod nvarchar(30) NOT NULL,
    ReferenceNumber nvarchar(50) NULL,
    Notes nvarchar(500) NULL,
    CreatedAt datetime2 NOT NULL,
    CONSTRAINT PK_Payments PRIMARY KEY (Id),
    CONSTRAINT FK_Payments_Invoices_InvoiceId FOREIGN KEY (InvoiceId) REFERENCES Invoices (Id) ON DELETE CASCADE
);

CREATE INDEX IX_Appointments_CreatedByUserId ON Appointments (CreatedByUserId);

CREATE INDEX IX_Appointments_CustomerId ON Appointments (CustomerId);

CREATE INDEX IX_AspNetRoleClaims_RoleId ON AspNetRoleClaims (RoleId);

CREATE UNIQUE INDEX RoleNameIndex ON AspNetRoles (NormalizedName) WHERE NormalizedName IS NOT NULL;

CREATE INDEX IX_AspNetUserClaims_UserId ON AspNetUserClaims (UserId);

CREATE INDEX IX_AspNetUserLogins_UserId ON AspNetUserLogins (UserId);

CREATE INDEX IX_AspNetUserRoles_RoleId ON AspNetUserRoles (RoleId);

CREATE INDEX EmailIndex ON AspNetUsers (NormalizedEmail);

CREATE UNIQUE INDEX UserNameIndex ON AspNetUsers (NormalizedUserName) WHERE NormalizedUserName IS NOT NULL;

CREATE INDEX IX_Customers_CreatedByUserId ON Customers (CreatedByUserId);

CREATE INDEX IX_Customers_Email ON Customers (Email);

CREATE INDEX IX_Invoices_CreatedByUserId ON Invoices (CreatedByUserId);

CREATE INDEX IX_Invoices_CustomerId ON Invoices (CustomerId);

CREATE UNIQUE INDEX IX_Invoices_InvoiceNumber ON Invoices (InvoiceNumber);

CREATE INDEX IX_Leads_AssignedToUserId ON Leads (AssignedToUserId);

CREATE INDEX IX_Leads_Email ON Leads (Email);

CREATE INDEX IX_Notifications_UserId ON Notifications (UserId);

CREATE INDEX IX_Payments_InvoiceId ON Payments (InvoiceId);

CREATE INDEX IX_TaskItems_AssignedToUserId ON TaskItems (AssignedToUserId);

CREATE INDEX IX_TaskItems_CreatedByUserId ON TaskItems (CreatedByUserId);

CREATE INDEX IX_UploadedDocuments_CustomerId ON UploadedDocuments (CustomerId);

CREATE INDEX IX_UploadedDocuments_UploadedByUserId ON UploadedDocuments (UploadedByUserId);

COMMIT;
GO



