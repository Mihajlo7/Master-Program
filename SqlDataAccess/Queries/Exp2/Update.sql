-- 1. Update status by id
UPDATE Team SET Status=@Status WHERE id=@Id;

-- 2. Update phone by Id
UPDATE Employee SET phone=@Phone WHERE id=@Id;

-- 3. By Location
UPDATE Team 
SET description=@Description 
WHERE department_id IN (SELECT id FROM Department WHERE location='Prague');

-- 4. 
UPDATE Team
SET description = 'Very Young'
WHERE id IN (
    SELECT t.id
    FROM Team t
    JOIN Employee e ON e.team_id = t.id
    WHERE DATEDIFF(YEAR, e.birth_day, GETDATE()) < 20
);
-- 5.
UPDATE Team
SET description=description+'  SuperTeam'
WHERE department_id IN (SELECT id FROM Department WHERE location='London' OR name LIKE 'H%')
AND id IN(
    SELECT t.id
    FROM Team t
    JOIN Employee e ON e.team_id = t.id
    WHERE e.title LIKE '%Engineer%'
    GROUP BY t.id
    HAVING COUNT(e.id)>5
)
