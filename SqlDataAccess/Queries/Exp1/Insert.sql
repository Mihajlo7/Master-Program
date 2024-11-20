INSERT INTO Employee(id,firstName,lastName,email,birthDay,title,phone)
VALUES(@EmployeeId,@EmployeeFirstname,@EmployeeLastname,@EmployeeEmail,@EmployeeBirthday,@EmployeeTitle,@EmployeePhone);

INSERT INTO Task(id,name,description,priority,deadline,status,responsible,supervisor)
VALUES(@TaskId,@TaskName,@TaskDescription,@TaskPriority,@TaskDeadline,@TaskStatus,@Responsible,@Supervisor);

INSERT INTO EmployeeTask(employeeId,taskId) VALUES(@EmployeeId,@TaskId);
