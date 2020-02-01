using System;
using System.IO;
using NoyauTP;

namespace Liensdansants
{
    class Program
    {
        static void Main(string[] args)
        {
            var lignes = File.ReadAllLines(@"..\..\..\Sudoku_Easy50.txt");
            var sudokus = Sudoku.ParseMulti(lignes);
            var solveur = new SolveurLiensDansants();
            var sudokuAResoudre = sudokus[1];
            Console.WriteLine(sudokuAResoudre);
            var solution1 = solveur.ResoudreSudoku(sudokuAResoudre);
            Console.WriteLine(solution1);
            Console.Read();


        }
    }


}
