﻿namespace RoguelikeCL.interfaces
{
   public interface IAbility
   {
      string Name { get; }
      int TurnsToRefresh { get; }
      int TurnsUntilRefreshed { get; }

      bool Perform();
      void Tick();
   }
}
