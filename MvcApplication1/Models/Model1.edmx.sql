
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 10/19/2016 11:12:33
-- Generated from EDMX file: C:\Users\Светлана\Desktop\Ver2\11\ЛР12_ЕС\MvcApplication1\MvcApplication1\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ExpertSystemsDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_PropertyVal]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ValSet] DROP CONSTRAINT [FK_PropertyVal];
GO
IF OBJECT_ID(N'[dbo].[FK_ValNote]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NoteSet] DROP CONSTRAINT [FK_ValNote];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemNote]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[NoteSet] DROP CONSTRAINT [FK_ItemNote];
GO
IF OBJECT_ID(N'[dbo].[FK_QuestionAnswer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AnswerSet] DROP CONSTRAINT [FK_QuestionAnswer];
GO
IF OBJECT_ID(N'[dbo].[FK_AnswerQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuestionSet] DROP CONSTRAINT [FK_AnswerQuestion];
GO
IF OBJECT_ID(N'[dbo].[FK_PropertyQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QuestionSet] DROP CONSTRAINT [FK_PropertyQuestion];
GO
IF OBJECT_ID(N'[dbo].[FK_AnswerSelector]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SelectorSet] DROP CONSTRAINT [FK_AnswerSelector];
GO
IF OBJECT_ID(N'[dbo].[FK_ValSelector]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SelectorSet] DROP CONSTRAINT [FK_ValSelector];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpertSystemProperty]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PropertySet] DROP CONSTRAINT [FK_ExpertSystemProperty];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpertSystemItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ItemSet] DROP CONSTRAINT [FK_ExpertSystemItem];
GO
IF OBJECT_ID(N'[dbo].[FK_UserExpertSystem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExpertSystemSet] DROP CONSTRAINT [FK_UserExpertSystem];
GO
IF OBJECT_ID(N'[dbo].[FK_SurveyLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LogSet] DROP CONSTRAINT [FK_SurveyLog];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpertSystemSurvey]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SurveySet] DROP CONSTRAINT [FK_ExpertSystemSurvey];
GO
IF OBJECT_ID(N'[dbo].[FK_LogLog]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LogSet] DROP CONSTRAINT [FK_LogLog];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemSurveyItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SurveyItemSet] DROP CONSTRAINT [FK_ItemSurveyItem];
GO
IF OBJECT_ID(N'[dbo].[FK_SurveySurveyItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SurveyItemSet] DROP CONSTRAINT [FK_SurveySurveyItem];
GO
IF OBJECT_ID(N'[dbo].[FK_LogTempRemovedItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TempLogItemSet] DROP CONSTRAINT [FK_LogTempRemovedItem];
GO
IF OBJECT_ID(N'[dbo].[FK_ItemTempRemovedItem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TempLogItemSet] DROP CONSTRAINT [FK_ItemTempRemovedItem];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[ItemSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ItemSet];
GO
IF OBJECT_ID(N'[dbo].[PropertySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PropertySet];
GO
IF OBJECT_ID(N'[dbo].[ValSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ValSet];
GO
IF OBJECT_ID(N'[dbo].[QuestionSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QuestionSet];
GO
IF OBJECT_ID(N'[dbo].[AnswerSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AnswerSet];
GO
IF OBJECT_ID(N'[dbo].[NoteSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NoteSet];
GO
IF OBJECT_ID(N'[dbo].[SelectorSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SelectorSet];
GO
IF OBJECT_ID(N'[dbo].[ExpertSystemSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpertSystemSet];
GO
IF OBJECT_ID(N'[dbo].[UserSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserSet];
GO
IF OBJECT_ID(N'[dbo].[LogSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogSet];
GO
IF OBJECT_ID(N'[dbo].[SurveySet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SurveySet];
GO
IF OBJECT_ID(N'[dbo].[SurveyItemSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SurveyItemSet];
GO
IF OBJECT_ID(N'[dbo].[TempLogItemSet]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TempLogItemSet];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'ItemSet'
CREATE TABLE [dbo].[ItemSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ExpertSystemId] int  NULL
);
GO

-- Creating table 'PropertySet'
CREATE TABLE [dbo].[PropertySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [RootQdeep] int  NOT NULL,
    [NumberInList] int  NOT NULL,
    [ExpertSystemId] int  NULL
);
GO

-- Creating table 'ValSet'
CREATE TABLE [dbo].[ValSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PropertyId] int  NULL,
    [Mean] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'QuestionSet'
CREATE TABLE [dbo].[QuestionSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QuestionTxt] nvarchar(max)  NOT NULL,
    [Deep] int  NOT NULL,
    [AnswerId] int  NULL,
    [PropertyId] int  NULL
);
GO

-- Creating table 'AnswerSet'
CREATE TABLE [dbo].[AnswerSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Val] nvarchar(max)  NOT NULL,
    [QuestionId] int  NULL
);
GO

-- Creating table 'NoteSet'
CREATE TABLE [dbo].[NoteSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ValId] int  NULL,
    [ItemId] int  NULL
);
GO

-- Creating table 'SelectorSet'
CREATE TABLE [dbo].[SelectorSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AnswerId] int  NULL,
    [ValId] int  NULL,
    [Probability] float  NOT NULL
);
GO

-- Creating table 'ExpertSystemSet'
CREATE TABLE [dbo].[ExpertSystemSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [UserId] int  NULL,
    [IsPublished] bit  NOT NULL
);
GO

-- Creating table 'UserSet'
CREATE TABLE [dbo].[UserSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Login] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [LastVisited] datetime  NOT NULL
);
GO

-- Creating table 'LogSet'
CREATE TABLE [dbo].[LogSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QId] int  NOT NULL,
    [AnswId] int  NOT NULL,
    [SurveyId] int  NULL,
    [PropId] int  NOT NULL,
    [LogId] int  NULL
);
GO

-- Creating table 'SurveySet'
CREATE TABLE [dbo].[SurveySet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ExpertSystemId] int  NULL,
    [Type] nvarchar(max)  NOT NULL,
    [UserId] int  NOT NULL,
    [IsPropbability] bit  NOT NULL
);
GO

-- Creating table 'SurveyItemSet'
CREATE TABLE [dbo].[SurveyItemSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ItemId] int  NULL,
    [SurveyId] int  NULL,
    [Probability] float  NOT NULL
);
GO

-- Creating table 'TempLogItemSet'
CREATE TABLE [dbo].[TempLogItemSet] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [LogId] int  NULL,
    [ItemId] int  NULL,
    [Probability] float  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'ItemSet'
ALTER TABLE [dbo].[ItemSet]
ADD CONSTRAINT [PK_ItemSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PropertySet'
ALTER TABLE [dbo].[PropertySet]
ADD CONSTRAINT [PK_PropertySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ValSet'
ALTER TABLE [dbo].[ValSet]
ADD CONSTRAINT [PK_ValSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'QuestionSet'
ALTER TABLE [dbo].[QuestionSet]
ADD CONSTRAINT [PK_QuestionSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AnswerSet'
ALTER TABLE [dbo].[AnswerSet]
ADD CONSTRAINT [PK_AnswerSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'NoteSet'
ALTER TABLE [dbo].[NoteSet]
ADD CONSTRAINT [PK_NoteSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SelectorSet'
ALTER TABLE [dbo].[SelectorSet]
ADD CONSTRAINT [PK_SelectorSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpertSystemSet'
ALTER TABLE [dbo].[ExpertSystemSet]
ADD CONSTRAINT [PK_ExpertSystemSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserSet'
ALTER TABLE [dbo].[UserSet]
ADD CONSTRAINT [PK_UserSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LogSet'
ALTER TABLE [dbo].[LogSet]
ADD CONSTRAINT [PK_LogSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SurveySet'
ALTER TABLE [dbo].[SurveySet]
ADD CONSTRAINT [PK_SurveySet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SurveyItemSet'
ALTER TABLE [dbo].[SurveyItemSet]
ADD CONSTRAINT [PK_SurveyItemSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TempLogItemSet'
ALTER TABLE [dbo].[TempLogItemSet]
ADD CONSTRAINT [PK_TempLogItemSet]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PropertyId] in table 'ValSet'
ALTER TABLE [dbo].[ValSet]
ADD CONSTRAINT [FK_PropertyVal]
    FOREIGN KEY ([PropertyId])
    REFERENCES [dbo].[PropertySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PropertyVal'
CREATE INDEX [IX_FK_PropertyVal]
ON [dbo].[ValSet]
    ([PropertyId]);
GO

-- Creating foreign key on [ValId] in table 'NoteSet'
ALTER TABLE [dbo].[NoteSet]
ADD CONSTRAINT [FK_ValNote]
    FOREIGN KEY ([ValId])
    REFERENCES [dbo].[ValSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ValNote'
CREATE INDEX [IX_FK_ValNote]
ON [dbo].[NoteSet]
    ([ValId]);
GO

-- Creating foreign key on [ItemId] in table 'NoteSet'
ALTER TABLE [dbo].[NoteSet]
ADD CONSTRAINT [FK_ItemNote]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[ItemSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemNote'
CREATE INDEX [IX_FK_ItemNote]
ON [dbo].[NoteSet]
    ([ItemId]);
GO

-- Creating foreign key on [QuestionId] in table 'AnswerSet'
ALTER TABLE [dbo].[AnswerSet]
ADD CONSTRAINT [FK_QuestionAnswer]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[QuestionSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionAnswer'
CREATE INDEX [IX_FK_QuestionAnswer]
ON [dbo].[AnswerSet]
    ([QuestionId]);
GO

-- Creating foreign key on [AnswerId] in table 'QuestionSet'
ALTER TABLE [dbo].[QuestionSet]
ADD CONSTRAINT [FK_AnswerQuestion]
    FOREIGN KEY ([AnswerId])
    REFERENCES [dbo].[AnswerSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AnswerQuestion'
CREATE INDEX [IX_FK_AnswerQuestion]
ON [dbo].[QuestionSet]
    ([AnswerId]);
GO

-- Creating foreign key on [PropertyId] in table 'QuestionSet'
ALTER TABLE [dbo].[QuestionSet]
ADD CONSTRAINT [FK_PropertyQuestion]
    FOREIGN KEY ([PropertyId])
    REFERENCES [dbo].[PropertySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_PropertyQuestion'
CREATE INDEX [IX_FK_PropertyQuestion]
ON [dbo].[QuestionSet]
    ([PropertyId]);
GO

-- Creating foreign key on [AnswerId] in table 'SelectorSet'
ALTER TABLE [dbo].[SelectorSet]
ADD CONSTRAINT [FK_AnswerSelector]
    FOREIGN KEY ([AnswerId])
    REFERENCES [dbo].[AnswerSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_AnswerSelector'
CREATE INDEX [IX_FK_AnswerSelector]
ON [dbo].[SelectorSet]
    ([AnswerId]);
GO

-- Creating foreign key on [ValId] in table 'SelectorSet'
ALTER TABLE [dbo].[SelectorSet]
ADD CONSTRAINT [FK_ValSelector]
    FOREIGN KEY ([ValId])
    REFERENCES [dbo].[ValSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ValSelector'
CREATE INDEX [IX_FK_ValSelector]
ON [dbo].[SelectorSet]
    ([ValId]);
GO

-- Creating foreign key on [ExpertSystemId] in table 'PropertySet'
ALTER TABLE [dbo].[PropertySet]
ADD CONSTRAINT [FK_ExpertSystemProperty]
    FOREIGN KEY ([ExpertSystemId])
    REFERENCES [dbo].[ExpertSystemSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpertSystemProperty'
CREATE INDEX [IX_FK_ExpertSystemProperty]
ON [dbo].[PropertySet]
    ([ExpertSystemId]);
GO

-- Creating foreign key on [ExpertSystemId] in table 'ItemSet'
ALTER TABLE [dbo].[ItemSet]
ADD CONSTRAINT [FK_ExpertSystemItem]
    FOREIGN KEY ([ExpertSystemId])
    REFERENCES [dbo].[ExpertSystemSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpertSystemItem'
CREATE INDEX [IX_FK_ExpertSystemItem]
ON [dbo].[ItemSet]
    ([ExpertSystemId]);
GO

-- Creating foreign key on [UserId] in table 'ExpertSystemSet'
ALTER TABLE [dbo].[ExpertSystemSet]
ADD CONSTRAINT [FK_UserExpertSystem]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[UserSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UserExpertSystem'
CREATE INDEX [IX_FK_UserExpertSystem]
ON [dbo].[ExpertSystemSet]
    ([UserId]);
GO

-- Creating foreign key on [SurveyId] in table 'LogSet'
ALTER TABLE [dbo].[LogSet]
ADD CONSTRAINT [FK_SurveyLog]
    FOREIGN KEY ([SurveyId])
    REFERENCES [dbo].[SurveySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SurveyLog'
CREATE INDEX [IX_FK_SurveyLog]
ON [dbo].[LogSet]
    ([SurveyId]);
GO

-- Creating foreign key on [ExpertSystemId] in table 'SurveySet'
ALTER TABLE [dbo].[SurveySet]
ADD CONSTRAINT [FK_ExpertSystemSurvey]
    FOREIGN KEY ([ExpertSystemId])
    REFERENCES [dbo].[ExpertSystemSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpertSystemSurvey'
CREATE INDEX [IX_FK_ExpertSystemSurvey]
ON [dbo].[SurveySet]
    ([ExpertSystemId]);
GO

-- Creating foreign key on [LogId] in table 'LogSet'
ALTER TABLE [dbo].[LogSet]
ADD CONSTRAINT [FK_LogLog]
    FOREIGN KEY ([LogId])
    REFERENCES [dbo].[LogSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LogLog'
CREATE INDEX [IX_FK_LogLog]
ON [dbo].[LogSet]
    ([LogId]);
GO

-- Creating foreign key on [ItemId] in table 'SurveyItemSet'
ALTER TABLE [dbo].[SurveyItemSet]
ADD CONSTRAINT [FK_ItemSurveyItem]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[ItemSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemSurveyItem'
CREATE INDEX [IX_FK_ItemSurveyItem]
ON [dbo].[SurveyItemSet]
    ([ItemId]);
GO

-- Creating foreign key on [SurveyId] in table 'SurveyItemSet'
ALTER TABLE [dbo].[SurveyItemSet]
ADD CONSTRAINT [FK_SurveySurveyItem]
    FOREIGN KEY ([SurveyId])
    REFERENCES [dbo].[SurveySet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SurveySurveyItem'
CREATE INDEX [IX_FK_SurveySurveyItem]
ON [dbo].[SurveyItemSet]
    ([SurveyId]);
GO

-- Creating foreign key on [LogId] in table 'TempLogItemSet'
ALTER TABLE [dbo].[TempLogItemSet]
ADD CONSTRAINT [FK_LogTempRemovedItem]
    FOREIGN KEY ([LogId])
    REFERENCES [dbo].[LogSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_LogTempRemovedItem'
CREATE INDEX [IX_FK_LogTempRemovedItem]
ON [dbo].[TempLogItemSet]
    ([LogId]);
GO

-- Creating foreign key on [ItemId] in table 'TempLogItemSet'
ALTER TABLE [dbo].[TempLogItemSet]
ADD CONSTRAINT [FK_ItemTempRemovedItem]
    FOREIGN KEY ([ItemId])
    REFERENCES [dbo].[ItemSet]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemTempRemovedItem'
CREATE INDEX [IX_FK_ItemTempRemovedItem]
ON [dbo].[TempLogItemSet]
    ([ItemId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------