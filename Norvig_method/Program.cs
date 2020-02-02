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
            var solveur = new Methode();
            var solution = solveur.Solve(s);
            Console.Read();
        }
    }
}
