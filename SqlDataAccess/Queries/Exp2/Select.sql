--Vrati sve timove i zaposlene po department *
SELECT *
FROM Department d 
LEFT JOIN Team t ON t.id=d.team_id
INNER JOIN Employee l ON t.lead_id=e.id
LEFT JOIN Employee e ON e.team_id=t.id;
--Vrati sve timove i zaposlene po department naziv
SELECT d.id DepartementId,d.name DepartmentName,d.location DepartmentLocation,
t.id TeamId, t.name TeamName, t.status TeamStatus, t.description TeamDescription,
l.id LeadId, l.firstname LeadFirstName, l.lastname LeadLastName, l.email LeadEmail, l.birthday LeadBirthDay, l.title LeadTitle, l.phone LeadPhone,
e.id Id, e.firstname Firstname, e.lastname Lastname, e.email Email, e.birthday BirthDay, e.title Title, e.phone Phone
FROM Department d 
LEFT JOIN Team t ON t.id=d.team_id
INNER JOIN Employee l ON t.lead_id=e.id
LEFT JOIN Employee e ON e.team_id=t.id;
-- Vrati sve po timove po department id
SELECT t.id TeamId, t.name TeamName, t.status TeamStatus, t.description TeamDescription,
l.id LeadId, l.firstname LeadFirstName, l.lastname LeadLastName, l.email LeadEmail, l.birthday LeadBirthDay, l.title LeadTitle, l.phone LeadPhone,
e.id Id, e.firstname Firstname, e.lastname Lastname, e.email Email, e.birthday BirthDay, e.title Title, e.phone Phone
FROM Team t
INNER JOIN Employee l ON t.lead_id=e.id
LEFT JOIN Employee e ON e.team_id=t.id
WHERE t.department_id=@DepartmentId;
-- vrati zaposlene po id tima
SELECT e.id Id, e.firstname Firstname, e.lastname Lastname, e.email Email, e.birthday BirthDay, e.title Title, e.phone Phone
FROM Employee e
WHERE e.team_id=@TeamId;
--vrati zaposlenog po Id
SELECT e.id Id, e.firstname Firstname, e.lastname Lastname, e.email Email, e.birthday BirthDay, e.title Title, e.phone Phone
FROM Employee e
WHERE e.id=@EmployeeId;
--vrati sve departmenta sa timovima koji su u status Active i sort po nazivu
--vrati sve department sa timovima u kojima je sef mladji od 35 g i imaju Engineer
--vrati department i broj zaposlenih koji su izmedju 30 i 40 godina
--vrati za svaki departemnt vrati timove koje broje zaposlene koji su Engineer i ima vise od 40 i sort