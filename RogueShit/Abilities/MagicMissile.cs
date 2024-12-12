using RogueSharp;
using RoguelikeCL.Core;
using RoguelikeCL.interfaces;
using RogueMain;

namespace RoguelikeCL.Abilities
{
   public class MagicMissile : Ability, ITargetable
   {
      private readonly int attack;
      private readonly int attackChance;

      public MagicMissile( int Attack, int AttackChance)
      {
         Name = "Magic Missile";
         TurnsToRefresh = 10;
         TurnsUntilRefreshed = 0;
         attack = Attack;
         attackChance = AttackChance;
      }

      protected override bool PerformAbility()
      {
         return RogueGame.TargetingSys.SelectEnemy( this );
      }

      public void SelectTarget( Point target )
      {
         DungeonMap map = RogueGame.DungeonMap;
         Player player = RogueGame.Player;
         Enemy enemy = map.GetEnemyAt( target.X, target.Y );
         if ( enemy != null )
         {
            RogueGame.MessLogs.AddLine( $"{player.Name} casts a {Name} at {enemy.Name}" );
            Actor magicMissleActor = new Actor
            {
               Attack = attack, AttackChance = attackChance, Name = Name
            };
            System.CommandSys.Attack( magicMissleActor, enemy );
         }
      }
   }
}
