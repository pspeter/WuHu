CREATE TABLE [dbo].[Gameplan]
(
	[gameplanId] INT NOT NULL PRIMARY KEY, 
    [gameplanName] NCHAR(100) NOT NULL, 
    [createdBy] INT NOT NULL, 
    CONSTRAINT [FK_Player_to_Gameplan] FOREIGN KEY ([createdBy]) REFERENCES [Player]([player_id])
)