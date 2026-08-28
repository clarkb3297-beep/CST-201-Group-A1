using BattleshipGame.Models;

namespace BattleshipGame.Core;

public static class ShipPatterns
{
    public static readonly IReadOnlyDictionary<string,
        IReadOnlyDictionary<string, Coordinate[]>> All =
        new Dictionary<string, IReadOnlyDictionary<string, Coordinate[]>>
        {
            ["Destroyer"] = new Dictionary<string, Coordinate[]>
            {
                ["square"] =
                [
                    new(0, 0), new(0, 1),
                    new(1, 0), new(1, 1)
                ]
            },
            ["Submarine"] = new Dictionary<string, Coordinate[]>
            {
                ["down-right"] = [new(0, 0), new(1, 1), new(2, 2)],
                ["down-left"] = [new(0, 0), new(1, -1), new(2, -2)]
            },
            ["Cruiser"] = new Dictionary<string, Coordinate[]>
            {
                ["horizontal"] = [new(0, 0), new(0, 1), new(0, 2)],
                ["vertical"] = [new(0, 0), new(1, 0), new(2, 0)]
            }
        };
}
