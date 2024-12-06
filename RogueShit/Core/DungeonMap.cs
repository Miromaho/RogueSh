using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RLNET;
using RogueMain;
using RogueMain.Core;
using RogueSharp;
using RogueShit.Core;

public class DungeonMap : Map
{
    public List<Rectangle> Rooms;
    private readonly List<Enemy> enemies;

    public DungeonMap()
    {
        Rooms = new List<Rectangle>();
        enemies = new List<Enemy>();
    }
    // Метод Draw будет вызываться каждый раз при обновлении карты
    // Он будет выводить все символы/цвета для каждой ячейки в консоль карты.
    public void Draw(RLConsole mapConsole, RLConsole statConsole)
    {
        mapConsole.Clear();
        foreach (Cell cell in GetAllCells())
        {
            SetConsoleSymbolForCell(mapConsole, cell);
        }

        int i = 0;
        foreach (Enemy enemy in enemies)
        {
            enemy.Draw(mapConsole, this);
            if (IsInFov(enemy.X, enemy.Y))
            {
                enemy.DrawStats(statConsole, i);
                i++;
            }
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        SetIsWalkable(enemy.X, enemy.Y, true);
        RogueGame.SchedulingSystem.Remove(enemy);
    }
    public Enemy GetEnemyAt(int x, int y)
    {
        return enemies.FirstOrDefault(e => e.X == x && e.Y == y);
    }
public void AddPlayer(Player player)
    {
        RogueGame.Player = player;
        SetIsWalkable(player.X, player.Y, false);
        UpdatePlayerFieldOfView();
        RogueGame.SchedulingSystem.Add(player);
    }
    private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
    {
        if (!cell.IsExplored)
        {
            return;
        }

        //Если ячейка в поле зрения она будет нарисована в светлых тонах
        if (IsInFov(cell.X, cell.Y))
        {
            if (cell.IsWalkable)
            {
                console.Set(cell.X, cell.Y, Colors.FloorFov, Colors.FloorBackgroundFov, '.');
            }
            else
            {
                console.Set(cell.X, cell.Y, Colors.WallFov, Colors.WallBackgroundFov, '#');
            }
        }

        //Если ячейка вне поля зрения то она будет нарисована в темных тонах
        else
        {
            if (cell.IsWalkable)
            {
                console.Set(cell.X, cell.Y, Colors.Floor, Colors.FloorBackground, '.');
            }
            else
            {
                console.Set(cell.X, cell.Y, Colors.Wall, Colors.WallBackground, '#');
            }
        }
    }
    // Добавление врагов на карту
    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);
        SetIsWalkable(enemy.X, enemy.Y, false);
        RogueGame.SchedulingSystem.Add(enemy);
    }
    public Point GetRandomWalkableLocationInRoom(Rectangle room)
    {
        if (DoesRoomHaveWalkableSpace(room))
        {
            for (int i = 0; i < 100; i++)
            {
                int x = RogueGame.Random.Next(1, room.Width - 2) + room.X;
                int y = RogueGame.Random.Next(1, room.Height - 2) + room.Y;
                if (IsWalkable(x, y))
                {
                    return new Point(x, y);
                }
            }
        }
        return Point.Zero;
    }
    public bool DoesRoomHaveWalkableSpace(Rectangle room)
    {
        for (int x = 1; x <= room.Width - 2; x++)
        {
            for (int y = 1; y <= room.Height - 2; y++)
            {
                if (IsWalkable(x + room.X, y + room.Y))
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void UpdatePlayerFieldOfView()
    {
        Player player = RogueGame.Player;
        ComputeFov(player.X, player.Y, player.Awareness, true);

        foreach (Cell cell in GetAllCells())
        {
            if (IsInFov(cell.X, cell.Y))
            {
                SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
            }
        }
    }
     // Будет возвращать истину(true) если удалось переместить персонажа на ячейку в ином случае ложь(false)
    public bool SetActorPosition(Actor actor, int x, int y)
    {
        if (GetCell(x, y).IsWalkable)
        {
            SetIsWalkable(actor.X, actor.Y, true);
            actor.X = x;
            actor.Y = y;
            SetIsWalkable(actor.X, actor.Y, false);
            if (actor is Player)
            {
                UpdatePlayerFieldOfView();
            }
            return true;
        }
        return false;
    }
    public void SetIsWalkable(int x, int y, bool isWalkable)
    {
        Cell cell = (Cell)GetCell(x, y); // Возможно нужно будет потом исправить
        SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
    }
}



