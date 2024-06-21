DROP TABLE IF EXISTS PasswordHash;
DROP TABLE IF EXISTS UserInformation;
DROP TABLE IF EXISTS ContactInformation;
DROP TABLE IF EXISTS HistoryStateTable;
DROP TABLE IF EXISTS Devices;
DROP TABLE IF EXISTS Users;



-- Create User table
CREATE TABLE Users
(
    Id    SERIAL PRIMARY KEY,
    Email VARCHAR(255) NOT NULL UNIQUE,
    Username varchar(255)
);

--Create device table
CREATE TABLE Devices
(
    Id         SERIAL PRIMARY KEY,
    DeviveName VARCHAR(255),
    UserId     INT,
    FOREIGN KEY (UserId) REFERENCES Users (Id)
);

--Create the table for the history of the fountain
CREATE TABLE HistoryStateTable
(
    DeviceId        INT NOT NULL,
    isOn            bool not null,
    temperatur      float not null,
    timestamp       varchar(50) NOT NULL,
    FOREIGN KEY (DeviceId) REFERENCES Devices (Id)
);

-- Create PasswordHash table
CREATE TABLE PasswordHash
(
    UserId    INT PRIMARY KEY,
    Hash      TEXT         NOT NULL,
    Salt      VARCHAR(255) NOT NULL,
    Algorithm VARCHAR(50)  NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id)
);

DELETE FROM HistoryStateTable WHERE DeviceId = 1;

--insert mock data
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,true,25.25,  '2024-06-10 09:04:21');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,false,24.65, '2024-06-10 09:06:00');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,true,25.25,  '2024-06-11 10:00:30');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,false,27.5,  '2024-06-11 10:04:59');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,true,28.5,   '2024-06-12 14:04:28');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,false,14.25, '2024-06-12 14:09:21');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,true,22.25,  '2024-06-13 18:30:00');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,false,10.62, '2024-06-13 18:36:35');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,true,9.5,    '2024-06-14 20:04:21');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,false,5.5,   '2024-06-14 20:09:28');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,true,7.7,    '2024-06-15 21:59:21');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,false,7.7,   '2024-06-15 22:03:26');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,true,5.7,   '2024-06-16 21:03:26');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,false,4.7,   '2024-06-16 21:10:26');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,true,18.7,   '2024-06-17 10:30:26');
INSERT INTO HistoryStateTable(DeviceId, isOn, temperatur, timestamp) VALUES (1,false,18.7,   '2024-06-17 10:35:36');