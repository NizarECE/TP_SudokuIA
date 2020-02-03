using System;
using System.IO;
using Noyau;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;


namespace Z3Solver
{
    class Program
    {
        static void Main(string[] args)
        {
            // Résoltuion avec C# 

            // On créé la grille de Sudoku à résoudre
            int[] initial_grid = new int[] { 0, 6, 0, 0, 5, 0, 0, 2, 0,

                                             0, 0, 0, 3, 0, 0, 0, 9, 0,

                                             7, 0, 0, 6, 0, 0, 0, 1, 0,

                                             0, 0, 6, 0, 3, 0, 4, 0, 0,

                                             0, 0, 4, 0, 7, 0, 1, 0, 0,

                                             0, 0, 5, 0, 9, 0, 8, 0, 0,

                                             0, 4, 0, 0, 0, 1, 0, 0, 6,

                                             0, 3, 0, 0, 0, 8, 0, 0, 0,

                                             0, 2, 0, 0, 4, 0, 0, 5, 0 };

            // Pour être conforme au groupe arbitre on créé "toSolve"
            Noyau.Sudoku toSolve = new Sudoku(initial_grid);
            Console.WriteLine(toSolve.ToString());


            Z3Solver solve = new Z3Solver();

            DateTime T_init = DateTime.Now;

            toSolve = solve.Solve(toSolve);

            DateTime T_final = DateTime.Now;

            TimeSpan t_exec = T_final - T_init;

            Console.WriteLine(toSolve.ToString());

            Console.WriteLine("La solution a été trouvé en " + (double)t_exec.TotalSeconds + " secondes");

            // Résoltuion avec Python en utilisant Ironpython

            string current_path = Directory.GetCurrentDirectory();
            string py_path = @"..\..\....\solve_using_z3.py";
            ScriptEngine engine = Python.CreateEngine();
            engine.ExecuteFile(py_path);
        }

    }
}

