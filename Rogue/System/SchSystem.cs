using RoguelikeCL.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoguelikeCL.System
{
    public class SchedulingSystem
    {
        private int time;
        private readonly SortedDictionary<int, List<IScheduleable>> scheduleables;

        public SchedulingSystem()
        {
            time = 0;
            scheduleables = new SortedDictionary<int, List<IScheduleable>>();
        }

        // Добавление нового объекта в расписание 
        // помещает объект в текущее время плюс свойство объекта Time(время)
        public void Add(IScheduleable scheduleable)
        {
            int key = time + scheduleable.Time;
            if (!scheduleables.ContainsKey(key))
            {
                scheduleables.Add(key, new List<IScheduleable>());
            }
            scheduleables[key].Add(scheduleable);
        }

        // удаление определенного объекта из расписания
        // Пригодится когда враг убит чтобы удалить его до того, как он сделает свое действие
        public void Remove(IScheduleable scheduleable)
        {
            KeyValuePair<int, List<IScheduleable>> scheduleableListFound = new KeyValuePair<int, List<IScheduleable>>(-1, null);

            foreach (var scheduleablesList in scheduleables)
            {
                if (scheduleablesList.Value.Contains(scheduleable))
                {
                    scheduleableListFound = scheduleablesList;
                    break;
                }
            }
            if (scheduleableListFound.Value != null)
            {
                scheduleableListFound.Value.Remove(scheduleable);
                if (scheduleableListFound.Value.Count <= 0)
                {
                    scheduleables.Remove(scheduleableListFound.Key);
                }
            }
        }
        //ищет следующий объект,чья очередь наступила, по расписанию
        public IScheduleable Get()
        {
            var firstScheduleableGroup = scheduleables.First();
            var firstScheduleable = firstScheduleableGroup.Value.First();
            Remove(firstScheduleable);
            time = firstScheduleableGroup.Key;
            return firstScheduleable;
        }
        // текущее время для расписания
        public int GetTime()
        {
            return time;
        }
        // Сброс времени и очистка расписания
        public void Clear()
        {
            time = 0;
            scheduleables.Clear();
        }
    }
}
