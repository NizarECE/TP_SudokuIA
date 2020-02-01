using System;
using System.Diagnostics;
using NoyauTP;


namespace SatisfactionContraintesUniverselles
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sudoku();

            s.newTop95Sudoku(0);
			var solveur = new SolveurSCU();

            var objChrono = Stopwatch.StartNew();
            var solution = solveur.Solve(s);

            Console.WriteLine($"Time to first solution Top95: {objChrono.Elapsed.TotalMilliseconds} ms");

            solution.showTwoSudoku();

            objChrono.Restart();
            for (int i = 0; i < 50; i++)
            {
                s.newEasySudoku(i);
                
                solution = solveur.Solve(s);
                solution.showTwoSudoku();
            }
            
            Console.WriteLine($"Mean Time 50 easy Sudokus : {objChrono.Elapsed.TotalMilliseconds/50.0} ms");

            solution.showTwoSudoku();





            Console.Read();
        }
    }


}

