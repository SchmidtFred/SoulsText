USE [master]
GO

IF db_id('SoulsText') IS NULL
	CREATE DATABASE [SoulsText]
GO

USE [SoulsText]
GO

DROP TABLE IF EXISTS UserProfile;
DROP TABLE IF EXISTS [Message];
DROP TABLE IF EXISTS Vote;
GO

CREATE TABLE UserProfile (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[UserName] VARCHAR(25) NOT NULL,
);

CREATE TABLE [Message] (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	Content VARCHAR(500) NOT NULL,
	X FLOAT NOT NULL DEFAULT 0,
	Y FLOAT NOT NULL DEFAULT 0,
	Z FLOAT NOT NULL DEFAULT 0,
	UserProfileId INTEGER NOT NULL

	CONSTRAINT [FK_Message_UserProfile] FOREIGN KEY([UserProfileId]) REFERENCES [UserProfile] ([Id]) ON DELETE CASCADE
);

CREATE TABLE Vote (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	Upvote BIT NOT NULL DEFAULT 0,
	UserProfileId INTEGER,
	MessageId INTEGER NOT NULL

	CONSTRAINT [FK_Vote_UserProfile] FOREIGN KEY (UserProfileId) REFERENCES [UserProfile] ([Id]) ON DELETE NO ACTION,
	CONSTRAINT [Fk_Vote_Message] FOREIGN KEY (MessageId) REFERENCES [Message] ([Id]) ON DELETE CASCADE
);
GO

SET IDENTITY_INSERT [UserProfile] ON
INSERT INTO [UserProfile]
	([Id], [UserName])
VALUES
	(1, 'Seracrooce'),
	(2, 'BillyBob'),
	(3, 'FunkyMunky');
SET IDENTITY_INSERT [UserProfile] OFF

SET IDENTITY_INSERT [Message] ON
INSERT INTO [Message]
	([Id], [Content], [X], [Y], [Z], [UserProfileId])
VALUES
	(1, 'Hello, this is a message.', 0, 0, 0, 1),
	(2, 'They call me, BillyBob', 0, 0, 0, 2),
	(3, 'They call you, Not BillyBob', 0, 0, 0, 2);
SET IDENTITY_INSERT [Message] OFF

SET IDENTITY_INSERT [Vote] ON
INSERT INTO [Vote]
	([Id], [Upvote], [UserProfileId], [MessageId])
VALUES
	(1, 1, 3, 3),
	(2, 1, 2, 1),
	(3, 0, 3, 1);
SET IDENTITY_INSERT [Vote] OFF