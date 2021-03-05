
CREATE TABLE Hotel (
	id int NOT NULL auto_increment,
	name varchar(255) NOT NULL,
	phone varchar(15) NULL,
	PRIMARY KEY (id)
);

CREATE TABLE Rooms (
	id int NOT NULL auto_increment,
	idHotel int NOT NULL,
	roomNumber varchar(5) NOT NULL,
	maxCapacity int NOT NULL DEFAULT(1),
	rate float NOT NULL,
	phoneExtension int NOT NULL,
	PRIMARY KEY (id),
	FOREIGN KEY (idHotel) REFERENCES Hotel(id)
);

CREATE TABLE Bookings (
	id int NOT NULL auto_increment,
	idRoom int NOT NULL,
	startDate DATETIME NOT NULL,
	endDate DATETIME NOT NULL,
	created DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	lastmodified DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
	PRIMARY KEY (id),
	FOREIGN KEY (idRoom) REFERENCES Rooms(id)
);

CREATE TABLE Configurations (
	id int NOT NULL AUTO_INCREMENT,
	maxStayDays SMALLINT NOT NULL,
	maxReserveAdvanceDays SMALLINT NOT NULL
	
);


