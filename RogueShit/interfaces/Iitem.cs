namespace RoguelikeCL.interfaces
{
   public interface Iitem
   {
      string Name { get; }
      int RemainingUses { get; }

      bool Use();
   }
}
