CREATE PROCEDURE [Security].[GetUserClaims] 
@userId int
as
select * from security.userclaims where userid = @userId