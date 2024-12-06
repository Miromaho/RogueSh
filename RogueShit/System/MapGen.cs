using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public MapGenerator(int Width, int Height, int MaxRooms, int RoomMaxSize, int RoomMinSize)
    {
        width = Width;
        height = Height;
        maxRooms = MaxRooms;
        roomMaxSize = RoomMaxSize;
        roomMinSize = RoomMinSize;
        map = new DungeonMap();
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
        }
    }


    public DungeonMap CreateMap()
    {
        map.Initialize(width, height);

        for (int r = maxRooms; r > 0; r--)
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
        foreach (Rectangle room in map.Rooms)
        {
            CreateRoom(room);
        }
        for (int r = 1; r < map.Rooms.Count; r++)
        {
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
        PlacePlayer();
        PlaceEnemies();
        return map;
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
}
