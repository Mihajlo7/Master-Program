-- 1. Proecure for update Employee Tasks
CREATE  PROC UpdateTasksFromOneEmployeeToAnother
	@FromEmployee BIGINT,
	@ToEmployee BIGINT
AS
BEGIN
	DECLARE @TaskId BIGINT;
	
	
	DECLARE id_cursor CURSOR FOR 
	SELECT id FROM Task
	WHERE id IN (SELECT id FROM Task t1 CROSS APPLY OPENJSON(t1.employees) t1e WHERE JSON_VALUE(t1e.value,'$.Employee.Id')=@FromEmployee)

	OPEN id_cursor
	FETCH NEXT FROM id_cursor INTO @TaskId;

	WHILE @@FETCH_STATUS=0
		BEGIN
		-- Part 1 --
			UPDATE Task
			SET employees= JSON_MODIFY(employees,'append $',(SELECT TOP 1 JSON_QUERY(t2e.value) FROM Task t2 CROSS APPLY OPENJSON(t2.employees) t2e WHERE JSON_VALUE(t2e.value,'$.Employee.Id')=@ToEmployee))
			WHERE id =@TaskId;
		-- Part 2 --
			DECLARE @JSONOutput AS NVARCHAR(MAX);
			DECLARE @JSONInput AS NVARCHAR(MAX);
			SELECT @JSONInput= employees FROM Task WHERE id=@TaskId;

			SELECT @JSONOutput = JSON_QUERY('[]')
			
			
			SELECT @JSONOutput=JSON_MODIFY(@JSONOutput,'append $',JSON_QUERY(@JSONInput, '$[' + [key] + ']'))
			FROM OPENJSON(@JsonInput)
			WHERE JSON_VALUE([value],'$.Employee.Id')<>@FromEmployee

			UPDATE Task
			SET employees=@JSONOutput
			WHERE id=@TaskId
			
			FETCH NEXT FROM id_cursor INTO @TaskId;
		END;
END;
?
-- 2 Update phone by Id
CREATE PROC UpdateEmployeePhoneById
	@Id BIGINT,
	@Phone NVARCHAR(50)
AS
BEGIN
	-- Update responsible
	UPDATE Task
	SET responsible= JSON_MODIFY(responsible,'$.Phone',@Phone)
	WHERE JSON_VALUE(responsible,'$.Id')=@Id;

	-- Update supervisor
	UPDATE Task
	SET supervisor= JSON_MODIFY(supervisor,'$.Phone',@Phone)
	WHERE JSON_VALUE(supervisor,'$.Id')=@Id;

	--Update employees
	WITH cte AS(
		SELECT [key], employees
		FROM Task CROSS APPLY OPENJSON(employees)
		WHERE JSON_VALUE([value],'$.Employee.Id')=@Id
	)
	UPDATE cte
	SET employees= JSON_MODIFY(employees,'$['+[key]+'].Employee.Phone',@Phone)
END;
?
-- 3. update employee by email
CREATE PROC UpdateEmployeePhoneByEmail
	@Email NVARCHAR(50),
	@Phone NVARCHAR(50)
AS
BEGIN
	-- Update responsible
	UPDATE Task
	SET responsible= JSON_MODIFY(responsible,'$.Phone',@Phone)
	WHERE JSON_VALUE(responsible,'$.Email')=@Email;

	-- Update supervisor
	UPDATE Task
	SET supervisor= JSON_MODIFY(supervisor,'$.Phone',@Phone)
	WHERE JSON_VALUE(supervisor,'$.Email')=@Email;

	--Update employees
	WITH cte AS(
		SELECT [key], employees
		FROM Task CROSS APPLY OPENJSON(employees)
		WHERE JSON_VALUE([value],'$.Employee.Id')=@Email
	)
	UPDATE cte
	SET employees= JSON_MODIFY(employees,'$['+[key]+'].Employee.Phone',@Phone)
END;
