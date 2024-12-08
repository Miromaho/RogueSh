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
    private readonly List<Enemy> enemies;

    public List<Rectangle> Rooms { get; set; }
    public List<Door> Doors { get; set; }
    public Stairs StairsUp { get; set; }
    public Stairs StairsDown { get; set; }

    public DungeonMap()
    {
        RogueGame.turnOrder.Clear();

        enemies = new List<Enemy>();
        Rooms = new List<Rectangle>();
        Doors = new List<Door>();
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

 
    public bool SetActorPosition(Actor actor, int x, int y)
    {

        if (GetCell(x, y).IsWalkable)
        {

            SetIsWalkable(actor.X, actor.Y, true);

            actor.X = x;
            actor.Y = y;

            SetIsWalkable(actor.X, actor.Y, false);

            OpenDoor(actor, x, y);
            if (actor is Player)
            {
                UpdatePlayerFieldOfView();
            }
            return true;
        }
        return false;
    }

    public Door GetDoor(int x, int y)
    {
        return Doors.SingleOrDefault(d => d.X == x && d.Y == y);
    }

    private void OpenDoor(Actor actor, int x, int y)
    {
        Door door = GetDoor(x, y);
        if (door != null && !door.IsOpen)
        {
            door.IsOpen = true;
            var cell = GetCell(x, y);

            SetCellProperties(x, y, true, cell.IsWalkable, cell.IsExplored);

            RogueGame.MessLogs.AddLine($"{actor.Name} opened a door");
        }
    }

    public void AddPlayer(Player player)
    {
        RogueGame.Player = player;
        SetIsWalkable(player.X, player.Y, false);
        UpdatePlayerFieldOfView();
        RogueGame.turnOrder.Add(player);
    }

    public void AddEnemy(Enemy enemy)
    {
        enemies.Add(enemy);

        SetIsWalkable(enemy.X, enemy.Y, false);
        RogueGame.turnOrder.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);

        SetIsWalkable(enemy.X, enemy.Y, true);
        RogueGame.turnOrder.Remove(enemy);
    }

    public Enemy GetEnemyAt(int x, int y)
    {
        return enemies.FirstOrDefault(m => m.X == x && m.Y == y);
    }
    public bool CanMoveDownToNextLevel()
    {
        Player player = RogueGame.Player;
        return StairsDown.X == player.X && StairsDown.Y == player.Y;
    }
    public void SetIsWalkable(int x, int y, bool isWalkable)
    {
        Cell cell = (Cell)GetCell(x, y);
        SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
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

    public void Draw(RLConsole mapConsole, RLConsole statConsole)
    {

        foreach (Cell cell in GetAllCells())
        {
            SetConsoleSymbolForCell(mapConsole, cell);
        }

        foreach (Door door in Doors)
        {
            door.Draw(mapConsole, this);
        }

        StairsUp.Draw(mapConsole, this);
        StairsDown.Draw(mapConsole, this);

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

    private void SetConsoleSymbolForCell(RLConsole console, Cell cell)
    {

        if (!cell.IsExplored)
        {
            return;
        }

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
}