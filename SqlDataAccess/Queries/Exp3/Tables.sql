DROP TABLE IF EXISTS Employee;
DROP TABLE IF EXISTS Intern;
DROP TABLE IF EXISTS Manager;
DROP TABLE IF EXISTS Developer;
DROP TABLE IF EXISTS SoftwareDeveloper;
DROP TABLE IF EXISTS DatabaseDeveloper;

CREATE TABLE Employee (
    id BIGINT PRIMARY KEY,
    firstname NVARCHAR(255) NOT NULL,
    lastname NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL,
    birthday DATE NOT NULL,
    title NVARCHAR(255) NOT NULL,
    phone NVARCHAR(50) NOT NULL
);

CREATE TABLE Developer (
    id BIGINT PRIMARY KEY FOREIGN KEY REFERENCES employee(id),
    seniority NVARCHAR(255) NOT NULL,
    yearsOfExperience INT NOT NULL,
    isRemote BIT NOT NULL
);

CREATE TABLE Intern (
    id BIGINT PRIMARY KEY FOREIGN KEY REFERENCES employee(id),
    university NVARCHAR(255),
    studyYear INT NOT NULL,
    city NVARCHAR(255) NOT NULL
);

CREATE TABLE Manager (
    id BIGINT PRIMARY KEY FOREIGN KEY REFERENCES employee(id),
    department NVARCHAR(255) NOT NULL,
    realisedProject INT NOT NULL,
    method NVARCHAR(255) NOT NULL
);

CREATE TABLE SoftwareDeveloper (
    id BIGINT PRIMARY KEY FOREIGN KEY REFERENCES developer(id),
    programmingLanguage NVARCHAR(255) NOT NULL,
    ide NVARCHAR(255) NOT NULL,
    isfullstack BIT NOT NULL
);

CREATE TABLE DatabaseDeveloper (
    id BIGINT PRIMARY KEY FOREIGN KEY REFERENCES developer(id),
    provider NVARCHAR(255) NOT NULL,
    isAdmin BIT NOT NULL,
    knownosql BIT NOT NULL
);
