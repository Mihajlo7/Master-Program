-- 0.
SELECT * FROM Department;
-- 1.
SELECT d.id DepartementId,d.name DepartmentName,d.location DepartmentLocation,
JSON_VALUE(t.value,'$.Id') TeamId,JSON_VALUE(t.value,'$.Name') TeamName,JSON_VALUE(t.value,'$.Status') TeamStatus,JSON_VALUE(t.value,'$.Description') TeamDescription,
JSON_VALUE(t.value,'$.Lead.Id') LeadId,JSON_VALUE(t.value,'$.Lead.FirstName') LeadFirstName,JSON_VALUE(t.value,'$.Lead.LastName') LeadLastName,
JSON_VALUE(t.value,'$.Lead.Email') LeadEmail,JSON_VALUE(t.value,'$.Lead.BirthDay') LeadBirthDay,JSON_VALUE(t.value,'$.Lead.Phone') LeadTitle,JSON_VALUE(t.value,'$.Lead.Phone') LeadPhone,
JSON_VALUE(e.value,'$.Id') Id,JSON_VALUE(e.value,'$.FirstName') FirstName,JSON_VALUE(e.value,'$.LastName') LastName,JSON_VALUE(e.value,'$.Email') Email, 
JSON_VALUE(e.value,'$.BirthDay') BirthDay,JSON_VALUE(e.value,'$.Title') Title,JSON_VALUE(e.value,'$.Phone') Phone
FROM Department d CROSS APPLY OPENJSON(teams) t
CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e;
---------------------------------------------------------------
-- 2.
SELECT JSON_VALUE(t.value,'$.Id') TeamId,JSON_VALUE(t.value,'$.Name') TeamName,JSON_VALUE(t.value,'$.Status') TeamStatus,JSON_VALUE(t.value,'$.Description') TeamDescription,
JSON_VALUE(t.value,'$.Lead.Id') LeadId,JSON_VALUE(t.value,'$.Lead.FirstName') LeadFirstName,JSON_VALUE(t.value,'$.Lead.LastName') LeadLastName,
JSON_VALUE(t.value,'$.Lead.Email') LeadEmail,JSON_VALUE(t.value,'$.Lead.BirthDay') LeadBirthDay,JSON_VALUE(t.value,'$.Lead.Phone') LeadTitle,JSON_VALUE(t.value,'$.Lead.Phone') LeadPhone,
JSON_VALUE(e.value,'$.Id') Id,JSON_VALUE(e.value,'$.FirstName') FirstName,JSON_VALUE(e.value,'$.LastName') LastName,JSON_VALUE(e.value,'$.Email') Email, 
JSON_VALUE(e.value,'$.BirthDay') BirthDay,JSON_VALUE(e.value,'$.Title') Title,JSON_VALUE(e.value,'$.Phone') Phone
FROM Department d CROSS APPLY OPENJSON(teams) t
CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
WHERE id=@DepartmentId;

----------------------------------------------------------------
-- 3.
SELECT JSON_VALUE(e.value,'$.Id') Id,JSON_VALUE(e.value,'$.FirstName') FirstName,JSON_VALUE(e.value,'$.LastName') LastName,JSON_VALUE(e.value,'$.Email') Email, 
JSON_VALUE(e.value,'$.BirthDay') BirthDay,JSON_VALUE(e.value,'$.Title') Title,JSON_VALUE(e.value,'$.Phone') Phone
FROM Department d CROSS APPLY OPENJSON(teams) t
CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
WHERE JSON_VALUE(t.value,'$.Id')=@TeamId;

----------------------------------------------------------------
-- 4.
SELECT JSON_VALUE(e.value,'$.Id') Id,JSON_VALUE(e.value,'$.FirstName') FirstName,JSON_VALUE(e.value,'$.LastName') LastName,JSON_VALUE(e.value,'$.Email') Email, 
JSON_VALUE(e.value,'$.BirthDay') BirthDay,JSON_VALUE(e.value,'$.Title') Title,JSON_VALUE(e.value,'$.Phone') Phone
FROM Department d CROSS APPLY OPENJSON(teams) t
CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
WHERE JSON_VALUE(e.value,'$.Id')=@EmployeeId;

----------------------------------------------------------------
-- 5.
SELECT d.id DepartementId,d.name DepartmentName,d.location DepartmentLocation,
JSON_VALUE(t.value,'$.Id') TeamId,JSON_VALUE(t.value,'$.Name') TeamName,JSON_VALUE(t.value,'$.Status') TeamStatus,JSON_VALUE(t.value,'$.Description') TeamDescription
FROM Department d CROSS APPLY OPENJSON(teams) t
--CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
WHERE d.location='Belgrade'
ORDER BY JSON_VALUE(t.value,'$.Name') ASC;

-----------------------------------------------------------------
-- 6. 
SELECT DISTINCT d.id DepartementId,d.name DepartmentName,d.location DepartmentLocation,
JSON_VALUE(t.value,'$.Id') TeamId,JSON_VALUE(t.value,'$.Name') TeamName,JSON_VALUE(t.value,'$.Status') TeamStatus,JSON_VALUE(t.value,'$.Description') TeamDescription
FROM Department d CROSS APPLY OPENJSON(teams) t
CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
WHERE DATEDIFF(YEAR,JSON_VALUE(t.value,'$.Lead.BirthDay'),GETDATE()) <35
AND JSON_VALUE(e.value,'$.Title') LIKE '%Engineer%';

------------------------------------------------------------------
-- 7.
SELECT  d.id DepartmentId, d.name DepartmentName,COUNT(JSON_VALUE(e.value,'$.Id')) EmployeesCount
FROM Department d CROSS APPLY OPENJSON(teams) t
CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
WHERE DATEDIFF(YEAR, JSON_VALUE(e.value,'$.BirthDay'), GETDATE()) BETWEEN 30 AND 40
GROUP BY d.Id,d.name;

-------------------------------------------------------------------
-- 8.
SELECT  d.id DepartmentId, d.name DepartmentName,
JSON_VALUE(t.value,'$.Id') TeamId,JSON_VALUE(t.value,'$.Name') TeamName,COUNT(JSON_VALUE(e.value,'$.Id')) EmployeesCount
FROM Department d CROSS APPLY OPENJSON(teams) t
CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
WHERE JSON_VALUE(e.value,'$.Title') LIKE '%Engineer%'
GROUP BY d.Id,d.name, JSON_VALUE(t.value,'$.Id') ,JSON_VALUE(t.value,'$.Name')
HAVING COUNT(JSON_VALUE(e.value,'$.Id'))>2
ORDER BY COUNT(JSON_VALUE(e.value,'$.Id')) DESC;
