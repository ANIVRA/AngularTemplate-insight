CREATE PROCEDURE [Security].[RemoveUserFromRole] 
@userId int, @role nvarchar(50)
as

delete ur
from [Security].[UserRoles] ur
join [Security].[Roles] r on r.[Id] = ur.[RoleId] 
where ur.UserId = @userId AND r.[Name] = @role