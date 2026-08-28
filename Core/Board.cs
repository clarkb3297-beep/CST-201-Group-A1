using System.Text;
using BattleshipGame.Models;

namespace BattleshipGame.Core;

public sealed class Board
{
    public const int Size = 10;
    private readonly List<Ship> ships = [];

    public IReadOnlyList<Ship> Ships => ships;

    public HashSet<Coordinate> OccupiedCells =>
        ships.SelectMany(ship => ship.Cells).ToHashSet();

    public bool CanPlace(IEnumerable<Coordinate> proposedCells)
    {
        HashSet<Coordinate> cells = proposedCells.ToHashSet();
        return cells.Count > 0
            && cells.All(cell => cell.IsInBounds(Size))
            && !cells.Overlaps(OccupiedCells);
    }

    public bool TryPlaceShip(
        string shipName,
        IEnumerable<Coordinate> proposedCells,
        out string message)
    {
        HashSet<Coordinate> cells = proposedCells.ToHashSet();

        if (!cells.All(cell => cell.IsInBounds(Size)))
        {
            message = "The ship would extend outside the 10 x 10 board.";
            return false;
        }

        if (cells.Overlaps(OccupiedCells))
        {
            message = "The ship would overlap a ship already on the board.";
            return false;
        }

        ships.Add(new Ship(shipName, cells));
        message = $"{shipName} placed successfully.";
        return true;
    }

    public string Render(bool revealShips)
    {
        HashSet<Coordinate> occupied = OccupiedCells;
        StringBuilder output = new();
        output.Append("    ");

        for (int column = 1; column <= Size; column++)
        {
            output.Append($"{column,2} ");
        }

        output.AppendLine();
        for (int row = 0; row < Size; row++)
        {
            output.Append($"{(char)('A' + row)}   ");
            for (int column = 0; column < Size; column++)
            {
                char symbol = revealShips && occupied.Contains(new Coordinate(row, column))
                    ? 'S'
                    : '.';
                output.Append($"{symbol,2} ");
            }
            output.AppendLine();
        }

        return output.ToString();
    }
}
