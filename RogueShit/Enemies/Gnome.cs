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
    public class Gnome : Enemy
    {
        public static Gnome Create(int level)
        {
            int health = Dice.Roll("1D3");
            return new Gnome
            {
                Attack = Dice.Roll("3D2") + level / 3,
                AttackChance = Dice.Roll("10D5"),
                Awareness = 15,
                Color = Colors.Gnome,
                Defense = Dice.Roll("1D3") + level / 3,
                DefenseChance = Dice.Roll("6D4"),
                Gold = Dice.Roll("5D20"),
                Health = health,
                MaxHealth = health,
                Name = "Gnome",
                Speed = 164,
                Symbol = 'G'
            };
        }
    }
}
