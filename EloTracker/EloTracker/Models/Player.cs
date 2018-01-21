using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EloTracker.Models
{
    public enum PlayerWinState { Win = 1, Loss, Stalemate }

    public class Player : INotifyPropertyChanged
    {
        private int _score;

        public readonly Guid Guid;



        public string Name { get; }

        public int Score
        {
            get { return _score; }
            set
            {
                if (Score != value)
                {
                    _score = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Score"));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Player() { }
        private Player(string name, int score = 1200, Guid? guid = null)
        {
            this.Name = name;
            this.Score = score;
            if (guid.HasValue)
            {
                this.Guid = guid.Value;
            }
            else
            {
                Guid = Guid.NewGuid();
            }

        }

        public static Player CreateNewPlayer(string name)
        {
            return new Player(name);
        }
        public static Player CreateExistingPlayer(string name, int score, Guid guid)
        {
            return new Player(name, score, guid);
        }

        public void SetScore(int score)
        {
            this.Score = score;
        }

        public static int compareScores(Player p1, Player p2)
        {
            if (p1.Score > p2.Score)
            {
                return 1;
            }
            else if (p1.Score < p2.Score)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }


    }
}
