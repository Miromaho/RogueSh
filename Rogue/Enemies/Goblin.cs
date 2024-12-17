using RogueMain.Core;
using RogueSharp.DiceNotation;
using RoguelikeCL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoguelikeCL.Behaviors;
using RoguelikeCL.System;

namespace RoguelikeCL.Enemies
{
    public class Goblin : Enemy
    {
        private int? turnsSpentRunning = null;
        public static Goblin Create(int level)
        {
            int health = Dice.Roll("2D6");
            return new Goblin
            {
                Attack = Dice.Roll("1D2") + level / 3,
                AttackChance = Dice.Roll("10D5"),
                Awareness = 10,
                Color = Colors.Goblin,
                Defense = Dice.Roll("1D2") + level / 3,
                DefenseChance = Dice.Roll("10D4"),
                Gold = Dice.Roll("1D20"),
                Health = health,
                MaxHealth = health,
                Name = "Goblin",
                Speed = 120,
                Symbol = 'g'
            };
        }
        public override void PerformAction(CommandSys commandSys)
        {
            var fullyHealBehavior = new FullyHeal();
            var runAwayBehavior = new RunAway();
            if (turnsSpentRunning.HasValue && turnsSpentRunning.Value > 15)
            {
                fullyHealBehavior.Act(this, commandSys);
                turnsSpentRunning = null;
            }
            else if (Health < MaxHealth)
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
            }
            else
            {
                base.PerformAction(commandSys);
            }
        }
    }
}
