DROP TABLE IF EXISTS Task;

DROP PROC IF EXISTS UpdateTasksFromOneEmployeeToAnother;
DROP PROC IF EXISTS UpdateEmployeePhoneById;
DROP PROC IF EXISTS UpdateEmployeePhoneByEmail;


CREATE TABLE Task(
	id BIGINT CONSTRAINT task_id PRIMARY KEY,
	name NVARCHAR(100) NULL,
	description NVARCHAR(1000) NULL,
	priority INT DEFAULT 0,
	deadline DATETIME  DEFAULT GETDATE(),
	status NVARCHAR(10) DEFAULT 'Unknown',
	responsible NVARCHAR(MAX) NULL,
	supervisor NVARCHAR(MAX) NULL,
	employees NVARCHAR(MAX) NULL
);


