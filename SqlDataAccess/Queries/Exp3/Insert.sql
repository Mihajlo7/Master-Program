INSERT INTO Employee (id, firstname, lastname, email, birthday, title, phone)
VALUES (@Id, @FirstName, @LastName, @Email, @Birthday, @Title, @Phone);

INSERT INTO developer (id, seniority, yearsofexperience, isremote)
VALUES (@Id, @Seniority, @YearsOfExperience, @IsRemote);

INSERT INTO intern (id, university, studyyear, city)
VALUES (@Id, @University, @StudyYear, @City);

INSERT INTO manager (id, department, realisedproject, method)
VALUES (@Id, @Department, @RealisedProject, @Method);

INSERT INTO softwaredeveloper (id, programminglanguage, ide, isfullstack)
VALUES (@Id, @ProgrammingLanguage, @IDE, @IsFullStack);

INSERT INTO databasedeveloper (id, provider, isadmin, knownosql)
VALUES (@Id, @Provider, @IsAdmin, @KnowNoSql);
