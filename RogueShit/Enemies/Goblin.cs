using RogueMain.Core;
using RogueSharp.DiceNotation;
using RoguelikeCL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeCL.Enemies
{
    public class Goblin : Enemy
    {
        public static Goblin Create(int level)
        {
            int health = Dice.Roll("1D5");
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
                Speed = 123,
                Symbol = 'g'
            };
        }
    }
}
