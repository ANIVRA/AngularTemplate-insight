CREATE PROCEDURE [Security].[FindUserByUserName] 
	@username nvarchar(128)
AS
SELECT * From [Security].[Users]
WHERE UserName = @username