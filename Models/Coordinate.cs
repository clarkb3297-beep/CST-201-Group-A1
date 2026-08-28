namespace BattleshipGame.Models;

public readonly record struct Coordinate(int Row, int Column)
{
    private const string RowLabels = "ABCDEFGHIJ";

    public bool IsInBounds(int boardSize = 10) =>
        Row >= 0 && Row < boardSize && Column >= 0 && Column < boardSize;

    public override string ToString() => $"{RowLabels[Row]}{Column + 1}";

    public static bool TryParse(string? input, out Coordinate coordinate)
    {
        coordinate = default;
        string cleaned = (input ?? string.Empty).Trim().ToUpperInvariant().Replace(" ", "");

        if (cleaned.Length < 2 || !RowLabels.Contains(cleaned[0]))
        {
            return false;
        }

        if (!int.TryParse(cleaned[1..], out int displayedColumn))
        {
            return false;
        }

        coordinate = new Coordinate(RowLabels.IndexOf(cleaned[0]), displayedColumn - 1);
        return coordinate.IsInBounds();
    }
}
