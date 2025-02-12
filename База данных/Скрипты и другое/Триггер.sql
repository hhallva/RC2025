CREATE TRIGGER trAddParentDepartment
   ON  Departments
   AFTER INSERT, UPDATE
AS 
BEGIN
	UPDATE Departments
	SET ParentDepartmentId=IIF(CHARINDEX('.', DepartmentId) = 0, 
		NULL, 
		LEFT(DepartmentId, LEN(DepartmentId)-CHARINDEX('.', REVERSE(DepartmentId))))
END
