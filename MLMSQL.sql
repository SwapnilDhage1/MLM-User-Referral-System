CREATE DATABASE MLMDb;
GO

USE MLMDb;
GO

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    MobileNumber NVARCHAR(15) NOT NULL,
    SponsorId INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SponsorId) REFERENCES Users(Id)
);
GO


CREATE INDEX idx_sponsor_id ON Users(SponsorId);
GO


CREATE PROCEDURE RegisterUser
    @FullName NVARCHAR(100),
    @Email NVARCHAR(100),
    @PasswordHash NVARCHAR(255),
    @MobileNumber NVARCHAR(15),
    @SponsorId INT = NULL,
    @NewUserId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO Users (FullName, Email, PasswordHash, MobileNumber, SponsorId, CreatedAt)
        VALUES (@FullName, @Email, @PasswordHash, @MobileNumber, @SponsorId, GETDATE());

        SET @NewUserId = SCOPE_IDENTITY();

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW 50000, 'An error occurred during registration', 1;
    END CATCH
END;
GO


DECLARE @NewUserId INT;
EXEC RegisterUser 
    @FullName = 'Test User1',
    @Email = 'testusr@example.com',
    @PasswordHash = 'somehashedpassword',
    @MobileNumber = '1234567890',
    @SponsorId = NULL,
    @NewUserId = @NewUserId OUTPUT;

SELECT @NewUserId AS NewUserId;


EXEC GetReferralTree @UserId = 2;

alter PROCEDURE GetReferralTree
    @UserId INT
AS
BEGIN
   
    SELECT Id, FullName, Email
    FROM Users
    WHERE Id = @UserId;

   
    SELECT Id, FullName, Email
    FROM Users
    WHERE SponsorId = @UserId;
END
EXEC GetReferralTree 1;


SELECT Id, FullName, Email, SponsorId FROM Users ORDER BY Id DESC;

