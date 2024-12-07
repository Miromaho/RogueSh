using RLNET;
using RogueMain.Core;
using RogueShit.Behaviors;
using RogueShit.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueShit.Core
{
    public class Enemy : Actor
    {
        public int? TurnsAlerted {get; set;}
        public void DrawStats(RLConsole statConsole, int position)
        {
            int yPosition = 13 + (position * 2);
            statConsole.Print(1, yPosition, Symbol.ToString(), Color);

            int width = Convert.ToInt32(((double)Health / (double)MaxHealth) * 16.0);
            int remainingWidth = 16 - width;

            statConsole.SetBackColor(3, yPosition, width, 1, Palette.Primary);
            statConsole.SetBackColor(3 + width, yPosition, remainingWidth, 1, Palette.PrimaryDarkest);

            statConsole.Print(2, yPosition, $": {Name}", Palette.DbLight);
        }
        public virtual void PerformAction(CommandSys commandSystem)
        {
            var behavior = new StandardMoveAndAttack();
            behavior.Act(this, commandSystem);
        }
    }
}
