using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WuHu.Dal.SqlServer
{
    class Player
    {
        const string SQL_FIND_BY_ID =
          @"SELECT * FROM Player WHERE playerId = @playerId;";

        const string SQL_FIND_ALL = @"SELECT * FROM Player";

        const string SQL_UPDATE_BY_ID =
          @"UPDATE Player (firstName,lastName,nickName,userName,password,salt,isAdmin,
                    playsMondays,playsTuesdays,playsWednesdays,playsThursdays,playsFridays,playsSaturdays,playsSundays,picture)
            SET firstName = @firstName, 
                lastName = @lastName,
                nickName = @nickName,
                userName = @userName,
                password = @password,
                salt = @salt,
                isAdmin = @isAdmin,
                playsMondays = @playsMondays,
                playsTuesdays = @playsTuesdays,
                playsWednesdays = @playsWednesdays,
                playsThursdays = @playsThursdays,
                playsFridays = @playsFridays,
                playsSaturdays = @playsSaturdays,
                playsSundays = @playsSundays,
                picture = @picture
            WHERE playerId = @playerId";

        const string SQL_INSERT =
          @"INSERT INTO Player (firstName,lastName,nickName,userName,password,salt,isAdmin,
                    playsMondays,playsTuesdays,playsWednesdays,playsThursdays,playsFridays,playsSaturdays,playsSundays,picture)
            VALUES (@firstName, @lastName, @nickName, @userName, @password, @salt, @isAdmin, @playsMondays, 
                    @playsTuesdays, @playsWednesdays, @playsThursdays, @playsFridays, @playsSaturdays, @playsSundays, @picture)";
    }
}
