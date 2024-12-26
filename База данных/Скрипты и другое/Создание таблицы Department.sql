--Создание таблицы с нуля
SELECT [DepartmentId]
	  ,SUBSTRING([DepartmentId], 1, LEN([DepartmentId]) - CHARINDEX('.',REVERSE(DepartmentId))) AS ParentDepartmentId
	  ,[Title]
INTO [Department]
FROM [dbo].[Test]



