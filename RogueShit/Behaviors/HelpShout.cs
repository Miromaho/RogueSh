using RoguelikeCL.Core;
using RoguelikeCL.interfaces;
using RoguelikeCL.System;
using RogueMain;
using RogueSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoguelikeCl.Behaviors
{
    public class HelpShout : IBehavior
    {
        public bool Act(Enemy enemy, CommandSys commandSys)
        {
            bool didShoutForHelp = false;
            DungeonMap dungeonMap = RogueGame.DungeonMap;
            FieldOfView enemyFov = new FieldOfView(dungeonMap);

            enemyFov.ComputeFov(enemy.X, enemy.Y, enemy.Awareness, true);
            foreach (var monsterLocation in dungeonMap.GetEnemyLocations())
            {
                if (enemyFov.IsInFov(monsterLocation.X, monsterLocation.Y))
                {
                    Enemy alertedMonster = dungeonMap.GetEnemyAt(monsterLocation.X, monsterLocation.Y);
                    if (!alertedMonster.TurnsAlerted.HasValue)
                    {
                        alertedMonster.TurnsAlerted = 1;
                        didShoutForHelp = true;
                    }
                }
            }

            if (didShoutForHelp)
            {
                RogueGame.MessLogs.AddLine($"{enemy.Name} shouts for help!");
            }

            return didShoutForHelp;
        }
    }
}
