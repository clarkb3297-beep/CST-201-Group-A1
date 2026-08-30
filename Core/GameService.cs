using BattleshipGame.Models;
using System.Text;

namespace BattleshipGame.Core;

public sealed class GameService
{
    private readonly HashSet<Coordinate> playerShots = [];
    private readonly HashSet<Coordinate> computerShots = [];
    private readonly Random random = new();

    public IReadOnlySet<Coordinate> PlayerShots => playerShots;
    public IReadOnlySet<Coordinate> ComputerShots => computerShots;

    public bool TryPlayerShot(
        Board computerBoard,
        Coordinate target,
        out bool isHit,
        out string message)
    {
        isHit = false;

        if (!target.IsInBounds(Board.Size))
        {
            message = "That coordinate is outside the board.";
            return false;
        }

        if (!playerShots.Add(target))
        {
            message = "You already selected that cell. Choose another one.";
            return false;
        }

        Ship? hitShip = computerBoard.Ships
            .FirstOrDefault(ship => ship.Cells.Contains(target));

        if (hitShip is null)
        {
            message = $"{target}: Miss.";
            return true;
        }

        isHit = true;
        message = $"{target}: Hit!";
        return true;
    }

public Coordinate ComputerShot(
    Board playerBoard,
    out bool isHit,
    out string message)
{
    if (computerShots.Count >= Board.Size * Board.Size)
    {
        throw new InvalidOperationException("No unselected cells remain.");
    }

    Coordinate target;

    do
    {
        target = new Coordinate(
            random.Next(Board.Size),
            random.Next(Board.Size));
    }
    while (!computerShots.Add(target));

    Ship? hitShip = playerBoard.Ships
        .FirstOrDefault(ship => ship.Cells.Contains(target));

    if (hitShip is null)
    {
        isHit = false;
        message = $"{target}: Computer missed.";
        return target;
    }

    isHit = true;
    message = $"{target}: Computer hit!";
    return target;
}

public bool IsShipSunk(
    Ship ship,
    IReadOnlySet<Coordinate> shots)
{
    return ship.Cells.All(cell => shots.Contains(cell));
}

public bool HasPlayerWon(Board computerBoard)
{
    return computerBoard.Ships.Count > 0 &&
        computerBoard.Ships.All(
            ship => IsShipSunk(ship, playerShots));
}

public bool HasComputerWon(Board playerBoard)
{
    return playerBoard.Ships.Count > 0 &&
        playerBoard.Ships.All(
            ship => IsShipSunk(ship, computerShots));
}

public string RenderBoard(
    Board board,
    IReadOnlySet<Coordinate> shots,
    bool revealShips)
{
    HashSet<Coordinate> occupied = board.OccupiedCells;
    StringBuilder output = new();

    output.Append("    ");

    for (int column = 1; column <= Board.Size; column++)
    {
        output.Append($"{column,2} ");
    }

    output.AppendLine();

    for (int row = 0; row < Board.Size; row++)
    {
        output.Append($"{(char)('A' + row)}   ");

        for (int column = 0; column < Board.Size; column++)
        {
            Coordinate cell = new(row, column);
            char symbol;

            if (shots.Contains(cell))
            {
                symbol = occupied.Contains(cell) ? 'X' : 'O';
            }
            else if (revealShips && occupied.Contains(cell))
            {
                symbol = 'S';
            }
            else
            {
                symbol = '.';
            }

            output.Append($"{symbol,2} ");
        }

        output.AppendLine();
    }

    return output.ToString();
}
}
