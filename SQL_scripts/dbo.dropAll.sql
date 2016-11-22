IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE  TABLE_NAME = 'Player'))
BEGIN
    DROP TABLE dbo.Match;
    DROP TABLE dbo.Tournament;
    DROP TABLE dbo.Rating;
    DROP TABLE dbo.Player;
    DROP TABLE dbo.ScoreParameter;
END;