using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lb2.DataBase
{
    public class PlayerTeam
    {
        public PlayerTeam()
        {
            Player = new Player();
            Team = new Team();
        }
        public int Id { get; set; }
        public Player Player { get; set; }
        public Team Team { get; set; }
    }
}
