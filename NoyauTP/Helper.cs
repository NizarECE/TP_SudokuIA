using System;
using System.Linq;

namespace NoyauTP
{
	public static class Helper
	{

		public static T[,] To2D<T>(this T[][] source)
		{
			try
			{
				int FirstDim = source.Length;
				int SecondDim = source.GroupBy(row => row.Length).Single().Key; // throws InvalidOperationException if source is not rectangular

				var result = new T[FirstDim, SecondDim];
				for (int i = 0; i < FirstDim; ++i)
				for (int j = 0; j < SecondDim; ++j)
					result[i, j] = source[i][j];

				return result;
			}
			catch (InvalidOperationException)
			{
				throw new InvalidOperationException("The given jagged array is not rectangular.");
			}
		}


		public static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
		{
			int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
			int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
			int numberOfRows = rowsLastIndex - rowsFirstIndex + 1;

			int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
			int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
			int numberOfColumns = columnsLastIndex - columnsFirstIndex + 1;

			T[][] jaggedArray = new T[numberOfRows][];
			for (int i = 0; i < numberOfRows; i++)
			{
				jaggedArray[i] = new T[numberOfColumns];

				for (int j = 0; j < numberOfColumns; j++)
				{
					jaggedArray[i][j] = twoDimensionalArray[i + rowsFirstIndex, j + columnsFirstIndex];
				}
			}
			return jaggedArray;
		}


	}
}