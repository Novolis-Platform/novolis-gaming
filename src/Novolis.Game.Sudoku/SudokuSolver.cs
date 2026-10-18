namespace Novolis.Game.Sudoku;

/// <summary>
/// Classic-capable Sudoku solver. It first applies deterministic singles and
/// then uses minimum-remaining-values backtracking when allowed.
/// </summary>
public sealed class SudokuSolver
{
    /// <summary>Creates a solver for a ruleset.</summary>
    public SudokuSolver(SudokuRuleSet? ruleset = null) =>
        Ruleset = ruleset ?? ClassicSudokuRuleSet.Default;

    /// <summary>Rules used for candidates, validation, and search.</summary>
    public SudokuRuleSet Ruleset { get; }

    /// <summary>Solves a board or reports why it cannot be solved.</summary>
    public SudokuSolveResult Solve(
        SudokuBoard board,
        SudokuSolveOptions? options = null)
    {
        ArgumentNullException.ThrowIfNull(board);
        options ??= new SudokuSolveOptions();
        var maximumSolutions = System.Math.Max(1, options.MaximumSolutions);
        var validation = Ruleset.Validate(board);
        if (!validation.IsValid)
        {
            return new SudokuSolveResult
            {
                Status = SudokuSolveStatus.Invalid,
                Statistics = new SudokuSolveStatistics(),
            };
        }

        var state = new SearchState(maximumSolutions, options.CaptureSteps);
        var working = board.Clone();
        var path = new List<SudokuSolveStep>();
        if (!Propagate(working, state, path))
        {
            return state.CreateResult(
                SudokuSolveStatus.Unsolvable,
                solution: null,
                solutionCount: 0,
                steps: path);
        }

        if (working.IsComplete)
        {
            return state.CreateResult(
                SudokuSolveStatus.Solved,
                working,
                solutionCount: 1,
                steps: path);
        }

        if (options.LogicOnly)
        {
            return state.CreateResult(
                SudokuSolveStatus.Stalled,
                working,
                solutionCount: 0,
                steps: path);
        }

        Search(state, working, depth: 0, path);
        var status = state.Solutions.Count switch
        {
            0 => SudokuSolveStatus.Unsolvable,
            1 => SudokuSolveStatus.Solved,
            _ => SudokuSolveStatus.MultipleSolutions,
        };
        return state.CreateResult(
            status,
            state.Solutions.FirstOrDefault(),
            state.Solutions.Count,
            state.FirstSolutionSteps is { } firstSteps ? firstSteps : path);
    }

    /// <summary>Returns legal candidates for an empty cell.</summary>
    public CandidateMask GetCandidates(SudokuBoard board, int cellIndex)
    {
        ArgumentNullException.ThrowIfNull(board);
        SudokuConstants.ValidateCellIndex(cellIndex);
        if (board.Get(cellIndex) != SudokuConstants.Empty)
            return CandidateMask.None;

        var candidates = CandidateMask.All;
        foreach (var peer in Ruleset.GetPeers(cellIndex))
        {
            var value = board.Get(peer);
            if (value != SudokuConstants.Empty)
                candidates = candidates.Remove(value);
        }

        for (byte value = 1; value <= SudokuConstants.SymbolCount; value++)
        {
            if (candidates.Contains(value)
                && Ruleset.Constraints.Any(constraint =>
                    !constraint.Allows(board, cellIndex, value)))
            {
                candidates = candidates.Remove(value);
            }
        }

        return candidates;
    }

    /// <summary>Returns candidates for every cell in row-major order.</summary>
    public CandidateMask[] GetCandidateGrid(SudokuBoard board)
    {
        ArgumentNullException.ThrowIfNull(board);
        var candidates = new CandidateMask[SudokuConstants.CellCount];
        for (var cellIndex = 0; cellIndex < candidates.Length; cellIndex++)
            candidates[cellIndex] = GetCandidates(board, cellIndex);
        return candidates;
    }

    private void Search(
        SearchState state,
        SudokuBoard board,
        int depth,
        List<SudokuSolveStep> path)
    {
        if (state.Solutions.Count >= state.MaximumSolutions)
            return;

        if (!Propagate(board, state, path))
        {
            state.Backtracks++;
            return;
        }

        if (board.IsComplete)
        {
            state.Solutions.Add(board.Clone());
            if (state.FirstSolutionSteps is null)
                state.FirstSolutionSteps = path.ToArray();
            return;
        }

        var (cellIndex, candidates) = FindMinimumRemainingValue(board);
        if (cellIndex < 0 || candidates.IsEmpty)
        {
            state.Backtracks++;
            return;
        }

        foreach (var value in candidates.Values())
        {
            if (state.Solutions.Count >= state.MaximumSolutions)
                return;

            state.SearchNodes++;
            state.MaxSearchDepth = System.Math.Max(state.MaxSearchDepth, depth + 1);
            var branch = board.Clone();
            branch.Set(cellIndex, value);
            var branchPath = new List<SudokuSolveStep>(path);
            if (state.CaptureSteps)
            {
                branchPath.Add(new SudokuSolveStep(
                    cellIndex,
                    value,
                    SudokuTechnique.Guess,
                    $"Tried {value} at cell {cellIndex} using minimum remaining values."));
            }

            var solutionCountBefore = state.Solutions.Count;
            Search(state, branch, depth + 1, branchPath);
            if (state.Solutions.Count == solutionCountBefore)
                state.Backtracks++;
        }
    }

