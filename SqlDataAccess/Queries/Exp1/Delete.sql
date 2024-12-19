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

Dear John,

I hope this message finds you well.
I wanted to take a moment to reconnect and remind you of our conversation this summer in Rogljevo. It was a pleasure meeting you and discussing potential opportunities.
As you might remember, I am Miško's nephew, and I truly appreciated the time you took to speak with me.

I must apologize for the delay in reaching out as we had planned. My master’s obligations kept me quite busy, and I regret not contacting you sooner.

As we discussed, I am very interested in exploring any potential roles or opportunities you might have in mind.
I have attached my resume and would greatly appreciate any advice or guidance you could share regarding possible next steps.

Please do not hesitate to let me know if you need any additional information or documentation from my side.

Thank you once again for your time and understanding. 
I look forward to the possibility of continuing our conversation and hearing from you soon.

Best regards,
Mihajlo Pavlović