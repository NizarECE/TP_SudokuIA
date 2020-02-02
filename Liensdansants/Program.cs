using System;
using System.IO;
using NoyauTP;

namespace Liensdansants
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sudoku();
            s.newEasySudoku(0);
            s.showSudoku();
            var solveur = new SolveurLiensDansants();
            
            var solution1 = solveur.Solve(s);
            solution1.showSudoku();
            Console.Read();


        }
    }


}
