using System.Linq;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Terminations;
using NoyauTP;


namespace Solver_algo_genetic
{
    public static class SudokuGeneticHelper
    {
		
		public static Sudoku EvolveSudokuSolution( Sudoku sudokuBoard, int populationSize, int generationNb)
		{
			var sudokuChromosome = new SudokuCellsChromosome(sudokuBoard);
			var fitness = new SudokuFitness(sudokuBoard);
			var selection = new EliteSelection();
			var crossover = new UniformCrossover();
			var mutation = new UniformMutation();

			var population = new Population(populationSize, populationSize, sudokuChromosome);
			var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation)
			{
				Termination = new OrTermination(new ITermination[]
				{
					new FitnessThresholdTermination(0),
					new GenerationNumberTermination(generationNb)
				})
			};

			ga.Start();

			var bestIndividual = ((SudokuCellsChromosome)ga.Population.BestChromosome);
			return bestIndividual.GetSudokus()[0];
			
		}
	}
}
