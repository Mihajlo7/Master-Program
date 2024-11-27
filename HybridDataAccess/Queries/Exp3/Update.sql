UPDATE Employee SET phone = @Phone WHERE id = @EmployeeId;

UPDATE Employee SET method = JSON_MODIFY(manager,@Method,'$.Method') WHERE id = @ManagerId AND manager is not null;

UPDATE Developer SET isFullStack = JSON_MODIFY(softwareDeveloper,1,'$.IsFullStack') 
WHERE id = @SoftwareDeveloperId AND softwareDeveloper is not null;

UPDATE Employee 
SET method = JSON_MODIFY(manager,'Lean','$.Method')
WHERE JSON_VALUE(manager,'$.Department') IN ('IT','Logistics') AND
DATEDIFF(YEAR, e.birthday, GETDATE()) BETWEEN 40 AND 50;

UPDATE Developer
SET isFullStack = JSON_MODIFY(softwareDeveloper,1,'$.IsFullStack')
WHERE yearsOfExperience > 10
AND id IN(SELECT id FROM Employee WHERE title LIKE '%Engineer%');