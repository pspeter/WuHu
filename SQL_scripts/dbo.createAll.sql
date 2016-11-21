
CREATE TABLE [dbo].[Match]
( 
	[tournamentId]       int  NOT NULL,
	[matchId]            int  NOT NULL  IDENTITY ( 0,1 ),
	[time]               Datetime2  NOT NULL ,
    [player1]    INT			 NOT NULL,
    [player2]    INT			 NOT NULL,
    [player3]    INT			 NOT NULL,
    [player4]    INT			 NOT NULL,
	[scoreTeam1]         TINYINT  NULL ,
	[scoreTeam2]         TINYINT  NULL ,
	[estimatedWinChance]       FLOAT  NOT NULL ,
	[isDone]             bit  NOT NULL 
)
go

ALTER TABLE [dbo].[Match]
	ADD CONSTRAINT [XPKMatch] PRIMARY KEY  CLUSTERED ([matchId] ASC)
go

CREATE TABLE [dbo].[Player]
( 
	[playerId]           int  NOT NULL  IDENTITY ( 0,1 ) ,
	[firstName]          nchar(100)  NOT NULL ,
	[lastName]           nchar(100)  NOT NULL ,
	[nickName]           nchar(20)  NULL ,
	[userName]           nchar(20)  NOT NULL ,
	[password]           nchar(100)  NOT NULL ,
	[salt]               VARBINARY(32)   NOT NULL ,
	[picture]            binary  NULL ,
	[isAdmin]            bit    NOT NULL,
    [playsMondays]       bit    NOT NULL,
    [playsTuesdays]      bit    NOT NULL,
    [playsWednesdays]    bit    NOT NULL,
    [playsThursdays]     bit    NOT NULL,
    [playsFridays]       bit    NOT NULL,
    [playsSaturdays]     bit    NOT NULL,
    [playsSundays]       bit    NOT NULL
)
go

ALTER TABLE [dbo].[Player]
	ADD CONSTRAINT [XPKPlayer] PRIMARY KEY  CLUSTERED ([playerId] ASC)
go

CREATE TABLE [dbo].[Rating]
( 
	[ratingId]           int  NOT NULL ,
	[playerId]           int  NOT NULL ,
	[date]               Datetime2  NOT NULL 
	CONSTRAINT [now_timestamp]
		 DEFAULT  CURRENT_TIMESTAMP,
	[value]              int  NULL 
)
go

ALTER TABLE [dbo].[Rating]
	ADD CONSTRAINT [XPKRating] PRIMARY KEY  CLUSTERED ([ratingId] ASC)
go

CREATE TABLE [dbo].[ScoreParameter]
( 
	[key]                nchar(200)  NOT NULL,
	[name]               nchar(200)  NOT NULL 
)
go

ALTER TABLE [dbo].[ScoreParameter]
	ADD CONSTRAINT [XPKScoreParameter] PRIMARY KEY  CLUSTERED ([key] ASC)
go

CREATE TABLE [dbo].[Tournament]
( 
	[tournamentId]       int  NOT NULL  IDENTITY ( 0,1 ) ,
	[name]               nchar(50)  NOT NULL ,
	[creator]           int  NULL 
)
go

ALTER TABLE [dbo].[Tournament]
	ADD CONSTRAINT [XPKTournament] PRIMARY KEY  CLUSTERED ([tournamentId] ASC)
go

ALTER TABLE [dbo].[Rating]
	ADD CONSTRAINT [R_1] FOREIGN KEY ([playerId]) REFERENCES [Player]([playerId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
go

ALTER TABLE [dbo].[Match]
	ADD CONSTRAINT [R_9] FOREIGN KEY ([tournamentId]) REFERENCES [Tournament]([tournamentId])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go



ALTER TABLE [dbo].[Match]
	ADD CONSTRAINT [R_10] FOREIGN KEY ([player1]) REFERENCES [Player]([playerId])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [dbo].[Match]
	ADD CONSTRAINT [R_11] FOREIGN KEY ([player2]) REFERENCES [Player]([playerId])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [dbo].[Match]
	ADD CONSTRAINT [R_12] FOREIGN KEY ([player3]) REFERENCES [Player]([playerId])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [dbo].[Match]
	ADD CONSTRAINT [R_13] FOREIGN KEY ([player4]) REFERENCES [Player]([playerId])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE [dbo].[Tournament]
	ADD CONSTRAINT [R_2] FOREIGN KEY ([creator]) REFERENCES [Player]([playerId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
go

