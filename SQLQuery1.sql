CREATE DATABASE BlogDb;
 GO
 USE BlogDb;
 GO

-----------------------------------------------------
-- 1) Role
-----------------------------------------------------
CREATE TABLE [Role] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL
);

-----------------------------------------------------
-- 2) User
-----------------------------------------------------
CREATE TABLE [User] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserName] NVARCHAR(100) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT (1),
    [Name] NVARCHAR(100) NOT NULL,
    [Surname] NVARCHAR(100) NOT NULL,
    [RegistrationDate] DATETIME NOT NULL,
    [RoleId] INT NOT NULL,
    CONSTRAINT FK_User_Role
        FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id])
);

-----------------------------------------------------
-- 3) Skill
-----------------------------------------------------
CREATE TABLE [Skill] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL
);

-----------------------------------------------------
-- 4) Tag
-----------------------------------------------------
CREATE TABLE [Tag] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL
);

-----------------------------------------------------
-- 5) Blog
-----------------------------------------------------
CREATE TABLE [Blog] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(200) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [Rating] DECIMAL(5,2) NULL,         -- Adjust precision/scale as needed
    [PublishDate] DATETIME NOT NULL,
    [UserId] INT NOT NULL,
    CONSTRAINT FK_Blog_User
        FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
);

-----------------------------------------------------
-- 6) BlogTag
-----------------------------------------------------
CREATE TABLE [BlogTag] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [BlogId] INT NOT NULL,
    [TagId] INT NOT NULL,
    CONSTRAINT FK_BlogTag_Blog
        FOREIGN KEY ([BlogId]) REFERENCES [Blog]([Id]),
    CONSTRAINT FK_BlogTag_Tag
        FOREIGN KEY ([TagId]) REFERENCES [Tag]([Id])
);

-----------------------------------------------------
-- 7) UserSkill
-----------------------------------------------------
CREATE TABLE [UserSkill] (
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,
    [SkillId] INT NOT NULL,
    CONSTRAINT FK_UserSkill_User
        FOREIGN KEY ([UserId]) REFERENCES [User]([Id]),
    CONSTRAINT FK_UserSkill_Skill
        FOREIGN KEY ([SkillId]) REFERENCES [Skill]([Id])
);