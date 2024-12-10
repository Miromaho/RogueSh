﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoRogue.GameFramework;
using RLNET;
using RogueMain;
using RogueSharp;
using RogueSharp.DiceNotation;
using RogueShit.Core;
using RogueShit.Enemies;

public class MapGenerator
{
    private readonly int width;
    private readonly int height;
    private readonly int maxRooms;
    private readonly int roomMaxSize;
    private readonly int roomMinSize;

    private readonly DungeonMap map;

    public MapGenerator(int Width, int Height, int MaxRooms, int RoomMaxSize, int RoomMinSize, int mapLevel)
    {
        width = Width;
        height = Height;
        maxRooms = MaxRooms;
        roomMaxSize = RoomMaxSize;
        roomMinSize = RoomMinSize;
        map = new DungeonMap();
    }
    public DungeonMap CreateMap()
    {
        map.Initialize(width, height);

        for (int r = 0; r < maxRooms; r++)
        {
            int roomWidth = RogueGame.Random.Next(roomMinSize, roomMaxSize);
            int roomHeight = RogueGame.Random.Next(roomMinSize, roomMaxSize);
            int roomXPosition = RogueGame.Random.Next(0, width - roomWidth - 1);
            int roomYPosition = RogueGame.Random.Next(0, height - roomHeight - 1);

            var newRoom = new Rectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

            bool newRoomIntersects = map.Rooms.Any(room => newRoom.Intersects(room));

            if (!newRoomIntersects)
            {
                map.Rooms.Add(newRoom);
            }
        }
        for (int r = 0; r < map.Rooms.Count; r++)
        {
            if (r == 0)
            {
                continue;
            }
            int previousRoomCenterX = map.Rooms[r - 1].Center.X;
            int previousRoomCenterY = map.Rooms[r - 1].Center.Y;
            int currentRoomCenterX = map.Rooms[r].Center.X;
            int currentRoomCenterY = map.Rooms[r].Center.Y;

            if (RogueGame.Random.Next(1, 2) == 1)
            {
                CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
            }
            else
            {
                CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
            }
        }

        foreach (Rectangle room in map.Rooms)
        {
            CreateRoom(room);
            CreateDoors(room);
        }

        CreateStairs();
        PlacePlayer();
        PlaceEnemies();
        return map;

    }

    private void CreateRoom(Rectangle room)
    {
        for (int x = room.Left + 1; x < room.Right; x++)
        {
            for (int y = room.Top + 1; y < room.Bottom; y++)
            {
                map.SetCellProperties(x, y, true, true, true);
            }
        }
    }
    private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
    {
        for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
        {
            map.SetCellProperties(x, yPosition, true, true);
        }
    }
    private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
    {
        for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
        {
            map.SetCellProperties(xPosition, y, true, true);
        }
    }
    private void CreateDoors(Rectangle room)
    {
        int xMin = room.Left;
        int xMax = room.Right;
        int yMin = room.Top;
        int yMax = room.Bottom;

        List<ICell> borderCells = map.GetCellsAlongLine(xMin, yMin, xMax, yMin).ToList();
        borderCells.AddRange(map.GetCellsAlongLine(xMin, yMin, xMin, yMax));
        borderCells.AddRange(map.GetCellsAlongLine(xMin, yMax, xMax, yMax));
        borderCells.AddRange(map.GetCellsAlongLine(xMax, yMin, xMax, yMax));

        foreach (Cell cell in borderCells)
        {
            if (IsPotentialDoor(cell))
            {

                map.SetCellProperties(cell.X, cell.Y, false, true);
                map.Doors.Add(new Door
                {
                    X = cell.X,
                    Y = cell.Y,
                    IsOpen = false
                });
            }
        }
    }

    private bool IsPotentialDoor(Cell cell)
    {

        if (!cell.IsWalkable)
        {
            return false;
        }

        Cell right = (Cell)map.GetCell(cell.X + 1, cell.Y);
        Cell left = (Cell)map.GetCell(cell.X - 1, cell.Y);
        Cell top = (Cell)map.GetCell(cell.X, cell.Y - 1);
        Cell bottom = (Cell)map.GetCell(cell.X, cell.Y + 1);

        if (map.GetDoor(cell.X, cell.Y) != null ||
            map.GetDoor(right.X, right.Y) != null ||
            map.GetDoor(left.X, left.Y) != null ||
            map.GetDoor(top.X, top.Y) != null ||
            map.GetDoor(bottom.X, bottom.Y) != null)
        {
            return false;
        }

        if (right.IsWalkable && left.IsWalkable && !top.IsWalkable && !bottom.IsWalkable)
        {
            return true;
        }

        if (!right.IsWalkable && !left.IsWalkable && top.IsWalkable && bottom.IsWalkable)
        {
            return true;
        }
        return false;
    }
    private void PlacePlayer()
    {
        Player player = RogueGame.Player;
        if (player == null)
        {
            player = new Player();
        }
        player.X = map.Rooms[0].Center.X;
        player.Y = map.Rooms[0].Center.Y;

        map.AddPlayer(player);
    }
    private void CreateStairs()
    {
        map.StairsUp = new Stairs
        {
            X = map.Rooms.First().Center.X + 1,
            Y = map.Rooms.First().Center.Y,
            IsUp = true
        };
        map.StairsDown = new Stairs
        {
            X = map.Rooms.Last().Center.X,
            Y = map.Rooms.Last().Center.Y,
            IsUp = false
        };
    }
    private void PlaceEnemies()
    {
        foreach (var room in map.Rooms)
        {
            if (Dice.Roll("1D10") < 8)
            {
                var numberOfEnemies = Dice.Roll("1D4");
                for (int i = 0; i < numberOfEnemies; i++)
                {
                   Point randomRoomLocation = map.GetRandomWalkableLocationInRoom(room);
                    if (randomRoomLocation != null)
                    {
                        var enemy = Goblin.Create(1);
                        enemy.X = randomRoomLocation.X;
                        enemy.Y = randomRoomLocation.Y;
                        map.AddEnemy(enemy);
                    }
                }
            }
            if (Dice.Roll("1D10") < 8)
            {
                var numberOfEnemies = Dice.Roll("1D4");
                for (int i = 0; i < numberOfEnemies; i++)
                {
                    Point randomRoomLocation = map.GetRandomWalkableLocationInRoom(room);
                    if (randomRoomLocation != null)
                    {
                        var enemy = Gnome.Create(1);
                        enemy.X = randomRoomLocation.X;
                        enemy.Y = randomRoomLocation.Y;
                        map.AddEnemy(enemy);
                    }
                }
            }
        }
    }
}
