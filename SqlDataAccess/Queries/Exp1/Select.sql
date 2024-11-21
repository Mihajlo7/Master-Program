--1. Get All
SELECT * 
FROM dbo.Task t 
INNER JOIN dbo.Employee r ON (r.id=t.responsible)
LEFT JOIN dbo.Employee s ON (s.id=t.supervisor)
LEFT JOIN dbo.EmployeeTask et ON (t.id=et.taskId)
INNER JOIN dbo.Employee em ON (em.id=et.employeeId);
--2. Get all but with names
SELECT t.id TaskId, t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline, t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone,
s.Id SupervisorId, s.email SupervisorEmail, s.firstName SupervisorFirstName, s.lastName SupervisorLastName,s.birthDay SupervisorBirthDay, s.title SupervisorTitle,s.phone SupervisorPhone,
em.Id EmployeeId,em.email EmployeeEmail, em.firstName EmployeeFirstName, em.lastName EmployeeLastName,em.birthDay EmployeeBirthDay, em.title EmployeeTitle,em.phone EmployeePhone
FROM dbo.Task t 
INNER JOIN dbo.Employee r ON (r.id=t.responsible)
LEFT JOIN dbo.Employee s ON (s.id=t.supervisor)
LEFT JOIN dbo.EmployeeTask et ON (t.id=et.taskId)
INNER JOIN dbo.Employee em ON (em.id=et.employeeId);
--3. Get all but order
SELECT t.id TaskId, t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline, t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone,
s.Id SupervisorId, s.email SupervisorEmail, s.firstName SupervisorFirstName, s.lastName SupervisorLastName,s.birthDay SupervisorBirthDay, s.title SupervisorTitle,s.phone SupervisorPhone,
em.Id EmployeeId,em.email EmployeeEmail, em.firstName EmployeeFirstName, em.lastName EmployeeLastName,em.birthDay EmployeeBirthDay, em.title EmployeeTitle,em.phone EmployeePhone
FROM dbo.Task t 
INNER JOIN dbo.Employee r ON (r.id=t.responsible)
LEFT JOIN dbo.Employee s ON (s.id=t.supervisor)
LEFT JOIN dbo.EmployeeTask et ON (t.id=et.taskId)
INNER JOIN dbo.Employee em ON (em.id=et.employeeId)
ORDER BY t.Id ASC;
--4. Get by Task Id
SELECT t.id TaskId, t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline, t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone,
s.Id SupervisorId, s.email SupervisorEmail, s.firstName SupervisorFirstName, s.lastName SupervisorLastName,s.birthDay SupervisorBirthDay, s.title SupervisorTitle,s.phone SupervisorPhone,
em.Id EmployeeId,em.email EmployeeEmail, em.firstName EmployeeFirstName, em.lastName EmployeeLastName,em.birthDay EmployeeBirthDay, em.title EmployeeTitle,em.phone EmployeePhone
FROM dbo.Task t 
INNER JOIN dbo.Employee r ON (r.id=t.responsible)
LEFT JOIN dbo.Employee s ON (s.id=t.supervisor)
LEFT JOIN dbo.EmployeeTask et ON (t.id=et.taskId)
INNER JOIN dbo.Employee em ON (em.id=et.employeeId)
WHERE t.id=@TaskId;

--5. Get By status and Priority
SELECT t.id TaskId, t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline, t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone,
s.Id SupervisorId, s.email SupervisorEmail, s.firstName SupervisorFirstName, s.lastName SupervisorLastName,s.birthDay SupervisorBirthDay, s.title SupervisorTitle,s.phone SupervisorPhone,
em.Id EmployeeId,em.email EmployeeEmail, em.firstName EmployeeFirstName, em.lastName EmployeeLastName,em.birthDay EmployeeBirthDay, em.title EmployeeTitle,em.phone EmployeePhone
FROM dbo.Task t 
INNER JOIN dbo.Employee r ON (r.id=t.responsible)
LEFT JOIN dbo.Employee s ON (s.id=t.supervisor)
LEFT JOIN dbo.EmployeeTask et ON (t.id=et.taskId)
INNER JOIN dbo.Employee em ON (em.id=et.employeeId)
WHERE t.priority>@Priority AND t.status IN ('Pending','New');
--6. Get By Deadline and
SELECT t.id TaskId, t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline, t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone
FROM dbo.Task t 
INNER JOIN dbo.Employee r ON (r.id=t.responsible)
WHERE t.deadline BETWEEN GETDATE() AND DATEADD(DAY, @Day, GETDATE()) AND t.status !='Completed'
ORDER BY t.deadline ASC;
--7. Get Task by Name Responisble and Supervisor
SELECT t.id TaskId, t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline, t.status TaskStatus
FROM Task t
WHERE t.responsible IN (SELECT id FROM Employee e WHERE e.firstName LIKE @Firstname) AND t.supervisor IN (SELECT id FROM Employee WHERE birthDay<@Birthday);

--8. Get Employee And All Tasks
SELECT e.id Id,e.email Email, COUNT(*) TaskCount
FROM Employee e LEFT JOIN EmployeeTask et ON (e.id=et.employeeId)
GROUP BY e.id, e.email;

--9. Get Employee and their tasks
SELECT e.id Id,e.email Email, COUNT(*) TaskCount
FROM Employee e LEFT JOIN EmployeeTask et ON (e.id=et.employeeId)
GROUP BY e.id, e.email
HAVING COUNT(*)>@NumOfEmployees
ORDER BY  COUNT(*) DESC;
