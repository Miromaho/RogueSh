using System.Collections.Generic;
using System.Linq;
using RLNET;
using RogueSharp;
using RoguelikeCL.Core;
using RoguelikeCL.interfaces;
using RogueMain;
using RogueMain.Core;

namespace RoguelikeCL.System
{
   public class TargetingSys
   {
      private enum SelectionType
      {
         None = 0,
         Target = 1,
         Area = 2,
         Line = 3
      }

      public bool IsPlayerTargeting { get; private set; }

      private Point cursorPosition;
      private List<Point> selectableTargets = new List<Point>();
      private int currentTargetIndex;
      private ITargetable targetable;
      private int area;
      private SelectionType selectionType;

      public bool SelectEnemy( ITargetable Targetable )
      {
         Initialize();
         selectionType = SelectionType.Target;
         DungeonMap map = RogueGame.DungeonMap;
         selectableTargets = map.GetEnemyLocationsInFieldOfView().ToList();
         targetable = Targetable;
         cursorPosition = selectableTargets.FirstOrDefault();
         if ( cursorPosition == null )
         {
            StopTargeting();
            return false;
         }

         IsPlayerTargeting = true;
         return true;
      }

      public bool SelectArea( ITargetable Targetable, int Area = 0 )
      {
         Initialize();
         selectionType = SelectionType.Area;
         Player player = RogueGame.Player;
         cursorPosition = new Point { X = player.X, Y = player.Y };
         targetable = Targetable;
         area = Area;

         IsPlayerTargeting = true;
         return true;
      }

      public bool SelectLine( ITargetable Targetable )
      {
         Initialize();
         selectionType = SelectionType.Line;
         Player player = RogueGame.Player;
         cursorPosition = new Point { X = player.X, Y = player.Y };
         targetable = Targetable;

         IsPlayerTargeting = true;
         return true;
      }

      private void StopTargeting()
      {
         IsPlayerTargeting = false;
         Initialize();
      }

      private void Initialize()
      {
         cursorPosition = Point.Zero;
         selectableTargets = new List<Point>();
         currentTargetIndex = 0;
         area = 0;
         targetable = null;
         selectionType = SelectionType.None;
      }

      public bool HandleKey( RLKey key )
      {
         if ( selectionType == SelectionType.Target )
         {
            HandleSelectableTargeting( key );
         }
         else if ( selectionType == SelectionType.Area )
         {
            HandleLocationTargeting( key );
         }
         else if ( selectionType == SelectionType.Line )
         {
            HandleLocationTargeting( key );
         }

         if ( key == RLKey.Enter )
         {
            targetable.SelectTarget( cursorPosition );
            StopTargeting();
            return true;
         }
         return false;
      }

      private void HandleSelectableTargeting( RLKey key )
      {
         if ( key == RLKey.Right || key == RLKey.Down )
         {
            currentTargetIndex++;
            if ( currentTargetIndex >= selectableTargets.Count )
            {
               currentTargetIndex = 0;
            }
            cursorPosition = selectableTargets[currentTargetIndex];
         }
         else if ( key == RLKey.Left || key == RLKey.Up )
         {
            currentTargetIndex--;
            if ( currentTargetIndex < 0 )
            {
               currentTargetIndex = selectableTargets.Count - 1;
            }
            cursorPosition = selectableTargets[currentTargetIndex];
         }
      }

      private void HandleLocationTargeting( RLKey key )
      {
         int x = cursorPosition.X;
         int y = cursorPosition.Y;
         DungeonMap map = RogueGame.DungeonMap;

         if ( key == RLKey.Right )
         {
            x++;
         }
         else if ( key == RLKey.Left )
         {
            x--;
         }
         else if ( key == RLKey.Up )
         {
            y--;
         }
         else if ( key == RLKey.Down )
         {
            y++;
         }

         if ( map.IsInFov( x, y ) )
         {
            cursorPosition.X = x;
            cursorPosition.Y = y;
         }
      }

      public void Draw( RLConsole mapConsole )
      {
         if ( IsPlayerTargeting )
         {
            DungeonMap map = RogueGame.DungeonMap;
            Player player = RogueGame.Player;
            if ( selectionType == SelectionType.Area )
            {
               foreach ( Cell cell in map.GetCellsInArea( cursorPosition.X, cursorPosition.Y, area ) )
               {
                  mapConsole.SetBackColor( cell.X, cell.Y, Palette.DbSun );
               }
            }
            else if ( selectionType == SelectionType.Line )
            {
               foreach ( Cell cell in map.GetCellsAlongLine( player.X, player.Y, cursorPosition.X, cursorPosition.Y ) )
               { 
                  mapConsole.SetBackColor( cell.X, cell.Y, Palette.DbSun );
               }
            }

            mapConsole.SetBackColor( cursorPosition.X, cursorPosition.Y, Palette.DbLight );
         }
      }
   }
}
