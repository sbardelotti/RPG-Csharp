using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Motor;

namespace RPG_GAME
{
    public partial class f_rpg_game : Form
    {
        private Player _player;
        public f_rpg_game()
        {
            InitializeComponent();

            Location location = new Location(1, "House", "Your house");

            _player = new Player(10, 10, 20, 0, 1);

            lb_hitPoints.Text = _player.CurrentHitPoints.ToString();
            lb_gold.Text = _player.Gold.ToString();
            lb_experience.Text = _player.ExperiencePoints.ToString();
            lb_level.Text = _player.Level.ToString();
        }
    }
}
