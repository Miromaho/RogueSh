using RoguelikeCL.Core;
using RogueMain;
using RogueSharp;
using RogueSharp.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RoguelikeCl.Items
{
    public class DestructionWand : Item
    {
        public DestructionWand()
        {
            Name = "Wand of Destruction";
            RemainingUses = 3;
        }

        protected override bool UseItem()
        {
            DungeonMap map = RogueGame.DungeonMap;
            Player player = RogueGame.Player;
            Point edgePoint = GetRandomEdgePoint(map);

            RogueGame.MessLogs.AddLine($"{player.Name} uses a {Name} and chaotically unleashes a void beam");
            Actor voidAttackActor = new Actor
            {
                Attack = 6,
                AttackChance = 90,
                Name = "The Void"
            };
            Cell previousCell = null;
            foreach (Cell cell in map.GetCellsAlongLine(player.X, player.Y, edgePoint.X, edgePoint.Y))
            {
                if (cell.X == player.X && cell.Y == player.Y)
                {
                    continue;
                }

                Enemy enemy = map.GetEnemyAt(cell.X, cell.Y);
                if (enemy != null)
                {
                    RoguelikeCL.System.CommandSys.Attack(voidAttackActor, enemy);
                }
                else
                {
                    map.SetCellProperties(cell.X, cell.Y, true, true, true);
                    if (previousCell != null)
                    {
                        if (cell.X != previousCell.X || cell.Y != previousCell.Y)
                        {
                            map.SetCellProperties(cell.X + 1, cell.Y, true, true, true);
                        }
                    }
                    previousCell = cell;
                }
            }

            RemainingUses--;

            return true;
        }

        private Point GetRandomEdgePoint(DungeonMap map)
        {
            var random = new DotNetRandom();
            int result = random.Next(1, 4);
            switch (result)
            {
                case 1: // Вврех
                    {
                        return new Point(random.Next(3, map.Width - 3), 3);
                    }
                case 2: // Вниз
                    {
                        return new Point(random.Next(3, map.Width - 3), map.Height - 3);
                    }
                case 3: // Право
                    {
                        return new Point(map.Width - 3, random.Next(3, map.Height - 3));
                    }
                case 4: // Лево
                    {
                        return new Point(3, random.Next(3, map.Height - 3));
                    }
                default:
                    {
                        return new Point(3, 3);
                    }
            }
        }
    }
}
