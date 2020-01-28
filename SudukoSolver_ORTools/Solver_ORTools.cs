using System;
using System.Diagnostics;

namespace SudokuSolver_ORTools
{
    class Solver_ORTools
    {
        public static String Exe(string sudoku)
        {
            // Création du process d'exécution du shell python
            var psi = new ProcessStartInfo();

            // Chemins d'accès du shell et du script exécuté                 !!! Bien vérifier le chemin entré !!!
            psi.FileName = @"C:\Users\Adrien\Anaconda3\python.exe";
            var script = @"C:\Users\Adrien\Desktop\ECE\ING4\Intelligence artificielle\sudoku_ortools.py";
            
            // Ajout des arguments à passer au shell et des paramètres d'exécution
            psi.ArgumentList.Add(script);
            psi.ArgumentList.Add(sudoku);
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // Création des variables de sorties d'exécution
            var errors = "";
            var results = "";

            // Exécution du script
            using(var process = Process.Start(psi))
            {

                // Récupération des sorties d'exécution
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
               
            }

            // Affichage des erreurs
            Console.WriteLine("ERRORS:");
            if (errors == "")
            {
                Console.WriteLine("No error");
            }
            else
            {
                Console.WriteLine(errors);
            }

            // Renvoie le resultat
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
