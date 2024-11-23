-- 1. Update Task expired and pedning
UPDATE dbo.Task SET Status = 'Cancelled' WHERE Deadline < GETDATE() AND Status = 'Pending';
-- 2. Update deadline for priority and deadline
UPDATE dbo.Task
SET Deadline = DATEADD(DAY, 5, Deadline) 
WHERE Priority < 4 
  AND Deadline BETWEEN GETDATE() AND DATEADD(DAY, 3, GETDATE());
-- 3. Update Deadline by LastName
UPDATE dbo.Task
SET deadline= DATEADD(DAY,3,deadline)
WHERE JSON_VALUE(responsible,'$.LastName') LIKE 'M%';
-- 4. Update Deadline By Responsible Title and Supervisor birthday
UPDATE dbo.Task
SET deadline= DATEADD(DAY,2,deadline)
WHERE JSON_VALUE(responsible,'$.Title') LIKE '%Engineer%' AND JSON_VALUE(supervisor,'$.BirthDay')<'1980-01-01';