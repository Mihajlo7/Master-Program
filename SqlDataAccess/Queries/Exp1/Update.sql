-- 1. Update Task expired and pedning
UPDATE dbo.Task SET Status = 'Cancelled' WHERE Deadline < GETDATE() AND Status = 'Pending';
-- 2. Update phone by Id,
UPDATE dbo.Employee SET phone=@Phone WHERE id=@Id;
-- 3. Update phone by Email,
UPDATE dbo.Employee SET phone=@Phone WHERE email=@Email;
-- 4. Update deadline for priority and deadline
UPDATE dbo.Task
SET Deadline = DATEADD(DAY, 5, Deadline) 
WHERE Priority < 4 
  AND Deadline BETWEEN GETDATE() AND DATEADD(DAY, 3, GETDATE());
-- 5. Update deadline by responisble name
UPDATE dbo.Task
SET deadline= DATEADD(DAY,3,deadline)
WHERE responsible IN(SELECT id FROM Employee WHERE lastname LIKE 'M%');
-- 6. Update deadline  by responsible title and supervisor birthday
UPDATE dbo.Task
SET deadline= DATEADD(DAY,2,deadline)
WHERE responsible IN(SELECT id FROM Employee WHERE title LIKE '%Engineer%')
AND supervisor IN (SELECT id FROM Employee WHERE birthday<'1980-01-01');
-- 7. Update tasks from one to other
UPDATE dbo.EmployeeTask
SET employeeId=@NewId
WHERE employeeId=@Id;