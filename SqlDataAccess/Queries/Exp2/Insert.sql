-- Unos podataka u tabelu Department
INSERT INTO Department (id, name, location)
VALUES (@Id, @Name, @Location);

-- Unos podataka u tabelu Employee
INSERT INTO Employee (id, first_name, last_name, email, birth_day, title, phone)
VALUES (@Id, @FirstName, @LastName, @Email, @BirthDay, @Title, @Phone);

-- Unos podataka u tabelu Team
INSERT INTO Team (id, name, status, description, department_id, leader_id)
VALUES (@Id, @Name, @Status, @Description, @DepartmentId, @LeaderId);

