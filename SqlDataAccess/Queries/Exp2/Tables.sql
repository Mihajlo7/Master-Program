
ALTER TABLE Employee DROP CONSTRAINT IF EXISTS fk_employee_team;
ALTER TABLE Team DROP CONSTRAINT IF EXISTS fk_team_department;
-- Brisanje tabele employee ako postoji
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

-- Kreiranje tabele team (bez foreign key ograničenja)
CREATE TABLE Team (
    id BIGINT PRIMARY KEY,
    name NVARCHAR(255) NULL,
    status NVARCHAR(255) NULL,
    description NVARCHAR(MAX) NULL,
    department_id BIGINT NULL,  -- Ovdje ne dodajemo FOREIGN KEY
    leader_id BIGINT NULL,      -- Ovdje ne dodajemo FOREIGN KEY
);

-- Kreiranje tabele employee (bez foreign key ograničenja)
CREATE TABLE Employee (
    id BIGINT PRIMARY KEY,
    first_name NVARCHAR(255) NULL,
    last_name NVARCHAR(255) NULL,
    email NVARCHAR(255) NULL,
    birth_day DATE NULL,
    title NVARCHAR(255) NULL,
    phone NVARCHAR(50) NULL,
    team_id BIGINT NULL  -- Ovdje ne dodajemo FOREIGN KEY
);

-- Dodavanje foreign key ograničenja za Team tabelu
ALTER TABLE Team
ADD CONSTRAINT fk_team_department FOREIGN KEY (department_id) REFERENCES Department(id) ON DELETE SET NULL;

--ALTER TABLE Team
--ADD CONSTRAINT fk_team_leader FOREIGN KEY (leader_id) REFERENCES Employee(id) ON DELETE SET NULL;

-- Dodavanje foreign key ograničenja za Employee tabelu
ALTER TABLE Employee
ADD CONSTRAINT fk_employee_team FOREIGN KEY (team_id) REFERENCES Team(id) ON DELETE SET NULL;