    private bool Propagate(
        SudokuBoard board,
        SearchState state,
        List<SudokuSolveStep> path)
    {
        while (true)
        {
            var changed = false;
            for (var cellIndex = 0; cellIndex < SudokuConstants.CellCount; cellIndex++)
            {
                if (board.Get(cellIndex) != SudokuConstants.Empty)
                    continue;

                var candidates = GetCandidates(board, cellIndex);
                if (candidates.IsEmpty)
                    return false;
                if (!candidates.IsSingle)
                    continue;

                var value = candidates.SingleValue;
                Place(
                    board,
                    state,
                    path,
                    cellIndex,
                    value,
                    SudokuTechnique.NakedSingle,
                    $"Only candidate for cell {cellIndex}.");
                changed = true;
            }

            if (changed)
                continue;

            foreach (var unit in Ruleset.Units)
            {
                for (byte value = 1; value <= SudokuConstants.SymbolCount; value++)
                {
                    var existing = unit.Cells.Any(cell => board.Get(cell) == value);
                    if (existing)
                        continue;

                    var possibleCells = unit.Cells
                        .Where(cell => board.Get(cell) == SudokuConstants.Empty)
                        .Where(cell => GetCandidates(board, cell).Contains(value))
                        .ToArray();
                    if (possibleCells.Length == 0)
                        return false;
                    if (possibleCells.Length != 1)
                        continue;

                    var cellIndex = possibleCells[0];
                    Place(
                        board,
                        state,
                        path,
                        cellIndex,
                        value,
                        SudokuTechnique.HiddenSingle,
                        $"{value} appears only once as a candidate in {unit.Id}.");
                    changed = true;
                    break;
                }

                if (changed)
                    break;
            }

            if (!changed)
                return true;
        }
    }

    private static void Place(
        SudokuBoard board,
        SearchState state,
        List<SudokuSolveStep> path,
        int cellIndex,
        byte value,
        SudokuTechnique technique,
        string reason)
    {
        board.Set(cellIndex, value);
        state.LogicalSteps++;
        if (state.CaptureSteps)
        {
            path.Add(new SudokuSolveStep(
                cellIndex,
                value,
                technique,
                reason));
        }
    }

    private (int CellIndex, CandidateMask Candidates) FindMinimumRemainingValue(
        SudokuBoard board)
    {
        var selectedCell = -1;
        var selectedCandidates = CandidateMask.None;
        var smallestCount = int.MaxValue;

        for (var cellIndex = 0; cellIndex < SudokuConstants.CellCount; cellIndex++)
        {
            if (board.Get(cellIndex) != SudokuConstants.Empty)
                continue;

            var candidates = GetCandidates(board, cellIndex);
            if (candidates.Count < smallestCount)
            {
                selectedCell = cellIndex;
                selectedCandidates = candidates;
                smallestCount = candidates.Count;
                if (smallestCount <= 2)
                    break;
            }
        }

        return (selectedCell, selectedCandidates);
    }

    private sealed class SearchState(int maximumSolutions, bool captureSteps)
    {
        public int MaximumSolutions { get; } = maximumSolutions;
        public bool CaptureSteps { get; } = captureSteps;
        public List<SudokuBoard> Solutions { get; } = [];
        public SudokuSolveStep[]? FirstSolutionSteps { get; set; }
        public int LogicalSteps { get; set; }
        public int SearchNodes { get; set; }
        public int Backtracks { get; set; }
        public int MaxSearchDepth { get; set; }

        public SudokuSolveResult CreateResult(
            SudokuSolveStatus status,
            SudokuBoard? solution,
            int solutionCount,
            IReadOnlyList<SudokuSolveStep> steps) =>
            new()
            {
                Status = status,
                Solution = solution,
                SolutionCount = solutionCount,
                Statistics = new SudokuSolveStatistics
                {
                    LogicalSteps = LogicalSteps,
                    SearchNodes = SearchNodes,
                    Backtracks = Backtracks,
                    MaxSearchDepth = MaxSearchDepth,
                },
                Steps = steps,
            };
    }
}
