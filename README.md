# Battleship CLC - Brandon's C# Component

## Brandon's Assigned Work

- 10 x 10 board structure
- Required ship-pattern definitions
- Brute-force pattern application
- Boundary and overlap validation
- Human ship-placement prompts
- Random legal computer fleet placement

Angelo can integrate the player/computer turn logic, hit-and-miss processing, repeated-shot prevention, and win conditions with the public `Board`, `Coordinate`, and `Ship` types.

## Run with .NET

```powershell
cd "CST-201-Group-A1"
dotnet run
```

Run the built-in placement tests:

```powershell
dotnet run -- --self-test
```

## Open in Eclipse

Eclipse does not provide native C# support. Install a compatible C#/.NET plug-in such as **aCute**, ensure the .NET SDK is configured, and then import or open this folder as the project location. The project is a standard SDK-style `.csproj` application.

If the Eclipse plug-in does not recognize the project correctly, use Eclipse as the editor and run the commands above in its integrated terminal. Visual Studio, Visual Studio Code with C# Dev Kit, or JetBrains Rider provide more complete C# support.

## Placement Input Examples

```text
Destroyer: A1, square
Submarine: C3, down-right
Cruiser: G1, horizontal
```

For the submarine's `down-left` orientation, the starting coordinate is the upper-right cell of the diagonal.
