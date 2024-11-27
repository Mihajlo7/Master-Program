DROP TABLE IF EXISTS Developer;
DROP TABLE IF EXISTS Employee;

CREATE TABLE Employee (
    id BIGINT PRIMARY KEY,
    firstname NVARCHAR(255) NOT NULL,
    lastname NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) NOT NULL,
    birthday DATE NOT NULL,
    title NVARCHAR(255) NOT NULL,
    phone NVARCHAR(50) NOT NULL,
    manager NVARCHAR(MAX) NULL
);

CREATE TABLE Developer (
    id BIGINT PRIMARY KEY FOREIGN KEY REFERENCES employee(id) ON DELETE CASCADE,
    seniority NVARCHAR(255) NOT NULL,
    yearsOfExperience INT NOT NULL,
    isRemote BIT NOT NULL,
    softwareDeveloper NVARCHAR(MAX) NULL
);