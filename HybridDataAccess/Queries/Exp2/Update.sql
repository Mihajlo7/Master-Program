
---------------------------------------
-- 0.
WITH cte AS(
	SELECT [key],teams
	FROM Department CROSS APPLY OPENJSON(teams)
	WHERE JSON_VALUE(value,'$.Id')=2
)
UPDATE cte
SET teams= JSON_MODIFY(teams,'$['+cte.[key]+'].Status','Neki Moj');

---------------------------------------
-- 1.
WITH cte AS(
	SELECT t.[key] teamKey, e.[key] empKey, teams
	FROM Department CROSS APPLY OPENJSON(teams) t CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
	WHERE JSON_VALUE(e.value,'$.Id')=@Id
)
UPDATE cte
SET teams= JSON_MODIFY(teams,'$['+cte.teamKey+'].Employees['+cte.empKey+'].Phone',@Phone);

---------------------------------------
-- 2.
WITH cte AS(
	SELECT t.[key] teamKey, teams
	FROM Department CROSS APPLY OPENJSON(teams) t
	WHERE location='Prague'
)
UPDATE cte
SET teams= JSON_MODIFY(teams,'$['+teamKey+'].Description',@Description);

----------------------------------------
-- 3.
WITH cte AS(
	SELECT t.[key] teamKey, teams
	FROM Department CROSS APPLY OPENJSON(teams) t CROSS APPLY OPENJSON(JSON_QUERY(value,'$.Employees')) e
	WHERE  DATEDIFF(YEAR, JSON_VALUE(e.value,'$.BirthDay'), GETDATE())<20
	--GROUP BY t.[key] , teams
)
UPDATE cte
SET teams= JSON_MODIFY(teams,'$['+cte.teamKey+'].Description','Very Young');

----------------------------------------
-- 4.
UPDATE Department
SET teams = JSON_MODIFY(teams, '$[' + t.[key] + '].Description', 'Super Team')
FROM Department d
CROSS APPLY OPENJSON(d.teams) t
CROSS APPLY OPENJSON(JSON_QUERY(t.value, '$.Employees')) e
WHERE (d.location = 'London' OR d.name LIKE 'H%') 
  AND JSON_VALUE(e.value, '$.Title') LIKE '%Engineer%'
  AND t.[key] IN (
    SELECT t.[key]
    FROM Department
    CROSS APPLY OPENJSON(teams) t
    CROSS APPLY OPENJSON(JSON_QUERY(t.value, '$.Employees')) e
    WHERE (location = 'London' OR name LIKE 'H%') 
      AND JSON_VALUE(e.value, '$.Title') LIKE '%Engineer%'
    GROUP BY t.[key]
    HAVING COUNT(t.[key]) > 5
  );
