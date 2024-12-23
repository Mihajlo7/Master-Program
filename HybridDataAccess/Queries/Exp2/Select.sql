-- 0.
SELECT * FROM Department;
-- 1.
SELECT 
    d.id AS DepartementId,
    d.name AS DepartmentName,
    d.location AS DepartmentLocation,
    CAST(JSON_VALUE(t.value, '$.Id') AS BIGINT) AS TeamId,
    JSON_VALUE(t.value, '$.Name') AS TeamName,
    JSON_VALUE(t.value, '$.Status') AS TeamStatus,
    JSON_VALUE(t.value, '$.Description') AS TeamDescription,
    CAST(JSON_VALUE(t.value, '$.Lead.Id') AS BIGINT) AS LeadId,
    JSON_VALUE(t.value, '$.Lead.FirstName') AS LeadFirstName,
    JSON_VALUE(t.value, '$.Lead.LastName') AS LeadLastName,
    JSON_VALUE(t.value, '$.Lead.Email') AS LeadEmail,
    TRY_CAST(LEFT(JSON_VALUE(t.value, '$.Lead.BirthDay'), 19) AS DATETIME) AS LeadBirthDay, -- Bez vremenske zone
    JSON_VALUE(t.value, '$.Lead.Phone') AS LeadTitle,
    JSON_VALUE(t.value, '$.Lead.Phone') AS LeadPhone,
    CAST(JSON_VALUE(e.value, '$.Id') AS BIGINT) AS Id,
    JSON_VALUE(e.value, '$.FirstName') AS FirstName,
    JSON_VALUE(e.value, '$.LastName') AS LastName,
    JSON_VALUE(e.value, '$.Email') AS Email,
    TRY_CAST(LEFT(JSON_VALUE(e.value, '$.BirthDay'), 19) AS DATETIME) AS BirthDay, -- Bez vremenske zone
    JSON_VALUE(e.value, '$.Title') AS Title,
    JSON_VALUE(e.value, '$.Phone') AS Phone
FROM 
    Department d
CROSS APPLY 
    OPENJSON(teams) t
CROSS APPLY 
    OPENJSON(JSON_QUERY(value, '$.Employees')) e;
---------------------------------------------------------------
-- 2.
SELECT 
    TRY_CAST(JSON_VALUE(t.value, '$.Id') AS BIGINT) AS TeamId,
    JSON_VALUE(t.value, '$.Name') AS TeamName,
    JSON_VALUE(t.value, '$.Status') AS TeamStatus,
    JSON_VALUE(t.value, '$.Description') AS TeamDescription,
    TRY_CAST(JSON_VALUE(t.value, '$.Lead.Id') AS BIGINT) AS LeadId,
    JSON_VALUE(t.value, '$.Lead.FirstName') AS LeadFirstName,
    JSON_VALUE(t.value, '$.Lead.LastName') AS LeadLastName,
    JSON_VALUE(t.value, '$.Lead.Email') AS LeadEmail,
    TRY_CAST(LEFT(JSON_VALUE(t.value, '$.Lead.BirthDay'), 19) AS DATETIME) AS LeadBirthDay,
    JSON_VALUE(t.value, '$.Lead.Phone') AS LeadTitle,
    JSON_VALUE(t.value, '$.Lead.Phone') AS LeadPhone,
    TRY_CAST(JSON_VALUE(e.value, '$.Id') AS BIGINT) AS Id,
    JSON_VALUE(e.value, '$.FirstName') AS FirstName,
    JSON_VALUE(e.value, '$.LastName') AS LastName,
    JSON_VALUE(e.value, '$.Email') AS Email,
    TRY_CAST(LEFT(JSON_VALUE(e.value, '$.BirthDay'), 19) AS DATETIME) AS BirthDay,
    JSON_VALUE(e.value, '$.Title') AS Title,
    JSON_VALUE(e.value, '$.Phone') AS Phone
FROM 
    Department d
CROSS APPLY 
    OPENJSON(teams) t
CROSS APPLY 
    OPENJSON(JSON_QUERY(value, '$.Employees')) e
