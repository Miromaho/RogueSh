﻿using RLNET;
using RogueMain.Core;
using RogueSharp;
using RoguelikeCL.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoguelikeCL.Abilities;
using RoguelikeCL.Equipment;

namespace RoguelikeCL.Core
{
    public class Actor : IActor, IDrawable, IScheduleable
    {
        public Actor()
        {
            Head = HeadEquipment.None();
            Body = BodyEquipment.None();
            Hand = HandEquipment.None();
            Feet = FeetEquipment.None();
        }
        //IActor переменные
        public HeadEquipment Head { get; set; }
        public BodyEquipment Body { get; set; }
        public HandEquipment Hand { get; set; }
        public FeetEquipment Feet { get; set; }

        private int _attack;
        private int _attackChance;
        private int _awareness;
        private int _defense;
        private int _defenseChance;
        private int _gold;
        private int _health;
        private int _maxHealth;
        private string _name;
        private int _speed;

        public int Attack{ get { return _attack; } set { _attack = value; }}     
        public int AttackChance{ get { return _attackChance; }set { _attackChance = value; }}
        public int Awareness{ get { return _awareness; }set { _awareness = value; }}
        public int Defense{get { return _defense; }set { _defense = value; }}
        public int DefenseChance{get { return _defenseChance; }set { _defenseChance = value; }}
        public int Gold{get { return _gold; }set { _gold = value; }}
        public int Health{get { return _health; }set { _health = value; }}
        public int MaxHealth{get { return _maxHealth; }set { _maxHealth = value; }}
        public string Name{get { return _name; }set { _name = value; }}
        public int Speed{get { return _speed; }set { _speed = value; }}

        //IDrawable
        public RLColor Color { get; set; }
       public char Symbol { get; set; }
       public int X { get; set; }
       public int Y { get; set; }

       public void Draw(RLConsole console, IMap map)
       {
            if (!map.GetCell(X, Y).IsExplored)
            {
                return;
            }

            if (map.IsInFov(X, Y))
            {
                console.Set(X, Y, Color, Colors.FloorBackgroundFov, Symbol);
            }
            else
            {
                console.Set(X, Y, Colors.Floor, Colors.FloorBackground, '.');
            }
       }
        //IScheduleable
        public int Time
        {
            get { return Speed; }
        }
    }
}

