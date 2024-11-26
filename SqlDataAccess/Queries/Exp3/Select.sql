SELECT * FROM Employee;

SELECT e.id, e.firstname, e.lastname, e.email, e.birthday, e.title, e.phone, m.department, m.realisedProject, m.method 
FROM Employee e INNER JOIN Manager m ON e.id = m.id;

SELECT e.id, e.firstname, e.lastname, e.email, e.birthday, e.title, e.phone, d.seniority, d.yearsOfExperience, d.isRemote, sd.programmingLanguage, sd.ide, sd.isFullStack 
FROM Employee e INNER JOIN Developer d ON e.id = d.id INNER JOIN SoftwareDeveloper sd ON d.id = sd.id;

SELECT * FROM Employee WHERE id = @EmployeeId;

SELECT e.id, e.firstname, e.lastname, e.email, e.birthday, e.title, e.phone, m.department, m.realisedProject, m.method
FROM Employee e INNER JOIN Manager m ON e.id = m.id 
WHERE DATEDIFF(YEAR, e.birthday, GETDATE()) < 30 AND m.method = 'Agile' 
ORDER BY e.birthday ASC;

SELECT e.id, e.firstname, e.lastname, d.seniority, d.yearsOfExperience, sd.ide 
FROM Employee e INNER JOIN Developer d ON e.id = d.id 
INNER JOIN SoftwareDeveloper sd ON d.id = sd.id 
WHERE d.isRemote = 1 AND sd.ide = 'Visual Studio';

SELECT e.id, e.firstname, e.lastname, d.seniority, d.yearsOfExperience, sd.programmingLanguage 
FROM Employee e INNER JOIN Developer d ON e.id = d.id 
INNER JOIN SoftwareDeveloper sd ON d.id = sd.id 
WHERE DATEDIFF(YEAR, e.birthday, GETDATE()) > 25 AND d.seniority = 'Medior' AND sd.programmingLanguage IN ('Java','C#') 
ORDER BY d.yearsOfExperience DESC;

SELECT m.method, COUNT(*) AS ManagerCount FROM Manager m GROUP BY m.method;

SELECT sd.programmingLanguage, COUNT(*) AS DeveloperCount, AVG(d.yearsOfExperience) AS AvgExperience 
FROM Developer d INNER JOIN SoftwareDeveloper sd ON d.id = sd.id 
GROUP BY sd.programmingLanguage 
HAVING COUNT(*) > 10
ORDER BY AVG(d.yearsOfExperience) DESC;

