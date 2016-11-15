CREATE TABLE [dbo].[Plays_on]
(
	[playerId] INT NOT NULL , 
    [day] NCHAR(9) NOT NULL, 
    CONSTRAINT [FK_Player_to_Plays_on] FOREIGN KEY ([playerId]) REFERENCES [Player]([player_id]), 
    CONSTRAINT [FK_Weekday_to_Plays_on] FOREIGN KEY ([day]) REFERENCES [Weekday]([day]), 
    CONSTRAINT [PK_Plays_on] PRIMARY KEY ([playerId], [day]) 
)
