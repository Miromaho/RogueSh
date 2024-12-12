using RoguelikeCL.Abilities;
using RoguelikeCL.Core;
using RogueMain;

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
         int healAmount = 15;
         RogueGame.MessLogs.AddLine( $"{RogueGame.Player.Name} consumes a {Name} and recovers {healAmount} health" );  

         Heal heal = new Heal( healAmount );

         RemainingUses--;

         return heal.Perform();
      }
   }
}
