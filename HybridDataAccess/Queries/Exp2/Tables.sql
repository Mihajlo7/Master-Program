-- Drop table if exists
ALTER TABLE Employee DROP CONSTRAINT IF EXISTS fk_employee_team;
ALTER TABLE Team DROP CONSTRAINT IF EXISTS fk_team_department;
DROP TABLE IF EXISTS Department;
-- Create table Department
CREATE TABLE Department(
	id BIGINT PRIMARY KEY,
	name NVARCHAR(255) NULL,
	location NVARCHAR(255) NULL,
	teams NVARCHAR(MAX) NULL
)
