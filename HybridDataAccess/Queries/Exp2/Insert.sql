INSERT INTO Department (id,name,location,teams) VALUES (@Id,@Name,@Location,@TeamsWithEmployees);

WITH cte AS(
	SELECT id,[key],teams,value 
	FROM Department CROSS APPLY OPENJSON(teams)
	WHERE JSON_VALUE(value,'$.Id')=@TeamId
)
UPDATE cte
SET teams= JSON_MODIFY(teams,'$['+cte.[key]+'].Employees',JSON_QUERY(@Employees));
