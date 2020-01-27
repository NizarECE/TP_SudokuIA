using System;
using NoyauTP;


namespace SatisfactionContraintesUniverselles
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sudoku();

            s.newEasySudoku(0);
            var solveur = new SolveurSCU();
            var solution = solveur.Solve(s);
            SudokuCSP essai = new SudokuCSP();
            solution.showSudoku();
            Console.Read();
        }
    }


}

