using System;

namespace SpaceShooter
{
    public class Player
    {
        public Player(string name, int score)
        {
            Name = name;
            Score = score;
        }

        public String Name { get; set; }

        public int Score { get; set; }
        
    }
}
