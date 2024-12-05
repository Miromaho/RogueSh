using OpenTK.Input;
using RogueMain;
using RogueShit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueShit.System
{
    public class CommandSys
    {
        public static bool PlayersMove(Direction direction)
        {
            int x = RogueGame.Player.X;
            int y = RogueGame.Player.Y;

            switch (direction)
            {
                case Direction.Up:
                    {
                        y = RogueGame.Player.Y - 1;
                        break;
                    }
                case Direction.Down:
                    {
                        y = RogueGame.Player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = RogueGame.Player.X - 1;
                        break;
                    }
                case Direction.Right:
                    {
                        x = RogueGame.Player.X + 1;
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            if (RogueGame.DungeonMap.SetActorPosition(RogueGame.Player, x, y))
            {
                return true;
            }
          return false;
        }
    }
}

