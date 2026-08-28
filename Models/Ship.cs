namespace BattleshipGame.Models;

public sealed class Ship
{
    public string Name { get; }
    public HashSet<Coordinate> Cells { get; }

    public Ship(string name, IEnumerable<Coordinate> cells)
    {
        Name = name;
        Cells = new HashSet<Coordinate>(cells);
    }
}
