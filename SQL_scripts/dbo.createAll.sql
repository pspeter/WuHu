
CREATE TABLE [dbo].[Match]
( 
	[tournamentId]       int  NOT NULL,
	[matchId]            int  NOT NULL  IDENTITY ( 0,1 ),
	[time]               Datetime2  NOT NULL ,
    [player1]    INT			 NOT NULL,
    [player2]    INT			 NOT NULL,
    [player3]    INT			 NOT NULL,
    [player4]    INT			 NOT NULL,
	[scoreTeam1]         int  NULL ,
	[scoreTeam2]         int  NULL ,
	[deltaPoints]        int  NOT NULL ,
	[isDone]             bit  NOT NULL 
)
go

ALTER TABLE [dbo].[Match]
	ADD CONSTRAINT [XPKMatch] PRIMARY KEY  CLUSTERED ([tournamentId] ASC,[matchId] ASC)
go

CREATE TABLE [dbo].[Player]
( 
	[playerId]           int  NOT NULL  IDENTITY ( 0,1 ) ,
	[firstName]          nchar(100)  NOT NULL ,
	[lastName]           nchar(100)  NOT NULL ,
	[nickName]           nchar(20)  NULL ,
	[userName]           nchar(20)  NOT NULL ,
	[password]           nchar(100)  NOT NULL ,
	[picture]            binary  NULL ,
	[isAdmin]            bit  NOT NULL 
)
go

ALTER TABLE [dbo].[Player]
	ADD CONSTRAINT [XPKPlayer] PRIMARY KEY  CLUSTERED ([playerId] ASC)
go

CREATE TABLE [dbo].[Plays_on]
( 
	[Day]                nchar(10)  NOT NULL ,
	[playerId]           int  NOT NULL 
)
go

ALTER TABLE [dbo].[Plays_on]
	ADD CONSTRAINT [XPKPlays_on] PRIMARY KEY  CLUSTERED ([Day] ASC,[playerId] ASC)
go

CREATE TABLE [dbo].[Rating]
( 
	[playerId]           int  NOT NULL ,
	[date]               Datetime2  NOT NULL 
	CONSTRAINT [now_timestamp]
		 DEFAULT  CURRENT_TIMESTAMP,
	[value]              int  NULL 
)
go

ALTER TABLE [dbo].[Rating]
	ADD CONSTRAINT [XPKRating] PRIMARY KEY  CLUSTERED ([playerId] ASC,[date] ASC)
go

CREATE TABLE [dbo].[ScoreParameter]
( 
	[parameterId]        int  NOT NULL  IDENTITY ( 0,1 ) ,
	[value]              nchar(1000)  NOT NULL ,
	[name]               nchar(1000)  NOT NULL 
)
go

ALTER TABLE [dbo].[ScoreParameter]
	ADD CONSTRAINT [XPKScoreParameter] PRIMARY KEY  CLUSTERED ([parameterId] ASC)
go

CREATE TABLE [dbo].[Tournament]
( 
	[tournamentId]       int  NOT NULL  IDENTITY ( 0,1 ) ,
	[name]               nchar(50)  NOT NULL ,
	[playerId]           int  NULL 
)
go

ALTER TABLE [dbo].[Tournament]
	ADD CONSTRAINT [XPKTournament] PRIMARY KEY  CLUSTERED ([tournamentId] ASC)
go

CREATE TABLE [dbo].[Weekday]
( 
	[Day]                nchar(10)  NOT NULL 
)
go

ALTER TABLE [dbo].[Weekday]
	ADD CONSTRAINT [XPKWeekday] PRIMARY KEY  CLUSTERED ([Day] ASC)
go


ALTER TABLE [dbo].[Match]
	ADD CONSTRAINT [R_9] FOREIGN KEY ([tournamentId]) REFERENCES [Tournament]([tournamentId])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go


ALTER TABLE [dbo].[Plays_on]
	ADD CONSTRAINT [R_4] FOREIGN KEY ([Day]) REFERENCES [Weekday]([Day])
		ON DELETE CASCADE
		ON UPDATE CASCADE
go

ALTER TABLE [dbo].[Plays_on]
	ADD CONSTRAINT [R_5] FOREIGN KEY ([playerId]) REFERENCES [Player]([playerId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
go


ALTER TABLE [dbo].[Rating]
	ADD CONSTRAINT [R_1] FOREIGN KEY ([playerId]) REFERENCES [Player]([playerId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
go


ALTER TABLE [dbo].[Team]
	ADD CONSTRAINT [R_6] FOREIGN KEY ([playerId1]) REFERENCES [Player]([playerId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
go

ALTER TABLE [dbo].[Team]
	ADD CONSTRAINT [R_7] FOREIGN KEY ([playerId2]) REFERENCES [Player]([playerId])
		ON DELETE NO ACTION
		ON UPDATE NO ACTION
go

ALTER TABLE [dbo].[Team]
	ADD CONSTRAINT [R_8] FOREIGN KEY ([tournamentId],[matchId]) REFERENCES [Match]([tournamentId],[matchId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
go


ALTER TABLE [dbo].[Tournament]
	ADD CONSTRAINT [R_2] FOREIGN KEY ([playerId]) REFERENCES [Player]([playerId])
		ON DELETE CASCADE
		ON UPDATE CASCADE
go

