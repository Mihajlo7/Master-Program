UPDATE Employee SET phone = @Phone WHERE id = @EmployeeId;

UPDATE Employee SET manager = JSON_MODIFY(manager,'$.Method',@Method) WHERE id = @ManagerId AND manager is not null;

UPDATE Developer SET softwareDeveloper = JSON_MODIFY(softwareDeveloper,'$.IsFullStack',1) 
WHERE id = @SoftwareDeveloperId AND softwareDeveloper is not null;

UPDATE Employee 
SET manager = JSON_MODIFY(manager,'$.Method','Lean')
WHERE JSON_VALUE(manager,'$.Department') IN ('IT','Logistics') AND
DATEDIFF(YEAR, birthday, GETDATE()) BETWEEN 40 AND 50;

UPDATE Developer
SET softwareDeveloper = JSON_MODIFY(softwareDeveloper,'$.IsFullStack',1)
WHERE yearsOfExperience > 10
AND id IN(SELECT id FROM Employee WHERE title LIKE '%Engineer%');