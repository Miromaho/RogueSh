using RoguelikeCL.Core;
using RoguelikeCL.Items;
namespace RoguelikeCL.System
{
    public static class ItemGen
   {
      public static Item CreateItem()
      {
         Pool<Item> itemPool = new Pool<Item>();
// Сюда добавлять предметы которые генерируются на уровне
         itemPool.Add( new HealingPotion(), 20 );
         itemPool.Add( new TeleportScroll(), 20 );

         return itemPool.Get();
      }
   }
}
