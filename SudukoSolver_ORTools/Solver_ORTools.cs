using System;
using System.Diagnostics;

namespace SudokuSolver_ORTools
{
    class Solver_ORTools
    {
        public static String Exe(string sudoku)
        {

            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Users\Adrien\Anaconda3\python.exe";

            var script = @"C:\Users\Adrien\Desktop\ECE\ING4\Intelligence artificielle\sudoku_ortools.py";
            

            psi.ArgumentList.Add(script);
            psi.ArgumentList.Add(sudoku);

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            var errors = "";
            var results = "";

            using(var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
               
            }

            Console.WriteLine("ERRORS:");
            if (errors == "")
            {
                Console.WriteLine("No error");
            }
            else
            {
                Console.WriteLine(errors);
            }

            Console.WriteLine("RESULTS:");
            results = results.Replace(", ","");
            results = results.Replace("][","");
            results = results.Replace("[", "");
            results = results.Replace("]", "");
            results = results.Replace("\n", "");
            results = results.Replace("\r", "");

            return results;
        }
        
    }
}
