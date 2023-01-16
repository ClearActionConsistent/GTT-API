CREATE TABLE [dbo].[Sports] (
    [SportId]     INT           IDENTITY (1, 1) NOT NULL,
    [SportImage]  VARCHAR (255) DEFAULT (NULL) NULL,
    [SportName]   VARCHAR (255) DEFAULT (NULL) NULL,
    [SportType]   VARCHAR (100) DEFAULT (NULL) NULL,
    [CreatedBy]   CHAR (36)     DEFAULT (NULL) NULL,
    [UpdatedBy]   CHAR (36)     DEFAULT (NULL) NULL,
    [CreatedDate] DATETIME      DEFAULT (getdate()) NOT NULL,
    [UpdatedDate] DATETIME      DEFAULT (getdate()) NOT NULL,
    [IsActive]    TINYINT       DEFAULT ('1') NOT NULL,
    [IsDeleted]   TINYINT       DEFAULT ('0') NOT NULL,
    PRIMARY KEY CLUSTERED ([SportId] ASC),
    CHECK ([SportType]='Other Sports' OR [SportType]='Winter Sports' OR [SportType]='Water Sports' OR [SportType]='Cycle Sports' OR [SportType]='Foot Sports')
);

