﻿using RLNET;
using RogueSharp;
using RoguelikeCL.interfaces;
using RoguelikeCL.Core;
using RogueMain;
using RogueMain.Core;

namespace RoguelikeCL.Core
{
   public class Item : Iitem, ITreasure, IDrawable
   {
      public Item()
      {
         Symbol = '!';
         Color = RLColor.LightBlue;
      }

      public string Name { get; protected set; }
      public int RemainingUses { get; protected set; }

      public bool Use()
      {
         return UseItem();
      }

      protected virtual bool UseItem()
      {
         return false;
      }

      public bool PickUp( IActor actor )
      {
            // возможно потом нужно будет исправить
            if (actor is Player player)
            {
                if (player.AddItem(this))
                {
                    RogueGame.MessLogs.AddLine($"{actor.Name} picked up {Name}");
                    return true;
                }
            }

            return false;
      }

      public RLColor Color { get; set; }

      public char Symbol { get; set; }

      public int X { get; set; }

      public int Y { get; set; }

      public void Draw( RLConsole console, IMap map )
      {
         if ( !map.IsExplored( X, Y ) )
         {
            return;
         }

         if ( map.IsInFov( X, Y ) )
         {
            console.Set( X, Y, Color, Colors.FloorBackgroundFov, Symbol );
         }
         else
         {
            console.Set( X, Y, RLColor.Blend( Color, RLColor.Gray, 0.5f ), Colors.FloorBackground, Symbol );
         }
      }
   }
}
