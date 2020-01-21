using System;
using NoyauTP;

namespace Norvig_method
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sudoku();
            s.newEasySudoku(0);
            var solveur = new Norvig();
            var solution = solveur.Solve(s);
            solution.showSudoku();
        }
    }
}
