using RogueSharp;
using RoguelikeCL.Core;
using RogueMain;

namespace RoguelikeCL.Items
{
   public class TeleportScroll : Item
   {
      public TeleportScroll()
      {
         Name = "Teleport Scroll";
         RemainingUses = 1;
      }

      protected override bool UseItem()
      {
         DungeonMap map = RogueGame.DungeonMap;
         Player player = RogueGame.Player;

         RogueGame.MessLogs.AddLine( $"{player.Name} uses a {Name} and reappears in another place" );

         Point point = map.GetRandomLocation();
         
         map.SetActorPosition( player, point.X, point.Y );
         
         RemainingUses--;

         return true;
      }
   }
}

