using System;
using System.Collections.Generic;
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
			objStrategyInfo.Inference = CSPInference.AC3;
			objStrategyInfo.Selection = CSPSelection.MRV;
			objStrategyInfo.StrategyType = CSPStrategy.ImprovedBacktrackingStrategy;
			objStrategyInfo.MaxSteps = 5000;
			var objStrategy = objStrategyInfo.GetStrategy();

			// Résolution du Sudoku
			var assignation = objStrategy.solve(objCSP);

			//Traduction du Sudoku
			SudokuCSPHelper.SetValuesFromAssignment(assignation, s);

			return s;
        }
    }
}
