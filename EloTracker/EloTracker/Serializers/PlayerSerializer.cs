using EloTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace EloTracker.ObjectSerializers
{
    [Serializable]
    public class PlayerSerializer : ISerializable
    {
        public readonly Guid Guid;
        public readonly string Name;
        public readonly int Score;

        public PlayerSerializer() { }

        public PlayerSerializer(Player player)
        {
            Guid = player.Guid;
            Name = player.Name;
            Score = player.Score;
        }

        public PlayerSerializer(SerializationInfo info, StreamingContext context)
        {
            Guid = (Guid)info.GetValue("Guid", typeof(Guid));
            Name = (string)info.GetValue("Name", typeof(string));
            Score = (int)info.GetValue("Score", typeof(int));
        }

        public Player GetPlayer()
        {
            return Player.CreateExistingPlayer(Name, Score, Guid);
        }

        public static List<PlayerSerializer> SerializeList(IEnumerable<Player> players)
        {
            List<PlayerSerializer> pSerials = new List<PlayerSerializer>();
            foreach (Player player in players)
            {
                pSerials.Add(new PlayerSerializer(player));
            }
            return pSerials;
        }

        public static List<Player> UnserializeList(IEnumerable<PlayerSerializer> pSerials)
        {
            List<Player> players = new List<Player>();
            foreach (PlayerSerializer pSerial in pSerials)
            {
                players.Add(pSerial.GetPlayer());
            }
            return players;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Guid", Guid, typeof(Guid));
            info.AddValue("Name", Name, typeof(string));
            info.AddValue("Score", Score, typeof(int));
        }
    }
}
