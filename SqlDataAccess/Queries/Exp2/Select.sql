-- 0. Vrati sve timove i zaposlene po department *
SELECT *
FROM Department d 
LEFT JOIN Team t ON t.department_id=d.id
INNER JOIN Employee l ON t.leader_id=l.id
LEFT JOIN Employee e ON e.team_id=t.id;
-- 1. Vrati sve timove i zaposlene po department naziv
SELECT d.id DepartementId,d.name DepartmentName,d.location DepartmentLocation,
t.id TeamId, t.name TeamName, t.status TeamStatus, t.description TeamDescription,
l.id LeadId, l.first_name LeadFirstName, l.last_name LeadLastName, l.email LeadEmail, l.birth_day LeadBirthDay, l.title LeadTitle, l.phone LeadPhone,
e.id Id, e.first_name Firstname, e.last_name Lastname, e.email Email, e.birth_day BirthDay, e.title Title, e.phone Phone
FROM Department d 
LEFT JOIN Team t ON t.department_id=d.id
INNER JOIN Employee l ON t.leader_id=l.id
LEFT JOIN Employee e ON e.team_id=t.id;
-- 2. Vrati sve po timove po department id
SELECT t.id TeamId, t.name TeamName, t.status TeamStatus, t.description TeamDescription,
l.id LeadId, l.first_name LeadFirstName, l.last_name LeadLastName, l.email LeadEmail, l.birth_day LeadBirthDay, l.title LeadTitle, l.phone LeadPhone,
e.id Id, e.first_name Firstname, e.last_name Lastname, e.email Email, e.birth_day BirthDay, e.title Title, e.phone Phone
FROM Team t
INNER JOIN Employee l ON t.leader_id=l.id
LEFT JOIN Employee e ON e.team_id=t.id
WHERE t.department_id=@DepartmentId;
-- 3.  vrati zaposlene po id tima
SELECT e.id Id, e.first_name Firstname, e.last_name Lastname, e.email Email, e.birth_day BirthDay, e.title Title, e.phone Phone
FROM Employee e
WHERE e.team_id=@TeamId;
-- 4. vrati zaposlenog po Id
SELECT e.id Id, e.first_name Firstname, e.last_name Lastname, e.email Email, e.birth_day BirthDay, e.title Title, e.phone Phone
FROM Employee e
WHERE e.id=@EmployeeId;
-- 5. vrati sve departmenta sa timovima koji su u status Active i sort po nazivu
SELECT d.id DepartementId,d.name DepartmentName,d.location DepartmentLocation,
t.id TeamId, t.name TeamName, t.status TeamStatus, t.description TeamDescription
FROM Department d 
LEFT JOIN Team t ON t.department_id=d.id
WHERE d.location='Belgrade'
ORDER BY t.name ASC;
-- 6. vrati sve department sa timovima u kojima je sef mladji od 35 g i imaju Engineer
SELECT d.id DepartementId,d.name DepartmentName,d.location DepartmentLocation,
t.id TeamId, t.name TeamName, t.status TeamStatus, t.description TeamDescription, e.id
FROM Department d 
LEFT JOIN Team t ON t.department_id=d.id
LEFT JOIN Employee e ON e.team_id=t.id
WHERE t.leader_id IN (SELECT id FROM Employee WHERE DATEDIFF(YEAR,birth_day,GETDATE()) <35)
AND e.title LIKE '%Engineer%';

-- 7. vrati department i broj zaposlenih koji su izmedju 30 i 40 godina
SELECT d.id DepartmentId, d.name DepartmentName,COUNT(e.id) EmployeesCount
FROM Department d
LEFT JOIN Team t ON (d.id=t.department_id) LEFT JOIN Employee e ON (t.id=e.team_id)
AND DATEDIFF(YEAR, e.birth_day, GETDATE()) BETWEEN 30 AND 40
GROUP BY d.Id,d.name;
-- 8. vrati za svaki departemnt vrati timove koje broje zaposlene koji su Engineer i ima vise od 40 i sort
SELECT d.id DepartmentId, d.name DepartmentName,t.id TeamId, t.name TeamName, COUNT(e.id) EmployeesCount
FROM Department d
LEFT JOIN Team t ON (d.id=t.department_id) LEFT JOIN Employee e ON (t.id=e.team_id)
WHERE title LIKE '%Engineer%'
GROUP BY  d.id, d.name,t.id, t.name
HAVING COUNT(e.id)>2
ORDER BY COUNT(e.id) DESC;