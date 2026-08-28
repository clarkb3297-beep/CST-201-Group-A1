using BattleshipGame.Models;

namespace BattleshipGame.Core;

public sealed class FleetPlacementService
{
    private readonly Random random;

    public FleetPlacementService(Random? random = null)
    {
        this.random = random ?? new Random();
    }

    public static HashSet<Coordinate> ApplyPattern(
        Coordinate anchor,
        IEnumerable<Coordinate> offsets)
    {
        HashSet<Coordinate> cells = [];
        foreach (Coordinate offset in offsets)
        {
            cells.Add(new Coordinate(
                anchor.Row + offset.Row,
                anchor.Column + offset.Column));
        }
        return cells;
    }

    public bool TryPlaceFromPattern(
        Board board,
        string shipName,
        string orientation,
        Coordinate anchor,
        out string message)
    {
        if (!ShipPatterns.All.TryGetValue(shipName, out var orientations)
            || !orientations.TryGetValue(orientation, out Coordinate[]? offsets))
        {
            message = "Unknown ship name or orientation.";
            return false;
        }

        HashSet<Coordinate> cells = ApplyPattern(anchor, offsets);
        return board.TryPlaceShip(shipName, cells, out message);
    }

    public void RandomlyPlaceFleet(Board board)
    {
        foreach ((string shipName, var orientations) in ShipPatterns.All)
        {
            List<HashSet<Coordinate>> legalCandidates = [];

            for (int row = 0; row < Board.Size; row++)
            {
                for (int column = 0; column < Board.Size; column++)
                {
                    Coordinate anchor = new(row, column);
                    foreach (Coordinate[] offsets in orientations.Values)
                    {
                        HashSet<Coordinate> cells = ApplyPattern(anchor, offsets);
                        if (board.CanPlace(cells))
                        {
                            legalCandidates.Add(cells);
                        }
                    }
                }
            }

            if (legalCandidates.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No legal placement remains for {shipName}.");
            }

            HashSet<Coordinate> selected = legalCandidates[random.Next(legalCandidates.Count)];
            if (!board.TryPlaceShip(shipName, selected, out string message))
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
