using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;

namespace WuHu.Domain
{
    [Serializable]
    public class Player
    {
        public Player(int playerId, string firstname, string lastname, string nickname, string username, 
            byte[] password, byte[] salt, bool isAdmin, bool playsMondays,
            bool playsTuesdays, bool playsWednesdays, bool playsThursdays, 
            bool playsFridays, bool playsSaturdays, bool playsSundays,
            byte[] picture)
        { 
            this.PlayerId = playerId;
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Nickname = nickname;
            this.Username = username;
            this.Password = password;
            this.Salt = salt;
            this.IsAdmin = isAdmin;
            this.PlaysMondays = playsMondays;
            this.PlaysTuesdays = playsTuesdays;
            this.PlaysWednesdays = playsWednesdays;
            this.PlaysThursdays = playsThursdays;
            this.PlaysFridays = playsFridays;
            this.PlaysSaturdays = playsSaturdays;
            this.PlaysSundays = playsSundays;
            this.Picture = picture;
        }
        
        public Player(string firstname, string lastname, string nickname, string username,
            string password, bool isAdmin, bool playsMondays,
            bool playsTuesdays, bool playsWednesdays, bool playsThursdays,
            bool playsFridays, bool playsSaturdays, bool playsSundays,
            byte[] picture)
        {
            this.Firstname = firstname;
            this.Lastname = lastname;
            this.Nickname = nickname;
            this.Username = username;
            this.Salt = PasswordManager.GenerateSalt();
            this.Password = PasswordManager.HashPassword(password, this.Salt);
            this.IsAdmin = isAdmin;
            this.PlaysMondays = playsMondays;
            this.PlaysTuesdays = playsTuesdays;
            this.PlaysWednesdays = playsWednesdays;
            this.PlaysThursdays = playsThursdays;
            this.PlaysFridays = playsFridays;
            this.PlaysSaturdays = playsSaturdays;
            this.PlaysSundays = playsSundays;
            this.Picture = picture;
        }

        public int? PlayerId { get; set; }
        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Nickname { get; set; }

        public string Username { get; set; }

        public byte[] Password { get; private set; }

        public void ChangePassword (string password)
        {
            this.Salt = PasswordManager.GenerateSalt();
            this.Password = PasswordManager.HashPassword(password, this.Salt);
        }

        public byte[] Salt { get; set; }

        public bool IsAdmin { get; set; }
        public bool PlaysMondays { get; set; }
        public bool PlaysTuesdays { get; set; }
        public bool PlaysWednesdays { get; set; }
        public bool PlaysThursdays { get; set; }
        public bool PlaysFridays { get; set; }
        public bool PlaysSaturdays { get; set; }
        public bool PlaysSundays { get; set; }

        // https://stackoverflow.com/questions/25400555/save-and-retrieve-image-binary-from-sql-server-using-entity-framework-6
        public byte[] Picture { get; set; }

        public override string ToString()
        {
            return Firstname + " '" + Nickname + "' " + Lastname;
        }

        public override bool Equals(object obj)
        {
            //playerId can be null, username is a better unique identifier
            return Username == ((obj as Player)?.Username); 
        }
    }
}
