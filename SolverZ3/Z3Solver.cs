using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Z3;
using Noyau;

namespace Z3Solver
{
    class Z3Solver : ISudokuSolver
    {
        public Sudoku Solve(Sudoku s)
        {
            //Création d'une instance Contexte
            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                // 9x9 matrix of integer variables
                IntExpr[][] X = new IntExpr[9][];
                for (uint i = 0; i < 9; i++)  // uint : entier positif
                {
                    X[i] = new IntExpr[9]; //Liste de 9 case
                    for (uint j = 0; j < 9; j++) //Parcourt la liste
                        //pour chaque case on affecte une valeur
                        X[i][j] = (IntExpr)ctx.MkConst(ctx.MkSymbol("x_" + (i + 1) + "_" + (j + 1)), ctx.IntSort);
                }

                // each cell contains a value in {1, ..., 9}
                Expr[][] cells_c = new Expr[9][];
                for (uint i = 0; i < 9; i++)
                {
                    cells_c[i] = new BoolExpr[9];
                    for (uint j = 0; j < 9; j++)
                        cells_c[i][j] = ctx.MkAnd(ctx.MkLe(ctx.MkInt(1), X[i][j]),
                                                  ctx.MkLe(X[i][j], ctx.MkInt(9)));
                }

                // each row contains a digit at most once
                BoolExpr[] rows_c = new BoolExpr[9];
                for (uint i = 0; i < 9; i++)
                    rows_c[i] = ctx.MkDistinct(X[i]);

                // each column contains a digit at most once
                BoolExpr[] cols_c = new BoolExpr[9];
                for (uint j = 0; j < 9; j++)
                {
                    IntExpr[] column = new IntExpr[9];
                    for (uint i = 0; i < 9; i++)
                        column[i] = X[i][j];

                    cols_c[j] = ctx.MkDistinct(column);
                }

                // each 3x3 square contains a digit at most once
                BoolExpr[][] sq_c = new BoolExpr[3][];
                for (uint i0 = 0; i0 < 3; i0++)
                {
                    sq_c[i0] = new BoolExpr[3];
                    for (uint j0 = 0; j0 < 3; j0++)
                    {
                        IntExpr[] square = new IntExpr[9];
                        for (uint i = 0; i < 3; i++)
                            for (uint j = 0; j < 3; j++)
                                square[3 * i + j] = X[3 * i0 + i][3 * j0 + j];
                        sq_c[i0][j0] = ctx.MkDistinct(square); //compare chaque carré
                    }
                }

                BoolExpr sudoku_c = ctx.MkTrue();
                foreach (BoolExpr[] t in cells_c)
                    sudoku_c = ctx.MkAnd(ctx.MkAnd(t), sudoku_c);
                sudoku_c = ctx.MkAnd(ctx.MkAnd(rows_c), sudoku_c);
                sudoku_c = ctx.MkAnd(ctx.MkAnd(cols_c), sudoku_c);
                foreach (BoolExpr[] t in sq_c)
                    sudoku_c = ctx.MkAnd(ctx.MkAnd(t), sudoku_c);
                
                // sudoku instance, we use '0' for empty cells
                var listCell = new List<List<int>>();
                for (int i = 0; i < 9; i++)
                {
                    listCell.Add(new List<int>());
                    for (int j = 0; j < 9; j++)
                    {
                        listCell[listCell.Count - 1].Add(s.Cells[(9 * i) + j]); // mettre j à la place du 1 non ?
                    }
                }
                int[,] instance = To2D(listCell.Select(ligne => ligne.ToArray()).ToArray());

                BoolExpr instance_c = ctx.MkTrue();
                for (uint i = 0; i < 9; i++)
                {
                    for (uint j = 0; j < 9; j++ )
                    {
                        instance_c = ctx.MkAnd(instance_c,
                            (BoolExpr)
                            ctx.MkITE(ctx.MkEq(ctx.MkInt(instance[i, j]), ctx.MkInt(0)),
                                        ctx.MkTrue(),
                                        ctx.MkEq(X[i][j], ctx.MkInt(instance[i, j]))));

                    }
                }

                Solver solve = ctx.MkSolver();
                solve.Assert(sudoku_c);
                solve.Assert(instance_c);

                if (solve.Check() == Status.SATISFIABLE)
                {
                    Model m = solve.Model;
                    Expr[,] R = new Expr[9, 9];
                    for (int i = 0; i < 9; i++)
                        for (int j = 0; j < 9; j++)
                            R[i, j] = m.Evaluate(X[i][j]);
                    Console.WriteLine("Sudoku solution:");

                    var solu = new Sudoku();
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 9; j++)
                            //Console.Write(" " + R[i, j]);
                            solu.Cells[i * 9 + j] = int.Parse( R[i, j].ToString() );
                        Console.WriteLine();
                    }

                    return solu;

                }
                else
                {
                    Console.WriteLine("Failed to solve sudoku");

                }

                return null;
            }
        }

        static T[,] To2D<T>(T[][] source)
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
    }
}
