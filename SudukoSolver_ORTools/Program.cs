using System;
using NoyauTP;
using SudokuSolver_ORTools;

namespace SudukoSolver_ORTools
{
    class Program
    {
        static void Main(string[] args)
        {
            // Création d'un sudoku aléatoire de niveau facile
            Sudoku s = new Sudoku();
            s.newEasySudoku(-1);

            // Affichage du sudoku initial généré
            s.showInitialSudoku();

            // Passage du type Sudoku à String pour l'exécution du script python
            String sudoku_string = s.sudokuToString(s.getInitialSudoku());

            // Solver
            String sudoku_resolu_string = Solver_ORTools.Exe(sudoku_string);

            // Retour au type Sudoku pour l'affichage
            int[][] sudoku_resolu = s.stringToSudoku(sudoku_resolu_string);

            // Affichage du résultat
            Console.WriteLine("RESULTS:");
            s.show(sudoku_resolu);
            Console.Read();
        }
    }
}
