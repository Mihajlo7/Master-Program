-- 1. All With Serialization in code
SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus, t.responsible TaskResponsible,t.supervisor TaskSupervisor,t.employees TaskEmployees
FROM dbo.Task t;
-- 2. All With Serialization on database
SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone,
s.Id SupervisorId, s.email SupervisorEmail, s.firstName SupervisorFirstName, s.lastName SupervisorLastName,s.birthDay SupervisorBirthDay, s.title SupervisorTitle,s.phone SupervisorPhone,
em.Id EmployeeId,em.email EmployeeEmail, em.firstName EmployeeFirstName, em.lastName EmployeeLastName,em.birthDay EmployeeBirthDay, em.title EmployeeTitle,em.phone EmployeePhone
FROM dbo.Task t
CROSS APPLY OPENJSON(responsible) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS r
OUTER APPLY OPENJSON(supervisor) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS  s
CROSS APPLY OPENJSON(employees) WITH(
	id BIGINT '$.Emloyee.Id',
	firstname NVARCHAR(100) '$.Emloyee.FirstName',
	lastname NVARCHAR(100) '$.Emloyee.LastName',
	email NVARCHAR(100) '$.Emloyee.Email',
	birthday NVARCHAR(100) '$.Emloyee.BirthDay',
	title NVARCHAR(200) '$.Emloyee.Title',
	phone NVARCHAR(100) '$.Emloyee.Phone'
) AS em;

-- 3. Get All and Sorted
SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone,
s.Id SupervisorId, s.email SupervisorEmail, s.firstName SupervisorFirstName, s.lastName SupervisorLastName,s.birthDay SupervisorBirthDay, s.title SupervisorTitle,s.phone SupervisorPhone,
em.Id EmployeeId,em.email EmployeeEmail, em.firstName EmployeeFirstName, em.lastName EmployeeLastName,em.birthDay EmployeeBirthDay, em.title EmployeeTitle,em.phone EmployeePhone
FROM dbo.Task t
CROSS APPLY OPENJSON(responsible) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS r
OUTER APPLY OPENJSON(supervisor) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS  s
CROSS APPLY OPENJSON(employees) WITH(
	id BIGINT '$.Emloyee.Id',
	firstname NVARCHAR(100) '$.Emloyee.FirstName',
	lastname NVARCHAR(100) '$.Emloyee.LastName',
	email NVARCHAR(100) '$.Emloyee.Email',
	birthday NVARCHAR(100) '$.Emloyee.BirthDay',
	title NVARCHAR(200) '$.Emloyee.Title',
	phone NVARCHAR(100) '$.Emloyee.Phone'
) AS em
ORDER BY t.id ASC;
-- 4. Get Task By Task Id 
SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone,
s.Id SupervisorId, s.email SupervisorEmail, s.firstName SupervisorFirstName, s.lastName SupervisorLastName,s.birthDay SupervisorBirthDay, s.title SupervisorTitle,s.phone SupervisorPhone,
em.Id EmployeeId,em.email EmployeeEmail, em.firstName EmployeeFirstName, em.lastName EmployeeLastName,em.birthDay EmployeeBirthDay, em.title EmployeeTitle,em.phone EmployeePhone
FROM dbo.Task t
CROSS APPLY OPENJSON(responsible) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS r
OUTER APPLY OPENJSON(supervisor) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS  s
CROSS APPLY OPENJSON(employees) WITH(
	id BIGINT '$.Emloyee.Id',
	firstname NVARCHAR(100) '$.Emloyee.FirstName',
	lastname NVARCHAR(100) '$.Emloyee.LastName',
	email NVARCHAR(100) '$.Emloyee.Email',
	birthday NVARCHAR(100) '$.Emloyee.BirthDay',
	title NVARCHAR(200) '$.Emloyee.Title',
	phone NVARCHAR(100) '$.Emloyee.Phone'
) AS em
WHERE t.id=@TaskId;
-- 5. Get by status and priority
SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone,
s.Id SupervisorId, s.email SupervisorEmail, s.firstName SupervisorFirstName, s.lastName SupervisorLastName,s.birthDay SupervisorBirthDay, s.title SupervisorTitle,s.phone SupervisorPhone,
em.Id EmployeeId,em.email EmployeeEmail, em.firstName EmployeeFirstName, em.lastName EmployeeLastName,em.birthDay EmployeeBirthDay, em.title EmployeeTitle,em.phone EmployeePhone
FROM dbo.Task t
CROSS APPLY OPENJSON(responsible) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS r
OUTER APPLY OPENJSON(supervisor) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS  s
CROSS APPLY OPENJSON(employees) WITH(
	id BIGINT '$.Emloyee.Id',
	firstname NVARCHAR(100) '$.Employee.FirstName',
	lastname NVARCHAR(100) '$.Employee.LastName',
	email NVARCHAR(100) '$.Employee.Email',
	birthday NVARCHAR(100) '$.Employee.BirthDay',
	title NVARCHAR(200) '$.Employee.Title',
	phone NVARCHAR(100) '$.Employee.Phone'
) AS em
WHERE t.priority>@Priority AND t.status IN ('Pending','New');
-- 6. Get By Deadline and Sorted
SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus,
r.Id ResponsibleId, r.email ResponsibleEmail, r.firstName ResponsibleFirstName, r.lastName ResponsibleLastName,r.birthDay ResponsibleBirthDay, r.title ResponsibleTitle,r.phone ResponsiblePhone
FROM dbo.Task t
CROSS APPLY OPENJSON(responsible) WITH(
	id BIGINT '$.Id',
	firstname NVARCHAR(100) '$.FirstName',
	lastname NVARCHAR(100) '$.LastName',
	email NVARCHAR(100) '$.Email',
	birthday NVARCHAR(100) '$.BirthDay',
	title NVARCHAR(200) '$.Title',
	phone NVARCHAR(100) '$.Phone'
)AS r
WHERE t.deadline BETWEEN GETDATE() AND DATEADD(DAY, @Day, GETDATE()) AND t.status !='Completed'
ORDER BY t.deadline ASC;

-- 7. Get by Responsible Name And Supervisor BirthDay
SELECT t.id TaskId,t.name TaskName, t.description TaskDescription, t.priority TaskPriority,t.deadline TaskDeadline,t.status TaskStatus
FROM dbo.Task t
WHERE JSON_VALUE(t.responsible,'$.FirstName') LIKE @FirstName
AND JSON_VALUE(t.supervisor,'$.BirthDay')<@Birthday;

-- 8. Get Employee And All Tasks
SELECT JSON_VALUE(em.value,'$.Employee.Id') Id,JSON_VALUE(em.value,'$.Employee.Email') Email, COUNT (t.id) TaskCount
FROM dbo.Task t CROSS APPLY OPENJSON(t.employees) em
GROUP BY JSON_VALUE(em.value,'$.Employee.Id') ,JSON_VALUE(em.value,'$.Employee.Email');

-- 9. Get Employee and their tasks having and order
SELECT JSON_VALUE(em.value,'$.Employee.Id') Id,JSON_VALUE(em.value,'$.Employee.Email') Email, COUNT (t.id) TaskCount
FROM dbo.Task t CROSS APPLY OPENJSON(t.employees) em
GROUP BY JSON_VALUE(em.value,'$.Employee.Id') ,JSON_VALUE(em.value,'$.Employee.Email')
HAVING COUNT (t.id)>@NumOfEmployees
ORDER BY COUNT (t.id) DESC;