WHERE 
    d.id = @DepartmentId;

----------------------------------------------------------------
-- 3.
SELECT 
    TRY_CAST(JSON_VALUE(e.value, '$.Id') AS BIGINT) AS Id,
    JSON_VALUE(e.value, '$.FirstName') AS FirstName,
    JSON_VALUE(e.value, '$.LastName') AS LastName,
    JSON_VALUE(e.value, '$.Email') AS Email,
    TRY_CAST(LEFT(JSON_VALUE(e.value, '$.BirthDay'), 19) AS DATETIME) AS BirthDay,
    JSON_VALUE(e.value, '$.Title') AS Title,
    JSON_VALUE(e.value, '$.Phone') AS Phone
FROM 
    Department d
CROSS APPLY 
    OPENJSON(teams) t
CROSS APPLY 
    OPENJSON(JSON_QUERY(value, '$.Employees')) e
WHERE 
    TRY_CAST(JSON_VALUE(t.value, '$.Id') AS BIGINT) = @TeamId;

----------------------------------------------------------------
-- 4.
SELECT 
    TRY_CAST(JSON_VALUE(e.value, '$.Id') AS BIGINT) AS Id,
    JSON_VALUE(e.value, '$.FirstName') AS FirstName,
    JSON_VALUE(e.value, '$.LastName') AS LastName,
    JSON_VALUE(e.value, '$.Email') AS Email,
    TRY_CAST(LEFT(JSON_VALUE(e.value, '$.BirthDay'), 19) AS DATETIME) AS BirthDay,
    JSON_VALUE(e.value, '$.Title') AS Title,
    JSON_VALUE(e.value, '$.Phone') AS Phone
FROM 
    Department d
CROSS APPLY 
    OPENJSON(teams) t
CROSS APPLY 
    OPENJSON(JSON_QUERY(value, '$.Employees')) e
WHERE 
    TRY_CAST(JSON_VALUE(e.value, '$.Id') AS BIGINT) = @EmployeeId;

----------------------------------------------------------------
-- 5.
SELECT 
    d.id AS DepartementId,
    d.name AS DepartmentName,
    d.location AS DepartmentLocation,
    TRY_CAST(JSON_VALUE(t.value, '$.Id') AS BIGINT) AS TeamId,
    JSON_VALUE(t.value, '$.Name') AS TeamName,
    JSON_VALUE(t.value, '$.Status') AS TeamStatus,
    JSON_VALUE(t.value, '$.Description') AS TeamDescription
FROM 
    Department d
CROSS APPLY 
    OPENJSON(teams) t
WHERE 
    d.location = 'Belgrade'
ORDER BY 
    JSON_VALUE(t.value, '$.Name') ASC;

-----------------------------------------------------------------
-- 6. 
SELECT 
    DISTINCT d.id DepartementId,
    d.name DepartmentName,
    d.location DepartmentLocation,
    TRY_CAST(JSON_VALUE(t.value,'$.Id')AS BIGINT) TeamId,
    JSON_VALUE(t.value,'$.Name') TeamName,
    JSON_VALUE(t.value,'$.Status') TeamStatus,
    JSON_VALUE(t.value,'$.Description') TeamDescription
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
SELECT  
    d.id DepartmentId, 
    d.name DepartmentName,
    TRY_CAST(JSON_VALUE(t.value,'$.Id') AS BIGINT) TeamId,
    JSON_VALUE(t.value,'$.Name') TeamName,
    COUNT(JSON_VALUE(e.value,'$.Id')) EmployeesCount
FROM Department d CROSS APPLY OPENJSON(teams) t
CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
WHERE JSON_VALUE(e.value,'$.Title') LIKE '%Engineer%'
GROUP BY d.Id,d.name, JSON_VALUE(t.value,'$.Id') ,JSON_VALUE(t.value,'$.Name')
HAVING COUNT(JSON_VALUE(e.value,'$.Id'))>2
ORDER BY COUNT(JSON_VALUE(e.value,'$.Id')) DESC;
