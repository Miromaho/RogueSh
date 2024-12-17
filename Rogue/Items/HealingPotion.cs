using RoguelikeCL.Abilities;
using RoguelikeCL.Core;
using RogueMain;
using RogueSharp.DiceNotation;

namespace RoguelikeCL.Items
{
   public class HealingPotion : Item
   {
      public HealingPotion()
      {
         Name = "Healing Potion";
         RemainingUses = 1;
      }

      protected override bool UseItem()
      {
         int healAmount = Dice.Roll("2d4 + 2 - 1d4");
         RogueGame.MessLogs.AddLine( $"{RogueGame.Player.Name} consumes a {Name} and recovers {healAmount} health" );  

         Heal heal = new Heal( healAmount );

         RemainingUses--;

         return heal.Perform();
      }
   }
}
