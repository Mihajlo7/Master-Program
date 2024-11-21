DROP TABLE IF EXISTS EmployeeTask;
DROP TABLE IF EXISTS Task;
DROP TABLE IF EXISTS Employee;

CREATE TABLE Employee(
	id BIGINT CONSTRAINT employee_pk PRIMARY KEY,
	firstName NVARCHAR(50) NULL,
	lastName NVARCHAR(50) NULL,
	email NVARCHAR(80) NULL,
	birthDay DATETIME DEFAULT GETDATE(),
	title NVARCHAR(100), 
	phone NVARCHAR(20),
);
CREATE TABLE Task(
	id BIGINT CONSTRAINT task_id PRIMARY KEY,
	name NVARCHAR(100) NULL,
	description NVARCHAR(1000) NULL,
	priority INT DEFAULT 0,
	deadline DATETIME  DEFAULT GETDATE(),
	status NVARCHAR(10) DEFAULT 'Unknown',
	responsible BIGINT CONSTRAINT responsible_fk FOREIGN KEY (responsible) REFERENCES Employee(id),
	supervisor BIGINT CONSTRAINT supervisor_fk FOREIGN KEY (supervisor) REFERENCES Employee(id)
);
CREATE TABLE EmployeeTask(
	employeeId BIGINT,
	taskId BIGINT,
	CONSTRAINT employee_task_pk PRIMARY KEY(employeeId,taskId),
	CONSTRAINT employee_fk FOREIGN KEY(employeeId) REFERENCES Employee(id),
	CONSTRAINT task_fk FOREIGN KEY(taskId) REFERENCES Task(id) ON DELETE CASCADE 
);
