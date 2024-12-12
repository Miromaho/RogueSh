using RogueMain;
using RogueSharp;
using RoguelikeCL.Core;
using RoguelikeCL.interfaces;
using RoguelikeCL.System;
using System.Text;
using System.Threading;
using Path = RogueSharp.Path;

namespace RoguelikeCL.Behaviors
{
    public class StandardMoveAndAttack : IBehavior
    { 
        public bool Act(Enemy enemy, CommandSys commandSys)
        {
            DungeonMap dungeonMap = RogueGame.DungeonMap;
            Player player = RogueGame.Player;
            FieldOfView monsterFov = new FieldOfView(dungeonMap);
            if (!enemy.TurnsAlerted.HasValue)
            {
                monsterFov.ComputeFov(enemy.X, enemy.Y, enemy.Awareness, true);
                if (monsterFov.IsInFov(player.X, player.Y))
                {
                    RogueGame.MessLogs.AddLine($"{enemy.Name} is eager to fight {player.Name}");
                    enemy.TurnsAlerted = 1;
                }
            }

            if (enemy.TurnsAlerted.HasValue)
            {
                dungeonMap.SetIsWalkable(enemy.X, enemy.Y, true);
                dungeonMap.SetIsWalkable(player.X, player.Y, true);

                PathFinder pathFinder = new PathFinder(dungeonMap);
                Path path = null;
                try
                {
                    path = pathFinder.ShortestPath(dungeonMap.GetCell(enemy.X, enemy.Y), dungeonMap.GetCell(player.X, player.Y));
                }
                catch (PathNotFoundException)
                {
                    RogueGame.MessLogs.AddLine($"{enemy.Name} waits for a turn");
                }

                dungeonMap.SetIsWalkable(enemy.X, enemy.Y, false);
                dungeonMap.SetIsWalkable(player.X, player.Y, false);

                if (path != null)
                {
                    try
                    {

                        commandSys.MoveEnemies(enemy, (Cell)path.StepForward());
                    }
                    catch (NoMoreStepsException)
                    {
                        RogueGame.MessLogs.AddLine($"{enemy.Name} growls in frustration");
                    }
                }

                enemy.TurnsAlerted++;
                if (enemy.TurnsAlerted > 15)
                {
                    enemy.TurnsAlerted = null;
                }
            }
            return true;
        }
    }   
}
