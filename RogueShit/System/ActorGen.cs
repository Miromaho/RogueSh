using RogueSharp;
using RoguelikeCL.Core;
using RoguelikeCL.Enemies;
using RogueMain.Core;

namespace RoguelikeCL.System
{
   public static class ActorGen
   {
      private static Player player = null;

      public static Enemy CreateEnemy( int level, Point location )
      {
         Pool<Enemy> enemyPool = new Pool<Enemy>();
         enemyPool.Add( Gnome.Create( level ), 25 );
         enemyPool.Add( Goblin.Create( level ), 50 );

         Enemy enemy = enemyPool.Get();
         enemy.X = location.X;
         enemy.Y = location.Y;

         return enemy;
      }


      public static Player CreatePlayer()
      {
         if ( player == null )
         {
               player = new Player {
               Attack = 5,
               AttackChance = 50,
               Awareness = 15,
               Color = Colors.Player,
               Defense = 2,
               DefenseChance = 20,
               Gold = 0,
               Health = 24,
               MaxHealth = 24,
               Name = "Ivan",
               Speed = 10,
               Symbol = '@'
            };
         }

         return player;
      }
   }
}