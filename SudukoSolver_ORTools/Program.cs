using System;
using NoyauTP;
using SudokuSolver_ORTools;

namespace SudukoSolver_ORTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Sudoku s = new Sudoku();
            s.newEasySudoku(-1);
            s.showInitialSudoku();
            String sudoku_string = s.sudokuToString(s.getInitialSudoku());
            String sudoku_resolu_string = Solver_ORTools.Exe(sudoku_string);
            int[][] sudoku_resolu = s.stringToSudoku(sudoku_resolu_string);
            s.show(sudoku_resolu);
            Console.Read();
        }
    }
}
