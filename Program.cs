using BattleshipGame.Core;
using BattleshipGame.Models;

namespace BattleshipGame;

public static class Program
{
    public static void Main(string[] args)
    {
        if (args.Contains("--self-test"))
        {
            RunSelfTests();
            return;
        }

        Console.WriteLine("BATTLESHIP - BRANDON'S CLC COMPONENT");
        Console.WriteLine("Board structure, patterns, validation, and random placement");
        Console.WriteLine();

        Board playerBoard = new();
        FleetPlacementService placementService = new();
        PlacePlayerFleet(playerBoard, placementService);

        Board computerBoard = new();
        placementService.RandomlyPlaceFleet(computerBoard);

        Console.WriteLine("\nYour completed fleet:");
        Console.WriteLine(playerBoard.Render(revealShips: true));
        Console.WriteLine("The computer fleet was placed randomly and remains hidden:");
        Console.WriteLine(computerBoard.Render(revealShips: false));
        PlayGame(playerBoard, computerBoard);
    }

    private static void PlacePlayerFleet(
        Board board,
        FleetPlacementService placementService)
    {
        Console.WriteLine("Coordinates use rows A-J and columns 1-10.");
        Console.WriteLine("For down-left, choose the submarine's upper-right cell.\n");

        foreach ((string shipName, var orientations) in ShipPatterns.All)
        {
            bool placed = false;
            while (!placed)
            {
                Console.WriteLine(board.Render(revealShips: true));
                Console.WriteLine($"Place the {shipName}.");
                Console.WriteLine($"Orientations: {string.Join(", ", orientations.Keys)}");

                Console.Write("Starting coordinate: ");
                if (!Coordinate.TryParse(Console.ReadLine(), out Coordinate anchor))
                {
                    Console.WriteLine("Invalid coordinate. Use A1 through J10.\n");
                    continue;
                }

                Console.Write("Orientation: ");
                string orientation = (Console.ReadLine() ?? string.Empty)
                    .Trim()
                    .ToLowerInvariant();

                placed = placementService.TryPlaceFromPattern(
                    board,
                    shipName,
                    orientation,
                    anchor,
                    out string message);

                Console.WriteLine(message + "\n");
            }
        }
    }

    private static void PlayGame(
    Board playerBoard,
    Board computerBoard)
{
    GameService gameService = new();

    while (true)
    {
        Console.WriteLine("\nYOUR BOARD");
        Console.WriteLine(
            gameService.RenderBoard(
                playerBoard,
                gameService.ComputerShots,
                revealShips: true));

        Console.WriteLine("COMPUTER BOARD");
        Console.WriteLine(
            gameService.RenderBoard(
                computerBoard,
                gameService.PlayerShots,
                revealShips: false));

        bool playerTurn = true;

        while (playerTurn)
        {
            Console.Write("Choose a cell to attack (A1-J10): ");
            string? input = Console.ReadLine();

            if (!Coordinate.TryParse(input, out Coordinate target))
            {
                Console.WriteLine(
                    "Invalid coordinate. Use A1 through J10.");
                continue;
            }

            bool validShot = gameService.TryPlayerShot(
                computerBoard,
                target,
                out bool playerHit,
                out string playerMessage);

            Console.WriteLine(playerMessage);

            if (!validShot)
            {
                continue;
            }

            if (gameService.HasPlayerWon(computerBoard))
            {
                Console.WriteLine(
                    "\nYou sank all of the computer's ships. You win!");
                return;
            }

            playerTurn = playerHit;
        }

        bool computerTurn = true;

        while (computerTurn)
        {
            Coordinate computerTarget =
                gameService.ComputerShot(
                    playerBoard,
                    out bool computerHit,
                    out string computerMessage);

            Console.WriteLine(computerMessage);

            if (gameService.HasComputerWon(playerBoard))
            {
                Console.WriteLine(
                    "\nThe computer sank all of your ships. Computer wins.");
                return;
            }

            computerTurn = computerHit;
        }
    }
}

    private static void RunSelfTests()
    {
        int passed = 0;

        Assert(Coordinate.TryParse("A1", out Coordinate first)
            && first == new Coordinate(0, 0), "A1 parsing");
        passed++;

        Assert(Coordinate.TryParse("J10", out Coordinate last)
            && last == new Coordinate(9, 9), "J10 parsing");
        passed++;

        Assert(!Coordinate.TryParse("K1", out _), "Out-of-range coordinate rejection");
        passed++;

        Board board = new();
        FleetPlacementService service = new(new Random(17));
        Assert(service.TryPlaceFromPattern(
            board, "Destroyer", "square", new Coordinate(0, 0), out _),
            "Valid destroyer placement");
        passed++;

        Assert(!service.TryPlaceFromPattern(
            board, "Cruiser", "horizontal", new Coordinate(1, 1), out _),
            "Overlap rejection");
        passed++;

        Assert(!service.TryPlaceFromPattern(
            board, "Cruiser", "horizontal", new Coordinate(9, 9), out _),
            "Boundary rejection");
        passed++;

        Board computerBoard = new();
        service.RandomlyPlaceFleet(computerBoard);
        Assert(computerBoard.Ships.Count == 3, "Complete computer fleet");
        Assert(computerBoard.OccupiedCells.Count == 10, "No computer overlap");
        passed += 2;

        Console.WriteLine($"All {passed} placement tests passed.");
    }

    private static void Assert(bool condition, string testName)
    {
        if (!condition)
        {
            throw new InvalidOperationException($"Self-test failed: {testName}");
        }
    }
}
