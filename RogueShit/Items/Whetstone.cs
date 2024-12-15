using RoguelikeCL.Core;
using RoguelikeCL.Equipment;
using RogueMain;
using RogueSharp.DiceNotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RoguelikeCl.Items
{
    public class Whetstone : Item
    {
        public Whetstone()
        {
            Name = "Whetstone";
            RemainingUses = 5;
        }

        protected override bool UseItem()
        {
            Player player = RogueGame.Player;

            if (player.Hand == HandEquipment.None())
            {
                RogueGame.MessLogs.AddLine($"{player.Name} is not holding anything they can sharpen");
            }
            else if (player.AttackChance >= 80)
            {
                RogueGame.MessLogs.AddLine($"{player.Name} cannot make their {player.Hand.Name} any sharper");
            }
            else
            {
                RogueGame.MessLogs.AddLine($"{player.Name} uses a {Name} to sharper their {player.Hand.Name}");
                player.Hand.AttackChance += Dice.Roll("1D3");
                RemainingUses--;
            }

            return true;
        }
    }
}
