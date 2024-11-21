-- 1. Delete all Tasks
DELETE FROM dbo.Task;
-- 2. Delete By Id
DELETE FROM dbo.Task WHERE id=@Id
-- 3. Delete By Status
DELETE FROM dbo.Task WHERE status=@Status;
-- 4. Delete by Responsible id
DELETE FROM dbo.Task WHERE responsible= (SELECT id FROM dbo.Employee WHERE id=@Id);
-- 5. Delete By Supervisor FirstName
DELETE FROM dbo.Task WHERE responsible= (SELECT id FROM dbo.Employee WHERE firstName LIKE 'J%');
