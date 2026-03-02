CREATE TABLE IF NOT EXISTS Churches (
  Id INT NOT NULL AUTO_INCREMENT,
  Name VARCHAR(200) NOT NULL,
  City VARCHAR(120) NOT NULL,
  OfficialOrganistsPerService INT NOT NULL,
  PRIMARY KEY (Id)
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS ChurchServiceDays (
  Id INT NOT NULL AUTO_INCREMENT,
  DayOfWeek INT NOT NULL,
  ServiceType INT NOT NULL,
  ChurchId INT NOT NULL,
  PRIMARY KEY (Id),
  KEY IX_ChurchServiceDays_ChurchId (ChurchId),
  CONSTRAINT FK_ChurchServiceDays_Churches_ChurchId FOREIGN KEY (ChurchId)
    REFERENCES Churches (Id) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Organists (
  Id INT NOT NULL AUTO_INCREMENT,
  ChurchId INT NOT NULL,
  Name VARCHAR(200) NOT NULL,
  ShortName VARCHAR(50) NOT NULL,
  Phone VARCHAR(20) NOT NULL,
  CanPlayYouthMeeting TINYINT(1) NOT NULL,
  CanPlayOfficialServices TINYINT(1) NOT NULL,
  CanPlayHalfHour TINYINT(1) NOT NULL,
  PRIMARY KEY (Id),
  KEY IX_Organists_ChurchId (ChurchId),
  CONSTRAINT FK_Organists_Churches_ChurchId FOREIGN KEY (ChurchId)
    REFERENCES Churches (Id) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS OrganistAvailabilities (
  Id INT NOT NULL AUTO_INCREMENT,
  OrganistId INT NOT NULL,
  DayOfWeek INT NOT NULL,
  ServiceType INT NOT NULL,
  PRIMARY KEY (Id),
  KEY IX_OrganistAvailabilities_OrganistId (OrganistId),
  CONSTRAINT FK_OrganistAvailabilities_Organists_OrganistId FOREIGN KEY (OrganistId)
    REFERENCES Organists (Id) ON DELETE CASCADE
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Users (
  Id INT NOT NULL AUTO_INCREMENT,
  Username VARCHAR(100) NOT NULL,
  Password VARCHAR(200) NOT NULL,
  Role INT NOT NULL,
  ChurchId INT NULL,
  OrganistId INT NULL,
  PRIMARY KEY (Id),
  UNIQUE KEY UX_Users_Username (Username),
  KEY IX_Users_ChurchId (ChurchId),
  KEY IX_Users_OrganistId (OrganistId),
  CONSTRAINT FK_Users_Churches_ChurchId FOREIGN KEY (ChurchId) REFERENCES Churches(Id) ON DELETE SET NULL,
  CONSTRAINT FK_Users_Organists_OrganistId FOREIGN KEY (OrganistId) REFERENCES Organists(Id) ON DELETE SET NULL
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS Schedules (
  Id INT NOT NULL AUTO_INCREMENT,
  ChurchId INT NOT NULL,
  StartDate DATE NOT NULL,
  EndDate DATE NOT NULL,
  ServiceType INT NOT NULL,
  CreatedAt DATETIME(6) NOT NULL,
  CreatedByUserId INT NOT NULL,
  PRIMARY KEY (Id),
  KEY IX_Schedules_ChurchId (ChurchId),
  KEY IX_Schedules_CreatedByUserId (CreatedByUserId),
  CONSTRAINT FK_Schedules_Churches_ChurchId FOREIGN KEY (ChurchId) REFERENCES Churches(Id) ON DELETE CASCADE,
  CONSTRAINT FK_Schedules_Users_CreatedByUserId FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id) ON DELETE RESTRICT
) ENGINE=InnoDB;

CREATE TABLE IF NOT EXISTS ScheduleItems (
  Id INT NOT NULL AUTO_INCREMENT,
  ScheduleId INT NOT NULL,
  Date DATE NOT NULL,
  OrganistName VARCHAR(100) NOT NULL,
  HalfHourOrganistName VARCHAR(100) NULL,
  PRIMARY KEY (Id),
  KEY IX_ScheduleItems_ScheduleId (ScheduleId),
  CONSTRAINT FK_ScheduleItems_Schedules_ScheduleId FOREIGN KEY (ScheduleId) REFERENCES Schedules(Id) ON DELETE CASCADE
) ENGINE=InnoDB;

INSERT INTO Users (Id, Username, Password, Role)
SELECT 1, 'master', 'Master@123', 1
WHERE NOT EXISTS (SELECT 1 FROM Users WHERE Username = 'master');
