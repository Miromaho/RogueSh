using RogueSharp;
using RoguelikeCL.Core;
using RogueMain;

namespace RoguelikeCL.Items
{
   public class EscapeScroll : Item
   {
      public EscapeScroll()
      {
         Name = "Escape Scroll";
         RemainingUses = 1;
      }

      protected override bool UseItem()
      {
         DungeonMap map = RogueGame.DungeonMap;
         Player player = RogueGame.Player;

         RogueGame.MessLogs.AddLine( $"{player.Name} uses a {Name} and reappears in another place" );


         map.SetActorPosition( player, map.Rooms.Last().Center.X, map.Rooms.Last().Center.Y);
         
         RemainingUses--;

         return true;
      }
   }
}

