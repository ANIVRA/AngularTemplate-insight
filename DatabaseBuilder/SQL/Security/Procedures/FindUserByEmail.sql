CREATE PROCEDURE [Security].[FindUserByEmail] 
	@email nvarchar(128)
AS
SELECT * From [Security].[Users]
WHERE Email = @email