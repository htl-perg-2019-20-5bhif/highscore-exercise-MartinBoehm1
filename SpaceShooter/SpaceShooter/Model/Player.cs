using System;
using System.ComponentModel.DataAnnotations;

namespace SpaceShooter
{
    public class Player
    {
        public int PlayerId { get; set; }
        [Required]
        public String Name { get; set; }
        [Required]
        public int Score { get; set; }
        
    }
}
