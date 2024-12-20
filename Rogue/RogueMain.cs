using RLNET;
using System.Drawing;
using RogueMain.Core;
using RoguelikeCL.Core;
using RoguelikeCL.System;
using RogueSharp.Random;
using RoguelikeCL.Items;
using RoguelikeCl.Items;

namespace RogueMain
{
    // размер окон консолей
    public class RogueGame
    {

        private static readonly int screenWidth = 100;
        private static readonly int screenHeight = 100;
        private static RLRootConsole rootConsole;

        private static readonly int mapWidth = 80;
        private static readonly int mapHeight = 50;
        private static RLConsole mapConsole;

        private static readonly int messageWidth = 80; 
        private static readonly int messageHeight = 11;
        private static RLConsole messageConsole;

        private static readonly int statWidth = 20;
        private static readonly int statHeight = 70;
        private static RLConsole statConsole;

        private static readonly int inventoryWidth = 80;
        private static readonly int inventoryHeight = 13;
        private static RLConsole inventoryConsole;

        private static int mapLevel = 1;
        private static bool renderRequired = true;

        public static DungeonMap DungeonMap { get; set; }
        public static Player Player { get; set; }
        public static CommandSys CommandSys { get; set; }
        public static MessLogs MessLogs { get; set; }
        public static SchedulingSystem turnOrder { get; set; }
        public static TargetingSys TargetingSys { get; private set; }
        public static IRandom Random { get; set; }
        
        public static void Main()
        {
            int seed = (int)DateTime.UtcNow.Ticks;
            Random = new DotNetRandom(seed);

            string fontFileName = "ASCII8x8.png";

            turnOrder = new SchedulingSystem();

            string consoleTitle = $"RoguelikeCL - Level {mapLevel} - seed {seed}";

            MessLogs = new MessLogs();
            MessLogs.AddLine("The Ivan arrives on level 1");
            MessLogs.AddLine($"Level seed: {seed}");


            MapGenerator mapGenerator = new MapGenerator(mapWidth, mapHeight, 12, 15, 6, mapLevel);
            DungeonMap = mapGenerator.CreateMap();

            rootConsole = new RLRootConsole(fontFileName, screenWidth, screenHeight, 8, 8, 1f, consoleTitle);
            mapConsole = new RLConsole(mapWidth, mapHeight);
            messageConsole = new RLConsole(messageWidth, messageHeight);
            statConsole = new RLConsole(statWidth, statHeight);
            inventoryConsole = new RLConsole(inventoryWidth, inventoryHeight);

            CommandSys = new CommandSys();
            TargetingSys = new TargetingSys();

            Player.Item1 = new TeleportScroll();
            rootConsole.Update += OnRootConsoleUpdate;

            rootConsole.Render += OnRootConsoleRender;

            //Цвета консолей статов, сообщений и прочего для того чтобы точно видеть границу.
            //inventoryConsole.SetBackColor(0, 0, inventoryWidth, inventoryHeight, Palette.DbMetal);
            //inventoryConsole.Print(1, 1, "Inventory", Colors.TextHeading);

            rootConsole.Run();
        }

        //Цикл хода. Вещи идут в этом порядке.
        private static void OnRootConsoleUpdate(object sender, UpdateEventArgs e)
        {
            bool didPlayerAct = false;
            RogueGame.Player.Tick();
            RLKeyPress keyPress = rootConsole.Keyboard.GetKeyPress();
            if (TargetingSys.IsPlayerTargeting)
            {
                if (keyPress != null)
                {
                    renderRequired = true;
                    TargetingSys.HandleKey(keyPress.Key);
                }
            }
            if (CommandSys.IsPlayerTurn)
                {
                    didPlayerAct = PlayerAction(didPlayerAct, keyPress);
                }

                if (didPlayerAct)
                {
                    renderRequired = true;
                    CommandSys.EndPlayerTurn();
                }

                else
                {
                    CommandSys.AddPlayerToTurnOrder();
                    renderRequired = true;
                }

            if (Player.Health <= 0)
            {
                Console.WriteLine($"You lose");
                rootConsole.Close();
            }
        }

        // Возможные действия
        private static bool PlayerAction(bool didPlayerAct, RLKeyPress keyPress)
        {
            if (keyPress != null)
            {
                if (keyPress.Key == RLKey.Up)
                {
                    didPlayerAct = CommandSys.PlayersMove(Direction.Up);
                }
                else if (keyPress.Key == RLKey.Down)
                {
                    didPlayerAct = CommandSys.PlayersMove(Direction.Down);
                }
                else if (keyPress.Key == RLKey.Left)
                {
                    didPlayerAct = CommandSys.PlayersMove(Direction.Left);
                }
                else if (keyPress.Key == RLKey.Right)
                {
                    didPlayerAct = CommandSys.PlayersMove(Direction.Right);
                }
                else if (keyPress.Key == RLKey.Escape)
                {
                    rootConsole.Close();
                }
                else if (keyPress.Key == RLKey.Insert)
                {
                    if (DungeonMap.CanMoveDownToNextLevel())
                    {
                        MapGenerator mapGenerator = new MapGenerator(mapWidth, mapHeight, 20, 13, 7, ++mapLevel);
                        DungeonMap = mapGenerator.CreateMap();
                        MessLogs = new MessLogs();
                        CommandSys = new CommandSys();
                        rootConsole.Title = $"RogueShit - Level {mapLevel}";
                        didPlayerAct = true;
                    }
                }
                else
                {
                    didPlayerAct = CommandSys.HandleKey(keyPress.Key);
                }
            }
            return didPlayerAct;
        }
        //Рендер в консоли
        private static void OnRootConsoleRender(object sender, UpdateEventArgs e)
        {
            if (renderRequired)
            {
                mapConsole.Clear();
                statConsole.Clear();
                messageConsole.Clear();
                inventoryConsole.Clear();

                DungeonMap.Draw(mapConsole, statConsole, inventoryConsole);

                MessLogs.Draw(messageConsole);
                TargetingSys.Draw(mapConsole);

                RLConsole.Blit(mapConsole, 0, 0, mapWidth, mapHeight, rootConsole, 0, inventoryHeight);
                RLConsole.Blit(statConsole, 0, 0, statWidth, statHeight, rootConsole, mapWidth, 0);
                RLConsole.Blit(messageConsole, 0, 0, messageWidth, messageHeight, rootConsole, 0, screenHeight - messageHeight);
                RLConsole.Blit(inventoryConsole, 0, 0, inventoryWidth, inventoryHeight, rootConsole, 0, 0);

                rootConsole.Draw();

                renderRequired = false;
            }
        }
    }
}
