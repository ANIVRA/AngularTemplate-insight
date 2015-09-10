CREATE PROCEDURE [Security].[GetUserLoginsByUserId] 
@userId int

as
select * from [Security].[UserLogins]
Where UserId = @userId