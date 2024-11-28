DROP TABLE IF EXISTS Employee;

-- Brisanje tabele team ako postoji
DROP TABLE IF EXISTS Team;

-- Brisanje tabele department ako postoji
DROP TABLE IF EXISTS Department;

-- Kreiranje tabele department
CREATE TABLE Department (
    id BIGINT PRIMARY KEY,
    name NVARCHAR(255) NULL,
    location NVARCHAR(255) NULL
);

-- Kreiranje tabele team
CREATE TABLE Team (
    id BIGINT PRIMARY KEY ,
    name NVARCHAR(255) NULL,
    status NVARCHAR(255) NULL,
    description NVARCHAR(MAX) NULL,
    department_id BIGINT NULL, -- Foreign key ka department
    leader_id BIGINT NULL, -- Foreign key ka employee
    CONSTRAINT fk_team_department FOREIGN KEY (department_id) REFERENCES department(id) ON DELETE SET NULL,
    CONSTRAINT fk_team_leader FOREIGN KEY (leader_id) REFERENCES employee(id) ON DELETE SET NULL
);

-- Kreiranje tabele employee
CREATE TABLE Employee (
    id BIGINT PRIMARY KEY ,
    first_name NVARCHAR(255) NULL,
    last_name NVARCHAR(255) NULL,
    email NVARCHAR(255) NULL,
    birth_day DATE NULL,
    title NVARCHAR(255) NULL,
    phone NVARCHAR(50) NULL,
    team_id BIGINT NULL, -- Foreign key ka team
    CONSTRAINT fk_employee_team FOREIGN KEY (team_id) REFERENCES team(id) ON DELETE SET NULL
);
