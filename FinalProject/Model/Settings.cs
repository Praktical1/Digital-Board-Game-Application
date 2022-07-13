using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FinalProject.Model
{
    public class Settings
    {
        public string background { get; set; }
        public string style { get; set; }
        public string userId { get; set; }
        public string password { get; set; }
        public string gameType { get; set; }
        public Boolean forcedMove { get; set; }

        private static Random rng = new Random();

        public Settings()
        {
            this.background = "../Images/Background.png";
            this.userId = "Guest_" + rng.Next(99999999).ToString();
            this.password = "";
            this.gameType = "Checkers";
            this.forcedMove = true;
        }

    }
}
