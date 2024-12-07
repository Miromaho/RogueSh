using RLNET;
using RogueMain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RogueShit.Core
{
    public class Player : Actor
    {
        public Player()
        {
            Attack = 5;
            AttackChance = 50;
            Defense = 5;
            DefenseChance = 20;
            Gold = 0;
            Health = 24;
            MaxHealth = 24;
            Awareness = 15;
            Speed = 10;
            Name = "Ivan";
            Color = Colors.Player;
            Symbol = '@';
            X = 12;
            Y = 10;
        }
        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Name:    {Name}", Colors.Text);
            statConsole.Print(1, 3, $"Health:  {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 5, $"Attack:  {Attack} ({AttackChance}%)", Colors.Text);
            statConsole.Print(1, 7, $"Defense: {Defense} ({DefenseChance}%)", Colors.Text);
            statConsole.Print(1, 9, $"Gold:    {Gold}", Colors.Gold);
        }
    }
}
