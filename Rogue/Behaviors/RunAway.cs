using RogueSharp;
using RoguelikeCL.Core;
using RoguelikeCL.interfaces;
using RoguelikeCL.System;
using RogueMain;

namespace RoguelikeCL.Behaviors
{
   public class RunAway : IBehavior
   {
      public bool Act( Enemy enemy, CommandSys commandSys )
      {
         DungeonMap dungeonMap = RogueGame.DungeonMap;
         Player player = RogueGame.Player;

         dungeonMap.SetIsWalkable( enemy.X, enemy.Y, true );
         dungeonMap.SetIsWalkable( player.X, player.Y, true );

         GoalMap goalMap = new GoalMap( dungeonMap );
         goalMap.AddGoal( player.X, player.Y, 0 );

         RogueSharp.Path path = null;
         try
         {
            path = goalMap.FindPathAvoidingGoals(enemy.X, enemy.Y );
         }
         catch ( PathNotFoundException )
         {
            RogueGame.MessLogs.AddLine( $"{enemy.Name} cowers in fear" );
         }

         dungeonMap.SetIsWalkable(enemy.X, enemy.Y, false );
         dungeonMap.SetIsWalkable( player.X, player.Y, false );

         if ( path != null )
         {
            try
            {
               commandSys.MoveEnemies( enemy, path.StepForward() );
            }
            catch ( NoMoreStepsException )
            {
               RogueGame.MessLogs.AddLine( $"{enemy.Name} cowers in fear" );
            }
         }

         return true;
      }
   }
}
