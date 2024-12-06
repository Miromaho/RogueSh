using RogueMain;
using RogueSharp;
using RogueShit.Core;
using RogueShit.interfaces;
using RogueShit.System;
using System.Text;

namespace RogueShit.Behaviors
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
                RogueSharp.Path path = null;

                try
                {
                    path = pathFinder.ShortestPath(
                       dungeonMap.GetCell(enemy.X, enemy.Y),
                       dungeonMap.GetCell(player.X, player.Y));
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
                        commandSys.MoveEnemies(enemy, path.TryStepForward() );
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