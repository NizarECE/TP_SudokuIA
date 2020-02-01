using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using aima.core.search.csp;
using java.lang;
using java.util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NoyauTP;

namespace SatisfactionContraintesUniverselles
{

	


	public class SudokuCSPHelper

	{

		
		// Fonction principale pour construire le CSP à partir d'un masque de Sudoku à résoudre
		public static CSP GetSudokuCSP(Sudoku s)
		{

			//initialisation à l'aide des contraintes communes

			var toReturn = GetSudokuBaseCSP();


			// Ajout des contraintes spécifiques au masque fourni

			var sArray = s.getInitialSudoku();
			var cellVars = toReturn.getVariables();


			//récupération du masque
			var mask = new Dictionary<int, int>();
			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (sArray[i][j] != 0)
					{
						mask[i * 9 + j] = sArray[i][j];

					}
				}
			}

			//Ajout des contraintes de masque en faisant défiler les variables existantes

			var maskQueue = new Queue<int>(mask.Keys);

			var currentMaskIdx = maskQueue.Dequeue();
			var currentVarName = GetVarName(currentMaskIdx / 9, currentMaskIdx % 9);
			foreach (Variable objVar in cellVars.toArray())
			{
				if (objVar.getName() == currentVarName)
				{
					toReturn.addConstraint(new ValueConstraint(objVar, mask[currentMaskIdx]));
					if (maskQueue.Count == 0)
					{
						break;
					}
					currentMaskIdx = maskQueue.Dequeue();
					currentVarName = GetVarName(currentMaskIdx / 9, currentMaskIdx % 9);
				}

			}

