
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 03/23/2015 22:14:12
-- Generated from EDMX file: C:\Users\henri_000\Documents\Visual Studio 2013\Projects\Macellum_Remake\Macellum_Remake\Models\DBConnection.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [macellum_new];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_TripUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Trips] DROP CONSTRAINT [FK_TripUser];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_RoleUser];
GO
IF OBJECT_ID(N'[dbo].[FK_FiskArter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Fisks] DROP CONSTRAINT [FK_FiskArter];
GO
IF OBJECT_ID(N'[dbo].[FK_FiskHavn]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Fisks] DROP CONSTRAINT [FK_FiskHavn];
GO
IF OBJECT_ID(N'[dbo].[FK_HavnFuleCost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FuleCosts] DROP CONSTRAINT [FK_HavnFuleCost];
GO
IF OBJECT_ID(N'[dbo].[FK_UserPassword]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_UserPassword];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Trips]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Trips];
GO
IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Passwords]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Passwords];
GO
IF OBJECT_ID(N'[dbo].[Roles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Roles];
GO
IF OBJECT_ID(N'[dbo].[Fisks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Fisks];
GO
IF OBJECT_ID(N'[dbo].[Arters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Arters];
GO
IF OBJECT_ID(N'[dbo].[Havns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Havns];
GO
IF OBJECT_ID(N'[dbo].[FuleCosts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FuleCosts];
GO
IF OBJECT_ID(N'[dbo].[ActiveSessionIds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ActiveSessionIds];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Trips'
CREATE TABLE [dbo].[Trips] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Open] bit  NOT NULL,
    [FishList] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [User_Id] int  NOT NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(max)  NOT NULL,
    [Role_Id] int  NOT NULL,
    [Password_Id] int  NOT NULL
);
GO

-- Creating table 'Passwords'
CREATE TABLE [dbo].[Passwords] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [pass] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Fisks'
CREATE TABLE [dbo].[Fisks] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AvgPrice] decimal(18,5)  NOT NULL,
    [MaxPrice] decimal(18,5)  NOT NULL,
    [Sort] int  NOT NULL,
    [Amount] decimal(18,5)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Arter_Id] int  NOT NULL,
    [Havn_Id] int  NOT NULL
);
GO

-- Creating table 'Arters'
CREATE TABLE [dbo].[Arters] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Havns'
CREATE TABLE [dbo].[Havns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FuleCosts'
CREATE TABLE [dbo].[FuleCosts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Price] decimal(18,5)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Havn_Id] int  NOT NULL
);
GO

-- Creating table 'ActiveSessionIds'
CREATE TABLE [dbo].[ActiveSessionIds] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SessionId] nvarchar(max)  NOT NULL,
    [UserId] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Trips'
ALTER TABLE [dbo].[Trips]
ADD CONSTRAINT [PK_Trips]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Passwords'
ALTER TABLE [dbo].[Passwords]
ADD CONSTRAINT [PK_Passwords]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Fisks'
ALTER TABLE [dbo].[Fisks]
ADD CONSTRAINT [PK_Fisks]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Arters'
ALTER TABLE [dbo].[Arters]
ADD CONSTRAINT [PK_Arters]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Havns'
ALTER TABLE [dbo].[Havns]
ADD CONSTRAINT [PK_Havns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FuleCosts'
ALTER TABLE [dbo].[FuleCosts]
ADD CONSTRAINT [PK_FuleCosts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ActiveSessionIds'
ALTER TABLE [dbo].[ActiveSessionIds]
ADD CONSTRAINT [PK_ActiveSessionIds]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'Trips'
ALTER TABLE [dbo].[Trips]
ADD CONSTRAINT [FK_TripUser]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TripUser'
CREATE INDEX [IX_FK_TripUser]
ON [dbo].[Trips]
    ([User_Id]);
GO

-- Creating foreign key on [Role_Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_RoleUser]
    FOREIGN KEY ([Role_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleUser'
CREATE INDEX [IX_FK_RoleUser]
ON [dbo].[Users]
    ([Role_Id]);
GO

-- Creating foreign key on [Arter_Id] in table 'Fisks'
ALTER TABLE [dbo].[Fisks]
ADD CONSTRAINT [FK_FiskArter]
    FOREIGN KEY ([Arter_Id])
    REFERENCES [dbo].[Arters]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FiskArter'
CREATE INDEX [IX_FK_FiskArter]
ON [dbo].[Fisks]
    ([Arter_Id]);
GO

-- Creating foreign key on [Havn_Id] in table 'Fisks'
ALTER TABLE [dbo].[Fisks]
ADD CONSTRAINT [FK_FiskHavn]
    FOREIGN KEY ([Havn_Id])
    REFERENCES [dbo].[Havns]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FiskHavn'
CREATE INDEX [IX_FK_FiskHavn]
ON [dbo].[Fisks]
    ([Havn_Id]);
GO

-- Creating foreign key on [Havn_Id] in table 'FuleCosts'
ALTER TABLE [dbo].[FuleCosts]
ADD CONSTRAINT [FK_HavnFuleCost]
    FOREIGN KEY ([Havn_Id])
    REFERENCES [dbo].[Havns]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HavnFuleCost'
CREATE INDEX [IX_FK_HavnFuleCost]
ON [dbo].[FuleCosts]
    ([Havn_Id]);
GO

-- Creating foreign key on [Password_Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserPassword]
    FOREIGN KEY ([Password_Id])
    REFERENCES [dbo].[Passwords]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPassword'
CREATE INDEX [IX_FK_UserPassword]
ON [dbo].[Users]
    ([Password_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------