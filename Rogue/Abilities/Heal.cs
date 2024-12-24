using System;
using RoguelikeCL.Core;
using RogueMain;

namespace RoguelikeCL.Abilities
{
   public class Heal : Ability
   {
      private readonly int amountToHeal;

      public Heal( int AmountToHeal = 10, int cooldown = 60 )
      {
         Name = "Heal Self";
         TurnsToRefresh = cooldown;
         TurnsUntilRefreshed = 0;
         amountToHeal = AmountToHeal;
      }

      protected override bool PerformAbility()
      {
         Player player = RogueGame.Player;

         player.Health = Math.Min( player.MaxHealth, player.Health + amountToHeal );

         return true;
      }
   }
}
