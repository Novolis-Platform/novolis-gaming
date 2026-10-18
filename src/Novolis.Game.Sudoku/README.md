# Novolis.Game.Sudoku

Host-neutral classic Sudoku infrastructure for product hosts and game tools.

The package provides:

- a 9×9 board backed by `Novolis.Math.Arrays.DenseGrid<T>`;
- classic row, column, and 3×3 box constraints;
- candidate masks, validation, logical singles, and bounded solution counting;
- deterministic seeded puzzle generation with uniqueness preservation; and
- explainable difficulty reports and player hints.

The solver consumes a ruleset and constraint collection. The first release
ships only `ClassicSudokuRuleSet`; later variants can add chess-inspired
constraints or shaped regions without coupling the engine to a UI host.
