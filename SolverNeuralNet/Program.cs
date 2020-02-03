using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using CsvHelper;
using Humanizer;
using Keras;
using Keras.Models;
using Numpy;
using Python.Runtime;


namespace SolverNeuralNet
{
	class Program
	{
		static void Main(string[] args)
		{

			var strPathCSV = GetFullPath(@"Dataset\sudoku.csv.gz");
			var strPathModel = GetFullPath(@"Models\sudoku.model");
			var nbSudokus = 1000;

			var stopW = Stopwatch.StartNew();

			
			var sudokus = DataSetHelper.ParseCSV(strPathCSV, nbSudokus);

			//var testSudoku = sudokus[0];

			//testSudoku.Quiz.showInitialSudoku();
			//Console.Write($"Given Solution :");
			//testSudoku.Solution.showInitialSudoku();
			var rand = new Random();
			var preTrainedModel = NeuralNetHelper.LoadModel(strPathModel);

			// Attribution des sudokus par niveau aléatoire
			NoyauTP.Sudoku sudokuzzz = new NoyauTP.Sudoku();
			NoyauTP.Sudoku sudokuzz = new NoyauTP.Sudoku();
			NoyauTP.Sudoku sudokuz = new NoyauTP.Sudoku();
			sudokuzzz.newEasySudoku(rand.Next(90));
			sudokuzz.newHardSudoku(rand.Next(100));
			sudokuz.newTop95Sudoku(rand.Next(82));
			//Console.Write($"Sudoku facile");
			
			//var solvedWithNeuralNet = NeuralNetHelper.SolveSudoku(testSudoku.Quiz, preTrainedModel);

			// Résolution avec la fonction SolveSudoku en prenant le modele préentrainé 
			var solvedWithNeuralNet_easy = NeuralNetHelper.SolveSudoku(sudokuzzz, preTrainedModel);
			var solvedWithNeuralNet_medium = NeuralNetHelper.SolveSudoku(sudokuzz, preTrainedModel);
			var solvedWithNeuralNet_hard = NeuralNetHelper.SolveSudoku(sudokuz, preTrainedModel);


			Console.Write($"Sudoku easy to solve:");
			sudokuzzz.showSudoku();
			Console.Write($"Solved with Neural Net :");


			solvedWithNeuralNet_easy.showSudoku();
			Console.Write($"Sudoku medium to solve:");
			sudokuzz.showSudoku();
			Console.Write($"Solved with Neural Net :");
			solvedWithNeuralNet_medium.showSudoku();


			Console.Write($"Sudoku hard to solve:");
			sudokuz.showSudoku();
			Console.Write($"Solved with Neural Net :");
			solvedWithNeuralNet_hard.showSudoku();


			Console.WriteLine($"Time Elpased: {stopW.Elapsed.Humanize(5)}");
			Console.ReadLine();
		}

		static string GetFullPath(string relativePath)
		{
			return System.IO.Path.Combine(Environment.CurrentDirectory, @"..\..\..\" + relativePath);
		}
	}

	

}
