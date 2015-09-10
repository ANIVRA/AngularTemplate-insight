CREATE PROCEDURE [Security].[GetRolesForUser] 
@userId int as
SELECT r.[Name]
FROM [Security].[UserRoles] AS ur
INNER JOIN [Security].[Roles] r ON r.[Id] = ur.[RoleId]
WHERE ur.[UserId] = @userId
