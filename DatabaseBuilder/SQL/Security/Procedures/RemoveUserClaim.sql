CREATE PROCEDURE [Security].[RemoveUserClaim] 
@userId int, @claimType nvarchar(max)
as

DELETE [Security].[UserClaims]
WHERE UserId = @userId AND ClaimType = @claimType