# Integration Handoff for Angelo Ellis

## Completed by Brandon Clark

The following public components are ready:

- `Coordinate.TryParse(...)` validates A1-J10 input.
- `ShipPatterns.All` contains every assignment-required ship and orientation.
- `FleetPlacementService.ApplyPattern(...)` performs brute-force shape matching.
- `FleetPlacementService.TryPlaceFromPattern(...)` validates and places a chosen ship.
- `FleetPlacementService.RandomlyPlaceFleet(...)` places all computer ships legally.
- `Board.Ships` exposes the placed ships for hit detection.
- `Board.OccupiedCells` exposes occupied cells without changing the board.

## Suggested Angelo Integration

Add these responsibilities without changing the placement APIs:

1. Track shots separately for the player and computer.
2. Reject coordinates outside A1-J10 and previously selected targets.
3. Compare each target with `Board.Ships` to report hit or miss.
4. Keep the same attacker active after a hit.
5. Detect a sunk ship after every occupied cell has been hit.
6. Detect victory after all three opposing ships are sunk.
7. Render hits and misses while hiding unhit computer ships.

Before merging, run:

```powershell
dotnet run -- --self-test
```
