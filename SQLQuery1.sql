CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(MAX) NOT NULL, -- Hashed and salted
    PhoneNumber NVARCHAR(20),
    Role NVARCHAR(20) DEFAULT 'Donor' CHECK (Role IN ( 'Volunteer', 'Donor'))
);


CREATE TABLE Disasters (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    StartDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NULL, 
    IsActive BIT DEFAULT 1
);


CREATE TABLE Donations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DonorId INT NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    DisasterId INT NULL FOREIGN KEY REFERENCES Disasters(Id) ON DELETE SET NULL, -- Donation can be general
    Amount DECIMAL(10, 2) NOT NULL CHECK (Amount > 0),
    DonationDate DATETIME2 DEFAULT GETUTCDATE(),
    IsAnonymous BIT DEFAULT 0
);


CREATE TABLE GoodsDonations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DonorId INT NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    DisasterId INT NULL FOREIGN KEY REFERENCES Disasters(Id) ON DELETE SET NULL,
    Description NVARCHAR(500) NOT NULL,
    Category NVARCHAR(100) NOT NULL, 
    Quantity INT NOT NULL CHECK (Quantity > 0),
    DonationDate DATETIME2 DEFAULT GETUTCDATE()
);


CREATE TABLE VolunteerShifts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    DisasterId INT NOT NULL FOREIGN KEY REFERENCES Disasters(Id) ON DELETE CASCADE,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    Description NVARCHAR(500),
    RequiredVolunteers INT NOT NULL CHECK (RequiredVolunteers > 0)
);

-- VolunteerRegistrations Table: Links Users to Shifts they've signed up for
CREATE TABLE VolunteerRegistrations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    VolunteerId INT NOT NULL FOREIGN KEY REFERENCES Users(Id) ON DELETE CASCADE,
    ShiftId INT NOT NULL FOREIGN KEY REFERENCES VolunteerShifts(Id) ON DELETE CASCADE,
    RegistrationDate DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT UK_VolunteerShift UNIQUE (VolunteerId, ShiftId) -- Prevents double-booking
);