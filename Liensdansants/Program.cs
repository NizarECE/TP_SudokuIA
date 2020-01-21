using System;
using NoyauTP;

namespace Liensdansants
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sudoku();
           
            s.newEasySudoku(0);
            var solveur = new SolveurLiensDansants();
            var solution = solveur.Solve(s);
            solution.showSudoku();
            Console.Read();

           
        }
    }

    
}
