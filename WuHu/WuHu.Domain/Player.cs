using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WuHu.Common;

namespace WuHu.Domain
{
    [DataContract(Namespace = "http://WuHu.Domain")]
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
            this.Salt = CryptoService.GenerateSalt();
            this.Password = CryptoService.HashPassword(password, this.Salt);
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

        [DataMember]
        public int? PlayerId { get; set; }
        [DataMember]
        public string Firstname { get; set; }

        [DataMember]
        public string Lastname { get; set; }

        [DataMember]
        public string Nickname { get; set; }

        [DataMember]
        public string Username { get; }

        [DataMember]
        public byte[] Password { get; set; }

        [DataMember]
        public byte[] Salt { get; set; }

        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public bool PlaysMondays { get; set; }
        [DataMember]
        public bool PlaysTuesdays { get; set; }
        [DataMember]
        public bool PlaysWednesdays { get; set; }
        [DataMember]
        public bool PlaysThursdays { get; set; }
        [DataMember]
        public bool PlaysFridays { get; set; }
        [DataMember]
        public bool PlaysSaturdays { get; set; }
        [DataMember]
        public bool PlaysSundays { get; set; }

        // https://stackoverflow.com/questions/25400555/save-and-retrieve-image-binary-from-sql-server-using-entity-framework-6
        [DataMember]
        public byte[] Picture { get; set; }

        public override string ToString()
        {
            return Firstname + " '" + Nickname + "' " + Lastname;
        }

        //playerId can be null, username is a better unique identifier
        public override bool Equals(object other)
        {
            var player = other as Player;
            return player != null && string.Equals(Username, player.Username, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return (Username != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Username) : 0);
        }
    }
}
