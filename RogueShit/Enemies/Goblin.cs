using RogueMain.Core;
using RogueSharp.DiceNotation;
using RogueShit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueShit.Enemies
{
    public class Goblin : Enemy
    {
        public static Goblin Create(int level)
        {
            int health = Dice.Roll("2D6");
            return new Goblin
            {
                Attack = Dice.Roll("1D6") + level / 3,
                AttackChance = Dice.Roll("1D20"),
                Awareness = 10,
                Color = Colors.Goblin,
                Defense = Dice.Roll("1D3") + level / 3,
                DefenseChance = Dice.Roll("1D20"),
                Gold = Dice.Roll("2D6"),
                Health = health,
                MaxHealth = health,
                Name = "Goblin",
                Speed = 14,
                Symbol = 'g'
            };
        }
    }
}
