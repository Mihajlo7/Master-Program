﻿INSERT INTO Task(id,name,description,priority,deadline,status,responsible,supervisor,employees)
VALUES(@TaskId,@TaskName,@TaskDescription,@TaskPriority,@TaskDeadline,@TaskStatus,@Responsible,@Supervisor,@Employees);

WITH cte AS(
	SELECT id,[key],teams,value 
	FROM Department CROSS APPLY OPENJSON(teams)
	WHERE JSON_VALUE(value,'$.Id')=@TeamId
)
UPDATE cte
SET teams= JSON_MODIFY(teams,'$['+cte.[key]+'].Employees',JSON_QUERY(@Employees));
