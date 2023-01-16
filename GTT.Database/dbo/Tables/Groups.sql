CREATE TABLE [dbo].[Groups] (
    [GroupId]     INT           IDENTITY (1, 1) NOT NULL,
    [GroupName]   VARCHAR (255) NOT NULL,
    [GroupImage]  VARCHAR (255) CONSTRAINT [DF__Groups__GroupIma__59063A47] DEFAULT (NULL) NULL,
    [Description] VARCHAR (255) CONSTRAINT [DF__Groups__Descript__59FA5E80] DEFAULT (NULL) NULL,
    [Website]     VARCHAR (255) CONSTRAINT [DF__Groups__Website__5AEE82B9] DEFAULT (NULL) NULL,
    [Location]    VARCHAR (255) NOT NULL,
    [GroupType]   VARCHAR (100) NOT NULL,
    [Sport]       INT           NOT NULL,
    [TotalRunner] INT           NULL,
    [CreatedBy]   CHAR (36)     CONSTRAINT [DF__Groups__CreatedB__5CD6CB2B] DEFAULT (NULL) NULL,
    [UpdatedBy]   CHAR (36)     CONSTRAINT [DF__Groups__UpdatedB__5DCAEF64] DEFAULT (NULL) NULL,
    [CreatedDate] DATETIME      CONSTRAINT [DF__Groups__CreatedD__5EBF139D] DEFAULT (getdate()) NOT NULL,
    [UpdatedDate] DATETIME      CONSTRAINT [DF__Groups__UpdatedD__5FB337D6] DEFAULT (getdate()) NOT NULL,
    [IsActive]    TINYINT       CONSTRAINT [DF__Groups__IsActive__60A75C0F] DEFAULT ('1') NOT NULL,
    [IsDeleted]   TINYINT       CONSTRAINT [DF__Groups__IsDelete__619B8048] DEFAULT ('0') NULL,
    CONSTRAINT [PK__Groups__149AF36A9576E7DF] PRIMARY KEY CLUSTERED ([GroupId] ASC),
    CONSTRAINT [CK__Groups__GroupTyp__5BE2A6F2] CHECK ([GroupType]='Other' OR [GroupType]='Shop' OR [GroupType]='Club' OR [GroupType]='Racing Team' OR [GroupType]='Company/Workplace'),
    CONSTRAINT [FK_Groups_Sports] FOREIGN KEY ([Sport]) REFERENCES [dbo].[Sports] ([SportId])
);

