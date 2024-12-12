using RoguelikeCL.Core;
using RogueMain;

namespace RoguelikeCL.Abilities
{
   public class DoNothing : Ability
   {
      public DoNothing()
      {
         Name = "None";
         TurnsToRefresh = 0;
         TurnsUntilRefreshed = 0;
      }

      protected override bool PerformAbility()
      {
         RogueGame.MessLogs.AddLine( "No ability in that slot" );
         return false;
      }
   }
}
