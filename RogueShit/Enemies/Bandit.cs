using RoguelikeCl.Behaviors;
using RoguelikeCL.Behaviors;
using RoguelikeCL.Core;
using RoguelikeCL.Enemies;
using RoguelikeCL.System;
using RogueMain.Core;
using RogueSharp.DiceNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeCl.Enemies
{
    public class Bandit : Enemy
    {
        private int? turnsSpentRunning = null;
        private bool shoutedForHelp = false;
        public static Bandit Create(int level)
        {
            int health = Dice.Roll("2D8 + 2");
            return new Bandit
            {
                Attack = Dice.Roll("2D4") + level / 3,
                AttackChance = Dice.Roll("10D5"),
                Awareness = 15,
                Color = Colors.Bandit,
                Defense = Dice.Roll("1D3") + level / 3,
                DefenseChance = Dice.Roll("6D4"),
                Gold = Dice.Roll("3D6"),
                Health = health,
                MaxHealth = health,
                Name = "Bandit",
                Speed = 100,
                Symbol = 'b'
            };
        }
        public override void PerformAction(CommandSys commandSys)
        {
            var HelpShoutBehavior = new HelpShout();
            var runAwayBehavior = new RunAway();
            if (Health < MaxHealth)
            {
                runAwayBehavior.Act(this, commandSys);
                if (turnsSpentRunning.HasValue)
                {
                    turnsSpentRunning += 1;
                }
                else
                {
                    turnsSpentRunning = 1;
                }
                if (!shoutedForHelp)
                {
                    shoutedForHelp = HelpShoutBehavior.Act(this, commandSys);
                }
            }
            else
            {
                base.PerformAction(commandSys);
            }
        }
    }
}
