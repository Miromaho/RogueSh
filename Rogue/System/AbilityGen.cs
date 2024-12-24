using RoguelikeCL.Abilities;
using RoguelikeCL.Core;
using RogueMain;

namespace RoguelikeCL.System
{
    public static class AbilityGen
    {
        public static Pool<Ability> abilityPool = null;
        public static Ability CreateAbility()
        {
            // сюда добавлять заклинания и абилки
            if (abilityPool == null)
            {
                abilityPool = new Pool<Ability>();
                abilityPool.Add(new Heal(10), 10);
                abilityPool.Add(new MagicMissile(1, 80, 500), 10);
                abilityPool.Add(new MagicMissile(20, 100, 2000, "Arcane Lance"), 5);
                abilityPool.Add(new Regeneration(1, 100), 10);
                abilityPool.Add(new Regeneration(1, 6, "Troll's blood"), 5);
            }
            return abilityPool.Get();
        }
    }
}
