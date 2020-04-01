using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceShooter
{
    public class PlayerComparer
    {
        public int comparer(Player p1, Player p2)
        {
            return p2.Score - p1.Score;
        }
    }
}
