IF (NOT EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE  TABLE_NAME = 'Player'))
BEGIN

    CREATE TABLE [dbo].[Match]
    ( 
        [tournamentId]       int  NOT NULL,
        [matchId]            int  NOT NULL  IDENTITY ( 0,1 ),
        [datetime]               Datetime2  NOT NULL ,
        [player1]    INT			 NOT NULL,
        [player2]    INT			 NOT NULL,
        [player3]    INT			 NOT NULL,
        [player4]    INT			 NOT NULL,
        [scoreTeam1]         TINYINT  NULL ,
        [scoreTeam2]         TINYINT  NULL ,
        [estimatedWinChance]       FLOAT  NOT NULL ,
        [isDone]             bit  NOT NULL 
    );

    ALTER TABLE [dbo].[Match]
        ADD CONSTRAINT [XPKMatch] PRIMARY KEY  CLUSTERED ([matchId] ASC);

    CREATE TABLE [dbo].[Player]
    ( 
        [playerId]           int  NOT NULL  IDENTITY ( 0,1 ) ,
        [firstName]          nvarchar(100)  NOT NULL ,
        [lastName]           nvarchar(100)  NOT NULL ,
        [nickName]           nvarchar(20)  NULL ,
        [userName]           nvarchar(20)  UNIQUE NOT NULL ,
        [password]           VARBINARY(32)  NOT NULL ,
        [salt]               VARBINARY(32)   NOT NULL ,
        [picture]            VARBINARY(MAX)  NULL ,
        [isAdmin]            bit    NOT NULL,
        [playsMondays]       bit    NOT NULL,
        [playsTuesdays]      bit    NOT NULL,
        [playsWednesdays]    bit    NOT NULL,
        [playsThursdays]     bit    NOT NULL,
        [playsFridays]       bit    NOT NULL,
        [playsSaturdays]     bit    NOT NULL,
        [playsSundays]       bit    NOT NULL
    );

    ALTER TABLE [dbo].[Player]
        ADD CONSTRAINT [XPKPlayer] PRIMARY KEY  CLUSTERED ([playerId] ASC);

    CREATE TABLE [dbo].[Rating]
    ( 
        [ratingId]           int  NOT NULL IDENTITY(0, 1),
        [playerId]           int  NOT NULL ,
        [datetime]               Datetime2  NOT NULL 
        CONSTRAINT [now_timestamp]
             DEFAULT  CURRENT_TIMESTAMP,
        [value]              int  NULL 
    );

    ALTER TABLE [dbo].[Rating]
        ADD CONSTRAINT [XPKRating] PRIMARY KEY  CLUSTERED ([ratingId] ASC);

    CREATE TABLE [dbo].[ScoreParameter]
    ( 
        [key]                nvarchar(200)  NOT NULL,
        [value]               nvarchar(200)  NOT NULL 
    );

    ALTER TABLE [dbo].[ScoreParameter]
        ADD CONSTRAINT [XPKScoreParameter] PRIMARY KEY  CLUSTERED ([key] ASC);

    CREATE TABLE [dbo].[Tournament]
    ( 
        [tournamentId]       int  NOT NULL  IDENTITY ( 0,1 ) ,
        [name]               nvarchar(50)  NOT NULL ,
        [datetime]               Datetime2  NOT NULL
    );

    ALTER TABLE [dbo].[Tournament]
        ADD CONSTRAINT [XPKTournament] PRIMARY KEY  CLUSTERED ([tournamentId] ASC);

    ALTER TABLE [dbo].[Rating]
        ADD CONSTRAINT [FK_Rating_playerId] FOREIGN KEY ([playerId]) REFERENCES [Player]([playerId])
            ON DELETE CASCADE
            ON UPDATE CASCADE;

    ALTER TABLE [dbo].[Match]
        ADD CONSTRAINT [FK_Match_tournamentId] FOREIGN KEY ([tournamentId]) REFERENCES [Tournament]([tournamentId])
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;

    ALTER TABLE [dbo].[Match]
        ADD CONSTRAINT [FK_Match_player1] FOREIGN KEY ([player1]) REFERENCES [Player]([playerId])
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;

    ALTER TABLE [dbo].[Match]
        ADD CONSTRAINT [FK_Match_player2] FOREIGN KEY ([player2]) REFERENCES [Player]([playerId])
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;

    ALTER TABLE [dbo].[Match]
        ADD CONSTRAINT [FK_Match_player3] FOREIGN KEY ([player3]) REFERENCES [Player]([playerId])
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;

    ALTER TABLE [dbo].[Match]
        ADD CONSTRAINT [FK_Match_player4] FOREIGN KEY ([player4]) REFERENCES [Player]([playerId])
            ON DELETE NO ACTION
            ON UPDATE NO ACTION;

END;
