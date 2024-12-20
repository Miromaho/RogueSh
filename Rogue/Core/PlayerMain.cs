using RLNET;
using RoguelikeCL.Abilities;
using RoguelikeCl.Items;
using RoguelikeCL.interfaces;
using RogueMain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoguelikeCL.Equipment;

namespace RoguelikeCL.Core
{
    public class Player : Actor
    {
        public IAbility QAbility { get; set; }
        public IAbility WAbility { get; set; }
        public IAbility EAbility { get; set; }
        public IAbility RAbility { get; set; }
        public IAbility AAbility { get; set; }

        public Iitem Item1 { get; set; }
        public Iitem Item2 { get; set; }
        public Iitem Item3 { get; set; }
        public Iitem Item4 { get; set; }
        public Iitem Item5 { get; set; }
        public Player()
        {
            QAbility = new DoNothing();
            WAbility = new DoNothing();
            EAbility = new DoNothing();
            RAbility = new DoNothing();
            AAbility = new DoNothing();
            Item1 = new NoItem();
            Item2 = new NoItem();
            Item3 = new NoItem();
            Item4 = new NoItem();
            Item5 = new NoItem();
        }

        public bool AddAbility(IAbility ability)
        {
            if (QAbility is DoNothing)
            {
                QAbility = ability;
            }
            else if (WAbility is DoNothing)
            {
                WAbility = ability;
            }
            else if (EAbility is DoNothing)
            {
                EAbility = ability;
            }
            else if (RAbility is DoNothing)
            {
                RAbility = ability;
            }
            else if (AAbility is DoNothing)
            {
                AAbility = ability;
            }
            else
            {
                return false;
            }

            return true;
        }

        public bool AddItem(Iitem item)
        {
            if (Item1 is NoItem)
            {
                Item1 = item;
            }
            else if (Item2 is NoItem)
            {
                Item2 = item;
            }
            else if (Item3 is NoItem)
            {
                Item3 = item;
            }
            else if (Item4 is NoItem)
            {
                Item4 = item;
            }
            else if (Item5 is NoItem)
            {
                Item5 = item;
            }
            else
            {
                return false;
            }

            return true;
        }

        public void DrawStats(RLConsole statConsole)
        {
            statConsole.Print(1, 1, $"Name:    {Name}", Colors.Text);
            statConsole.Print(1, 3, $"Health:  {Health}/{MaxHealth}", Colors.Text);
            statConsole.Print(1, 5, $"Attack:  {Attack} ({AttackChance}%)", Colors.Text);
            statConsole.Print(1, 7, $"Defense: {Defense} ({DefenseChance}%)", Colors.Text);
            statConsole.Print(1, 9, $"Gold:    {Gold}", Colors.Gold);
        }
        public void DrawInventory(RLConsole inventoryConsole)
        {
            inventoryConsole.Print(1, 1, "Equipment", Colors.Text);
            inventoryConsole.Print(1, 3, $"Head: {Head.Name}", Head == HeadEquipment.None() ? Palette.DbOldStone : Palette.DbLight);
            inventoryConsole.Print(1, 5, $"Body: {Body.Name}", Body == BodyEquipment.None() ? Palette.DbOldStone : Palette.DbLight);
            inventoryConsole.Print(1, 7, $"Hand: {Hand.Name}", Hand == HandEquipment.None() ? Palette.DbOldStone : Palette.DbLight);
            inventoryConsole.Print(1, 9, $"Feet: {Feet.Name}", Feet == FeetEquipment.None() ? Palette.DbOldStone : Palette.DbLight);

            inventoryConsole.Print(28, 1, "Abilities", Colors.Text);
            DrawAbility(QAbility, inventoryConsole, 0);
            DrawAbility(WAbility, inventoryConsole, 1);
            DrawAbility(EAbility, inventoryConsole, 2);
            DrawAbility(RAbility, inventoryConsole, 3);
            DrawAbility(AAbility, inventoryConsole, 4);

            inventoryConsole.Print(55, 1, "Items", Colors.Text);
            DrawItem(Item1, inventoryConsole, 0);
            DrawItem(Item2, inventoryConsole, 1);
            DrawItem(Item3, inventoryConsole, 2);
            DrawItem(Item4, inventoryConsole, 3);
            DrawItem(Item5, inventoryConsole, 4);
        }
        private void DrawAbility(IAbility ability, RLConsole inventoryConsole, int position)
        {
            char letter = 'Q';
            if (position == 0)
            {
                letter = 'Q';
            }
            else if (position == 1)
            {
                letter = 'W';
            }
            else if (position == 2)
            {
                letter = 'E';
            }
            else if (position == 3)
            {
                letter = 'R';
            }
            else if (position == 4)
            {
                letter = 'A';
            }

            RLColor highlightTextColor = Palette.DbOldStone;
            if (!(ability is DoNothing))
            {
                if (ability.TurnsUntilRefreshed == 0)
                {
                    highlightTextColor = Palette.DbLight;
                }
                else
                {
                    highlightTextColor = Palette.DbSkin;
                }
            }

            int xPosition = 28;
            int xHighlightPosition = 28 + 4;
            int yPosition = 3 + (position * 2);
            inventoryConsole.Print(xPosition, yPosition, $"{letter} - {ability.Name}", highlightTextColor);

            if (ability.TurnsToRefresh > 0)
            {
                int width = Convert.ToInt32(((double)ability.TurnsUntilRefreshed / (double)ability.TurnsToRefresh) * 16.0);
                int remainingWidth = 20 - width;
                inventoryConsole.SetBackColor(xHighlightPosition, yPosition, width, 1, Palette.DbOldBlood);
                inventoryConsole.SetBackColor(xHighlightPosition + width, yPosition, remainingWidth, 1, RLColor.Black);
            }
        }

        private void DrawItem(Iitem item, RLConsole inventoryConsole, int position)
        {
            int xPosition = 55;
            int yPosition = 3 + (position * 2);
            string place = (position + 1).ToString();
            inventoryConsole.Print(xPosition, yPosition, $"{place} - {item.Name}", item is NoItem ? Palette.DbOldStone : Palette.DbLight);
        }

        public void Tick()
        {
            QAbility?.Tick();
            WAbility?.Tick();
            EAbility?.Tick();
            RAbility?.Tick();
        }
    }
}
