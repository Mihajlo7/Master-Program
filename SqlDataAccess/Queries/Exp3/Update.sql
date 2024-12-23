UPDATE Employee SET phone = @Phone WHERE id = @EmployeeId;

UPDATE Manager SET method = @Method WHERE id = @ManagerId;

UPDATE SoftwareDeveloper SET isFullStack = 1 WHERE id = @SoftwareDeveloperId;

UPDATE Manager 
SET method = 'Lean'
WHERE department IN ('IT','Logistics')
AND id IN (SELECT e.id FROM Employee e WHERE DATEDIFF(YEAR, e.birthday, GETDATE()) BETWEEN 40 AND 50);

UPDATE SoftwareDeveloper
SET isFullStack = 1
WHERE id IN(SELECT id FROM Developer WHERE yearsOfExperience > 10)
AND id IN(SELECT id FROM Employee WHERE title LIKE '%Engineer%');

UPDATE Employee
SET title = 'Principle ' + title
WHERE id IN (SELECT id FROM Developer WHERE seniority='Senior' AND yearsOfExperience>20)
AND id IN (SELECT id FROM SoftwareDeveloper WHERE isFullStack=1);
