--Заполнение существующей таблицы

INSERT INTO Department (DepartmentId, ParentDepartmentId, Name)
SELECT [DepartmentId]
      ,SUBSTRING([DepartmentId], 1, LEN([DepartmentId]) - CHARINDEX('.',REVERSE(DepartmentId)))
      ,[Title]
FROM [dbo].[Test]