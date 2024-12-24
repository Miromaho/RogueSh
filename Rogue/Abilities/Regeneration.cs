using System;
using RoguelikeCL.Core;
using RogueMain;

namespace RoguelikeCL.Abilities
{
    public class Regeneration : Ability
    {
        private readonly int amountToHeal;

        public Regeneration(int AmountToHeal = 1, int cooldown = 50, string name = "Regeneration")
        {
            Name = name;
            TurnsToRefresh = cooldown;
            TurnsUntilRefreshed = 0;
            amountToHeal = AmountToHeal;
            Passive = true;
        }

        protected override bool PerformAbility()
        {
            Player player = RogueGame.Player;

            player.Health = Math.Min(player.MaxHealth, player.Health + amountToHeal);

            return true;
        }
    }
}
