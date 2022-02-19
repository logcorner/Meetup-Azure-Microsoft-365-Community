CREATE TABLE [dbo].[ToDo] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Title ]        NVARCHAR (50) NOT NULL,
    [Description  ] NVARCHAR (50) NOT NULL,
    [Status ]       INT           NOT NULL,
    [ImageUrl]      NVARCHAR (50) NOT NULL
);

