using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using RLNET;
using RogueMain;
using RogueMain.Core;
using RogueSharp;
using RogueShit.Core;

public class DungeonMap : Map
{
    public List<Rectangle> Rooms;
    public DungeonMap() 
    {
        Rooms = new List<Rectangle>();
    }
    // Метод Draw будет вызываться каждый раз при обновлении карты
    // Он будет выводить все символы/цвета для каждой ячейки в консоль карты.
    public void Draw(RLConsole mapConsole)
    {
        mapConsole.Clear();
        foreach (Cell cell in GetAllCells())
        {
            SetConsoleSymbolForCell(mapConsole, cell);
        }
    }
    public void AddPlayer(Player player)
    {
        RogueGame.Player = player;
        SetIsWalkable(player.X, player.Y, false);
        UpdatePlayerFieldOfView();
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



