CREATE TABLE [dbo].[ToDoItem] (
    [Id]            INT           IDENTITY (1, 1) NOT NULL,
    [Title ]        NVARCHAR (50) NOT NULL,
    [Description  ] NVARCHAR (50) NOT NULL,
    [Status ]       INT           NOT NULL,
    [AssignedTo]    NVARCHAR (50) NOT NULL,
    [TodoId]        INT           NOT NULL
);

