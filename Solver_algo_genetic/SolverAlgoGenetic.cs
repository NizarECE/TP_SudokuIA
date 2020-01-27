using System;
using NoyauTP;
namespace Solver_algo_genetic
{
    public class SolverAlgoGenetic:ISudokuSolver
    {
        public SolverAlgoGenetic()
        {
        }

        public Sudoku Solve(Sudoku s)
        {


            // Resoudre le sudoku 
            return SudokuGeneticHelper.EvolveSudokuSolution(s, 5000, 100);
        }
    }
}
