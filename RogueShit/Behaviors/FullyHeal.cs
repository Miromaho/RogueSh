using RoguelikeCL.Core;
using RoguelikeCL.interfaces;
using RoguelikeCL.System;
using RogueMain;

namespace RoguelikeCL.Behaviors
{
   public class FullyHeal : IBehavior
   {
      public bool Act( Enemy enemy, CommandSys commandSys )
      {
         if ( enemy.Health < enemy.MaxHealth )
         {
            int healthToRecover = enemy.MaxHealth - enemy.Health;
            enemy.Health = enemy.MaxHealth;
            RogueGame.MessLogs.AddLine( $"{enemy.Name} catches his breath and recovers {healthToRecover} health" );    
            return true;
         }
         return false;
      }
   }
}
