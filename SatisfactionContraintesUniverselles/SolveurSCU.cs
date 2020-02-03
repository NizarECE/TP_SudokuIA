using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using aima.core.search.csp;
using NoyauTP;

namespace SatisfactionContraintesUniverselles
{
    public class SolveurSCU : ISudokuSolver
    {



        public Sudoku Solve(Sudoku s)
        {
            //Construction du CSP

            var objCSP = SudokuCSPHelper.GetSudokuCSP(s);


			// Définition d'une stratégie de résolution

			var objStrategyInfo = new CSPStrategyInfo();
			objStrategyInfo.EnableLCV = true;
			objStrategyInfo.Inference = CSPInference.ForwardChecking;
			objStrategyInfo.Selection = CSPSelection.MRVDeg;
			objStrategyInfo.StrategyType = CSPStrategy.ImprovedBacktrackingStrategy;
			objStrategyInfo.MaxSteps = 5000;
			var objStrategy = objStrategyInfo.GetStrategy();



			// Résolution du Sudoku
            var objChrono = Stopwatch.StartNew();
			var assignation = objStrategy.solve(objCSP);
            Console.WriteLine($"Pure solve Time : {objChrono.Elapsed.TotalMilliseconds} ms");


			//Traduction du Sudoku
			SudokuCSPHelper.SetValuesFromAssignment(assignation, s);

			return s;
        }
    }
}
