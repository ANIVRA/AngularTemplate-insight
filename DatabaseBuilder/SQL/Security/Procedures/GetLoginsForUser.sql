CREATE PROCEDURE [Security].[GetLoginsForUser] 
@userId int
as
select * from security.userlogins where userid = @userId