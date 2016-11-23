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
        private static int SaltLengthLimit = 32;
        private static byte[] GetSalt() // auslagern? nix in Domain zu suchen
        {
            var salt = new byte[SaltLengthLimit];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        public Player(int playerId, string firstName, string lastName, string nickName, string userName, 
            byte[] password, byte[] salt, bool isAdmin, bool playsMondays,
            bool playsTuesdays, bool playsWednesdays, bool playsThursdays, 
            bool playsFridays, bool playsSaturdays, bool playsSundays,
            byte[] picture)
        { 
            this.PlayerId = playerId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.NickName = nickName;
            this.UserName = userName;
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
        
        public Player(string firstName, string lastName, string nickName, string userName,
            string password, bool isAdmin, bool playsMondays,
            bool playsTuesdays, bool playsWednesdays, bool playsThursdays,
            bool playsFridays, bool playsSaturdays, bool playsSundays,
            byte[] picture)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.NickName = nickName;
            this.UserName = userName;
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
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string NickName { get; set; }

        public string UserName { get; set; }

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
            return NickName;
        }
    }
}
