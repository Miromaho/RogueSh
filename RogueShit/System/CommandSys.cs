using OpenTK.Input;
using RogueMain;
using RogueSharp;
using RogueSharp.DiceNotation;
using RogueShit.Core;
using RogueShit.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RogueShit.System
{
    public class CommandSys
    {
        public bool IsPlayerTurn { get; set; }
        public static bool PlayersMove(Direction direction)
        {
            int x = RogueGame.Player.X;
            int y = RogueGame.Player.Y;
            switch (direction)
            {
                case Direction.Up:
                    {
                        y = RogueGame.Player.Y - 1;
                        break;
                    }
                case Direction.Down:
                    {
                        y = RogueGame.Player.Y + 1;
                        break;
                    }
                case Direction.Left:
                    {
                        x = RogueGame.Player.X - 1;
                        break;
                    }
                case Direction.Right:
                    {
                        x = RogueGame.Player.X + 1;
                        break;
                    }
                default:
                    {
                        return false;
                    }
            }

            if (RogueGame.DungeonMap.SetActorPosition(RogueGame.Player, x, y))
            {
                return true;
            }
            Enemy enemy = RogueGame.DungeonMap.GetEnemyAt(x, y);
            if (enemy != null)
            {
                Attack(RogueGame.Player, enemy);
                return true;
            }
            return false;
        }
        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }
        public void AddPlayerToTurnOrder()
        {
            //IScheduleable turnOrder = RogueGame.turnOrder.Get();
            IsPlayerTurn = true;
            RogueGame.turnOrder.Add(RogueGame.Player);


        }
        public void AddEnemiesToTurnOrder()
        {

            IScheduleable turnOrder = RogueGame.turnOrder.Get();

            Enemy enemy = turnOrder as Enemy;
            if (enemy != null)
            {
                enemy.PerformAction(this);
                RogueGame.turnOrder.Add(enemy);
            }
            else
            {
                AddEnemiesToTurnOrder();
            }
        }
        public void MoveEnemies(Enemy enemy, ICell cell)
        {
            if (!RogueGame.DungeonMap.SetActorPosition(enemy, cell.X, cell.Y))
            {
                if (RogueGame.Player.X == cell.X && RogueGame.Player.Y == cell.Y)
                {
                    Attack(enemy, RogueGame.Player);
                }
            }
        }
        public static void Attack(Actor attacker, Actor defender)
        {
            StringBuilder attackMessage = new StringBuilder();
            StringBuilder defenseMessage = new StringBuilder();

            int hits = ResolveAttack(attacker, defender, attackMessage);

            int blocks = ResolveDefense(defender, hits, attackMessage, defenseMessage);

            RogueGame.MessLogs.AddLine(attackMessage.ToString());
            if (!string.IsNullOrWhiteSpace(defenseMessage.ToString()))
            {
                RogueGame.MessLogs.AddLine(defenseMessage.ToString());
            }

            int damage = hits - blocks;

            ResolveDamage(defender, damage);
        }
        // Атакующий бросает кости и потом основываясь на своих статах, узнает получил ли он хоть одно попадание
        private static int ResolveAttack(Actor attacker, Actor defender, StringBuilder attackMessage)
        {
            int hits = 0;

            attackMessage.AppendFormat("{0} attacks {1} and rolls: ", attacker.Name, defender.Name);

            // Бросьте 20 - гранный кубик, равный значению атаки атакующего игрока
            DiceExpression attackDice = new DiceExpression().Dice(attacker.Attack, 20);
            DiceResult attackResult = attackDice.Roll();

            // Номинал каждого брошенного кубика
            foreach (TermResult termResult in attackResult.Results)
            {
                attackMessage.Append(termResult.Value + ", ");

                if (termResult.Value >= 20 - attacker.AttackChance)
                {
                    hits++;
                }
            }
            return hits;
        }
        private static int ResolveDefense(Actor defender, int hits, StringBuilder attackMessage, StringBuilder defenseMessage)
        {
            int blocks = 0;

            if (hits > 0)
            {
                attackMessage.AppendFormat("scoring {0} hits.", hits);
                defenseMessage.AppendFormat("  {0} defends and rolls: ", defender.Name);

                DiceExpression defenceDice = new DiceExpression().Dice(defender.Defense, 20);
                DiceResult defenseRoll = defenceDice.Roll();

                foreach (TermResult termResult in defenseRoll.Results)
                {
                    defenseMessage.Append(termResult.Value + ", ");
                    if (termResult.Value >= 20 - defender.DefenseChance)
                    {
                        blocks++;
                    }
                }
                defenseMessage.AppendFormat("resulting in {0} blocks.", blocks);
            }
            else
            {
                attackMessage.Append("Miss!");
            }
            return blocks;
        }

        private static void ResolveDamage(Actor defender, int damage)
        {
            if (damage > 0)
            {
                defender.Health = defender.Health - damage;

                RogueGame.MessLogs.AddLine($"  {defender.Name} was hit for {damage} damage");

                if (defender.Health <= 0)
                {
                    ResolveDeath(defender);
                }
            }
            else
            {
                RogueGame.MessLogs.AddLine($"  {defender.Name} blocked all damage");
            }
        }
        private static void ResolveDeath(Actor defender)
        {
            if (defender is Player)
            {
                RogueGame.MessLogs.AddLine($"  {defender.Name} was killed GIT GUD!.");
            }
            else if (defender is Enemy)
            {
                RogueGame.DungeonMap.RemoveEnemy((Enemy)defender);

                RogueGame.MessLogs.AddLine($"  {defender.Name} died and dropped {defender.Gold} gold");
            }
        }
    }
}
