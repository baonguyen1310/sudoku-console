# Sudoku App

A command-line Sudoku game written in C# (.NET 8).

## Environment

- **OS:** Windows, macOS, or Linux
- **Runtime:** [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

Verify your installation:

```
dotnet --version
```

You should see a version starting with `8.`.

---

## How to Build

From the repository root (`SudokuApp/`):

```
dotnet build
```

---

## How to Run

```
dotnet run --project src/Sudoku.ConsoleApp
```

The game will display a puzzle with 30 pre-filled numbers and prompt you for commands.

---

## How to Play

The grid uses **letters A–I** for rows (top to bottom) and **numbers 1–9** for columns (left to right). Empty cells are shown as `_`.

```
    1 2 3 4 5 6 7 8 9
  A 5 3 _ _ 7 _ _ _ _
  B 6 _ _ 1 9 5 _ _ _
  ...
```

### Commands

| Command       | Example    | Description                                      |
|---------------|------------|--------------------------------------------------|
| Place a number | `A3 4`    | Place the number 4 in row A, column 3            |
| Clear a cell  | `C5 clear` | Remove your entry from row C, column 5           |
| Get a hint    | `hint`     | Reveal one correct number                        |
| Check grid    | `check`    | Report any duplicate numbers in rows, columns, or subgrids |
| Quit          | `quit`     | Exit the game                                    |

### Rules

- You **cannot** modify pre-filled (given) cells.
- Numbers must be between **1 and 9**.
- Conflicting moves are accepted but flagged when you run `check`.

### Winning

The game ends automatically when all cells are correctly filled with no rule violations.

---

## How to Run Tests

```
dotnet test
```

All 116 tests should pass.

---

## Project Structure

```
SudokuApp/
├── src/
│   ├── Sudoku.Core/          # Game logic: grid, rules, solver, puzzle generation
│   └── Sudoku.ConsoleApp/    # Console UI: commands, rendering, game loop
└── tests/
    └── Sudoku.Tests/         # Unit and end-to-end tests (xUnit)
```

## Design Overview

- **`Sudoku.Core`** contains all game logic and has no dependency on the console. It can be reused with any UI.
- **`Sudoku.ConsoleApp`** handles user input and output. Each command (`place`, `clear`, `hint`, `check`, `quit`) is a separate class implementing `IGameCommand`.
- **Puzzle generation** starts from a fully solved grid, then removes cells one by one while ensuring the puzzle remains uniquely solvable (using a backtracking solver).
- **Solving** uses a backtracking algorithm with bitmask constraint tracking and a minimum-remaining-values (MRV) heuristic for efficiency.
