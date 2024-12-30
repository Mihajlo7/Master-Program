SELECT * FROM Employee;

SELECT  e.id, e.firstname, e.lastname, e.email, e.birthday, e.title, e.phone, m.department, m.realisedProject, m.method
FROM Employee e CROSS APPLY OPENJSON(manager) WITH(
	Department NVARCHAR(100) '$.Department',
	RealisedProject INT '$.RealisedProject',
	Method NVARCHAR(20) '$.Method'
)AS m
WHERE manager is not null;

SELECT e.id, e.firstname, e.lastname, e.email, e.birthday, e.title, e.phone, d.seniority, d.yearsOfExperience, d.isRemote, sd.programmingLanguage, sd.ide, sd.isFullStack 
FROM Employee e 
INNER JOIN Developer d ON e.id = d.id 
CROSS APPLY OPENJSON(softwareDeveloper) WITH (
	Programminglanguage NVARCHAR(30) '$.ProgrammingLanguage',
	IDE NVARCHAR(30) '$.IDE',
	IsFullStack BIT '$.IsFullStack'
) AS sd;

SELECT * FROM Employee WHERE id = @EmployeeId;

SELECT  e.id, e.firstname, e.lastname, e.email, e.birthday, e.title, e.phone, m.department, m.realisedProject, m.method
FROM Employee e CROSS APPLY OPENJSON(manager) WITH(
	Department NVARCHAR(100) '$.Department',
	RealisedProject INT '$.RealisedProject',
	Method NVARCHAR(20) '$.Method'
)AS m
WHERE DATEDIFF(YEAR, e.birthday, GETDATE()) < 30 AND m.method = 'Agile' 
ORDER BY e.birthday ASC;

SELECT e.id, e.firstname, e.lastname, e.email, e.birthday, e.title, e.phone, d.seniority, d.yearsOfExperience, d.isRemote, sd.programmingLanguage, sd.ide, sd.isFullStack 
FROM Employee e 
INNER JOIN Developer d ON e.id = d.id 
CROSS APPLY OPENJSON(softwareDeveloper) WITH (
	Programminglanguage NVARCHAR(30) '$.ProgrammingLanguage',
	IDE NVARCHAR(30) '$.IDE',
	IsFullStack BIT '$.IsFullStack'
) AS sd
WHERE d.isRemote = 1 AND sd.ide = 'Visual Studio';

SELECT e.id, e.firstname, e.lastname, e.email, e.birthday, e.title, e.phone, d.seniority, d.yearsOfExperience, d.isRemote, sd.programmingLanguage, sd.ide, sd.isFullStack 
FROM Employee e 
INNER JOIN Developer d ON e.id = d.id 
CROSS APPLY OPENJSON(softwareDeveloper) WITH (
	Programminglanguage NVARCHAR(30) '$.ProgrammingLanguage',
	IDE NVARCHAR(30) '$.IDE',
	IsFullStack BIT '$.IsFullStack'
) AS sd
WHERE DATEDIFF(YEAR, e.birthday, GETDATE()) > 25 AND d.seniority = 'Medior' AND sd.programmingLanguage IN ('Java','C#') 
ORDER BY d.yearsOfExperience DESC;

SELECT m.Method, COUNT(*)
FROM Employee CROSS APPLY OPENJSON(manager) WITH(
	Method NVARCHAR(30) '$.Method'
) AS m
GROUP BY m.Method;

SELECT JSON_VALUE(d.softwareDeveloper,'$.ProgrammingLanguage') AS ProgrammingLanguage, 
COUNT(*) AS DeveloperCount, 
AVG(d.yearsOfExperience) AS AvgExperience 
FROM Developer d
GROUP BY JSON_VALUE(d.softwareDeveloper,'$.ProgrammingLanguage') 
HAVING COUNT(*) > 10
ORDER BY AVG(d.yearsOfExperience) DESC; 
