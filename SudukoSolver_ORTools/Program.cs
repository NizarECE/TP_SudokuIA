using System;
using NoyauTP;

namespace SudukoSolver_ORTools
{
    class Program
    {
        static void Main(string[] args)
        {
            var s = new Sudoku();
            s.showSudoku();
            Console.Read();
        }
    }
}