			return toReturn;

		}

		// Récupération de la solution
		public static void SetValuesFromAssignment(Assignment a, Sudoku s)
		{

			foreach (Variable objVar in a.getVariables().toArray())
			{
				int rowIdx = 0;
				int colIdx = 0;
				GetIndices(objVar, ref rowIdx, ref colIdx);
				int value = (int)a.getAssignment(objVar);
				s.setCaseSudoku(rowIdx, colIdx, value);
			}
		}

		// CSP de Sudoku sans masque (les règles)
		public static CSP GetSudokuBaseCSP()
		{
			if (_BaseSudokuCSP == null)
			{
				lock (_BaseSudokuCSPLock)
				{
					if (_BaseSudokuCSP == null)
					{
						var toReturn = new DynamicCSP();

						
						//Domaine

						var cellPossibleValues = Enumerable.Range(1, 9);
						var cellDomain = new Domain(cellPossibleValues.Cast<object>().ToArray());



						//Variables

						var variables = new Dictionary<int, Dictionary<int, Variable>>();
						for (int rowIndex = 0; rowIndex < 9; rowIndex++)
						{
							var rowVars = new Dictionary<int, Variable>();
							for (int colIndex = 0; colIndex < 9; colIndex++)
							{
								var varName = GetVarName(rowIndex, colIndex);
								var cellVariable = new Variable(varName);
								toReturn.AddNewVariable(cellVariable);
								toReturn.setDomain(cellVariable, cellDomain);
								rowVars[colIndex] = cellVariable;
							}

							variables[rowIndex] = rowVars;
						}



						//Contraintes

						var contraints = new List<Constraint>();

						// Lignes
						foreach (var objPair in variables)
						{
							var ligneVars = objPair.Value.Values.ToList();
							var ligneContraintes = SudokuCSPHelper.GetAllDiffConstraints(ligneVars);
							contraints.AddRange(ligneContraintes);
						}

						//colonnes
						for (int j = 0; j < 9; j++)
						{
							var jClosure = j;
							var colVars = variables.Values.SelectMany(x => { return new Variable[] { x[jClosure] }; }).ToList();
							var colContraintes = SudokuCSPHelper.GetAllDiffConstraints(colVars);
							contraints.AddRange(colContraintes);
						}

						//Boites

						for (int b = 0; b < 9; b++)
						{
							var boiteVars = new List<Variable>();
							var iStart = b / 3;
							var jStart = b % 3;
							for (int i = 0; i < 3; i++)
							{
								for (int j = 0; j < 3; j++)
								{
									boiteVars.Add(variables[iStart + i][jStart + j]);
								}
							}
							var boitesContraintes = SudokuCSPHelper.GetAllDiffConstraints(boiteVars);
							contraints.AddRange(boitesContraintes);
						}


						//Ajout de toutes les contraintes
						foreach (var constraint in contraints)
						{
							toReturn.addConstraint(constraint);
						}

						_BaseSudokuCSP = toReturn;
					}

				}
			}

			return (CSP)_BaseSudokuCSP;//.Clone();
		}



		private static Regex _NameRegex =
			new Regex(@"cell(\d)(\d)", RegexOptions.Compiled | RegexOptions.CultureInvariant);

	    private static void GetIndices(Variable obVariable, ref int rowIdx, ref int colIdx)
	    {
		    var objMatch = _NameRegex.Match(obVariable.getName());
		    rowIdx = int.Parse(objMatch.Groups[0].Value, CultureInfo.InvariantCulture);
		    colIdx = int.Parse(objMatch.Groups[1].Value, CultureInfo.InvariantCulture);
		}

	    private static string GetVarName(int rowIndex, int colIndex)
	    {
		    return $"cell{rowIndex}{colIndex}";

	    }

	    private static object _BaseSudokuCSPLock = new object();
		private static DynamicCSP _BaseSudokuCSP;

		

		public static IEnumerable<Constraint> GetAllDiffConstraints(IList<Variable> vars)
		{
			var toReturn = new List<Constraint>();
			for (int i = 0; i < vars.Count; i++)
			{
				for (int j = i+1; j < vars.Count; j++)
				{
					var diffContraint =
						new aima.core.search.csp.NotEqualConstraint(vars[i],
							vars[j]);
					toReturn.Add(diffContraint);
				}
			}

			return toReturn;
		}


		
	}



	// Version customisée du CSP pour utiliser un constructeur vide, et fournissant le clone pour partager variables et une partie des contraintes
	public class DynamicCSP : CSP, ICloneable
	{

		public void AddNewVariable(Variable objVariable)
		{
			this.addVariable(objVariable);
		}


		public object Clone()
		{
			var toReturn = new DynamicCSP();
			foreach (Variable variable in this.getVariables().toArray())
			{
				toReturn.addVariable(variable);
				toReturn.setDomain(variable, this.getDomain(variable));
			}

			foreach (Constraint constraint in this.getConstraints().toArray())
			{
				toReturn.addConstraint(constraint);
			}

			return toReturn;
		}
	}


	// Contrainte en valeur pour le masque
	public class ValueConstraint : Constraint
	{

		public Variable Var1 { get; set; }

		public object Value { get; set; }

		private java.util.List _Scope;

		public ValueConstraint(Variable objVar1, object objValue)
		{
			Var1 = objVar1;
			Value = objValue;
			_Scope = new java.util.ArrayList(1);
			_Scope.add(Var1);
		}

		public java.util.List getScope()
		{
			return _Scope;
		}

		public bool isSatisfiedWith(Assignment a)
		{
			return a.getAssignment(Var1) == Value;
		}
	}


	public enum CSPStrategy
	{
		BacktrackingStrategy,
		ImprovedBacktrackingStrategy,
		MinConflictsStrategy,
	}

	public enum CSPInference
	{
		None,
		ForwardChecking,
		AC3,
	}

	public enum CSPSelection
	{
		DefaultOrder,
		MRV,
		MRVDeg,
	}

	// Classe pour simplifier les tests de stratégie de résolution
	public class CSPStrategyInfo
	{
		public CSPStrategyInfo()
		{
			MaxSteps = 50;
		}

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
		[JsonConverter(typeof(StringEnumConverter))]
		public CSPStrategy StrategyType { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
		[JsonConverter(typeof(StringEnumConverter))]
		public CSPSelection Selection { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Include)]
		[JsonConverter(typeof(StringEnumConverter))]
		public CSPInference Inference { get; set; }

		public bool EnableLCV { get; set; }

		public int MaxSteps { get; set; }

		public SolutionStrategy GetStrategy()
		{
			SolutionStrategy toReturn;
			switch (StrategyType)
			{
				case CSPStrategy.BacktrackingStrategy:
					toReturn = new BacktrackingStrategy();
					break;
				case CSPStrategy.ImprovedBacktrackingStrategy:
					var improved = new ImprovedBacktrackingStrategy();
					toReturn = improved;
					improved.enableLCV(EnableLCV);
					switch (Selection)
					{
						case CSPSelection.DefaultOrder:
							break;
						case CSPSelection.MRV:
							improved.setVariableSelection(ImprovedBacktrackingStrategy.Selection.MRV);
							break;
						case CSPSelection.MRVDeg:
							improved.setVariableSelection(ImprovedBacktrackingStrategy.Selection.MRV_DEG);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					switch (Inference)
					{
						case CSPInference.None:
							break;
						case CSPInference.ForwardChecking:
							improved.setInference(ImprovedBacktrackingStrategy.Inference.FORWARD_CHECKING);
							break;
						case CSPInference.AC3:
							improved.setInference(ImprovedBacktrackingStrategy.Inference.AC3);
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}
					break;
				case CSPStrategy.MinConflictsStrategy:
					toReturn = new MinConflictsStrategy(MaxSteps);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return toReturn;
		}
	}


	//Pour pouvoir faire de l'instrumentation au besoin
	public class StepCounter : CSPStateListener
	{
		
		public int AssignmentCount { get; set; }

		public int DomainCount { get; set; }

		public StepCounter()
		{
			this.AssignmentCount = 0;
			this.DomainCount = 0;
		}

		public virtual void stateChanged(Assignment assignment, aima.core.search.csp.CSP csp)
		{
			++this.AssignmentCount;
		}

		public virtual void stateChanged(aima.core.search.csp.CSP csp)
		{
			++this.DomainCount;
		}

		public virtual void reset()
		{
			this.AssignmentCount = 0;
			this.DomainCount = 0;
		}

		public virtual string getResults()
		{
			StringBuffer stringBuffer = new StringBuffer();
			stringBuffer.append(new StringBuilder().append("assignment changes: ").append(this.AssignmentCount).toString());
			if (this.DomainCount != 0)
				stringBuffer.append(new StringBuilder().append(", domain changes: ").append(this.DomainCount).toString());
			return stringBuffer.toString();
		}



		public override string ToString()
		{
			return getResults();
		}
	}

}
