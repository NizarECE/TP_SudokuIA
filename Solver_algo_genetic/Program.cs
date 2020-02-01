using System;
using NoyauTP;

namespace Solver_algo_genetic
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sudoku();
            s.newEasySudoku(0);
			s.showSudoku();
            var solver = new SolverAlgoGenetic();
            var solution = solver.Solve(s);
            solution.showSudoku();
            Console.Read();
        }
    }
}
