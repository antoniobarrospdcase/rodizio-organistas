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
