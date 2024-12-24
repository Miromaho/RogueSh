using RoguelikeCL.Abilities;
using RoguelikeCL.Core;

namespace RoguelikeCL.System
{
   public static class AbilityGen
   {
      public static Pool<Ability> abilityPool = null;
      public static Ability CreateAbility()
      {
         // сюда добавлять заклинания и абилки
         if ( abilityPool == null )
         {
            abilityPool = new Pool<Ability>();
            abilityPool.Add( new Heal( 10 ), 10 );
            abilityPool.Add( new MagicMissile( 2, 80, 150 ), 10 );
         }
         return abilityPool.Get();
      }
   }
}
