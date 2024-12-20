using RoguelikeCL.Equipment;

namespace RoguelikeCL.System
{
   public class EquipmentGen
   {
      private readonly Pool<Core.Equipment> equipmentPool;

      public EquipmentGen( int level )
      {
         // Сюда добавлять рандомное снаряжение на уровень 
         equipmentPool = new Pool<Core.Equipment>();

         if ( level <= 3 )
         {
            equipmentPool.Add( BodyEquipment.Leather(), 20 );
            equipmentPool.Add( HeadEquipment.Leather(), 20 );
            equipmentPool.Add( FeetEquipment.Leather(), 20 );
            equipmentPool.Add( HandEquipment.Dagger(), 25 );
            equipmentPool.Add( HandEquipment.Sword(), 5 );
            equipmentPool.Add( HeadEquipment.Chain(), 5 );
            equipmentPool.Add( BodyEquipment.Chain(), 5 );
            equipmentPool.Add( BodyEquipment.Fur(), 5);
         }
         else if ( level <= 6 )
         {
            equipmentPool.Add(FeetEquipment.Fur(), 10);
            equipmentPool.Add( HandEquipment.Staff(), 13);
            equipmentPool.Add( BodyEquipment.Chain(), 20 );
            equipmentPool.Add( HeadEquipment.Chain(), 20 );
            equipmentPool.Add( FeetEquipment.Chain(), 20 );
            equipmentPool.Add( HandEquipment.Sword(), 15 );
            equipmentPool.Add( HandEquipment.Axe(), 15 );
            equipmentPool.Add( HeadEquipment.Plate(), 5 );
            equipmentPool.Add( BodyEquipment.Plate(), 5 );
         }
         else
         {
            equipmentPool.Add( BodyEquipment.Plate(), 25 );
            equipmentPool.Add( HeadEquipment.Plate(), 25 );
            equipmentPool.Add( FeetEquipment.Plate(), 25 );
            equipmentPool.Add( HandEquipment.TwoHandedSword(), 25 );
         }
      }

      public Core.Equipment CreateEquipment()
      { 
         return equipmentPool.Get();
      }
   }
}
