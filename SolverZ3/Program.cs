using System;
using NoyauTP;

namespace SolverZ3
{
    class Program
    {
        static void Main(string[] args)
        {

            var s = new Sudoku();
     
            s.newEasySudoku(0);
            var solver = new Z3();
            var solution = solver.Solve(s);
            solution.showSudoku();
            Console.Read();

            // Console.WriteLine("Hello World!");
        }
    }

    
}
