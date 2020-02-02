using System;
using System.Collections.Generic;
using System.Text;
using NoyauTP;
using System.Linq;

namespace Norvig_method
{
    public class Methode : ISudokuSolver
    {
        // numerotation des ligne et colonnes
        static string rows = "ABCDEFGHI";
        static string cols = "123456789";
        static string digits = "123456789";
        static string[] squares = cross(rows, cols);
        static Dictionary<string, IEnumerable<string>> peers;
        static Dictionary<string, IGrouping<string, string[]>> units;

        static Methode()
        {

            // regroupe les 3 "units" (cad des lignes, colonnes et box) où se trouve une case
            var unitlist = ((from c in cols select cross(rows, c.ToString()))
                               .Concat(from r in rows select cross(r.ToString(), cols))
                               .Concat(from rs in (new[] { "ABC", "DEF", "GHI" }) from cs in (new[] { "123", "456", "789" }) select cross(rs, cs)));

            // les units de la case -> recuperation dans un dictionnaire avec clef (la case) et valeur (liste of unit)
            units = (from s in squares from u in unitlist where u.Contains(s) group u by s into g select g).ToDictionary(g => g.Key);

            //les peers de la case -> recuperation dans un dictionnaire avec clef (la case) et valeur (liste of peers)
            peers = (from s in squares from u in units[s] from s2 in u where s2 != s group s2 by s into g select g).ToDictionary(g => g.Key, g => g.Distinct());

        }


        //méthode générale de solving
        public Sudoku Solve(Sudoku s)
        {
            //création tableau de int pour pouvoir appeler getSudoku :/
            int[][] temp = new int[2][];
            int i = 0;
            int j = 0;
            //Console.WriteLine(s.show(s.getSudoku(temp)));
            //solution trouvé à cette ligne
            var solution = search(parse_grid(s.sudokuToString(s.getSudoku(temp))));
            //simple remplissage sur la classe sudoku a travers kvp qui permet de parcourir le dictionnaire renvoyé par search
            foreach (KeyValuePair<string, string> kvp in solution)
            {
                if (j == 9)
                {
                    i++;
                    j = 0;
                }
                //on met la value de kvp en int dans le setCaseSudoku
                s.setCaseSudoku(i, j % 9, Int32.Parse(kvp.Value));
                j++;
            }
            //Console.Write("Balise 2");
            Console.WriteLine(s.show(s.getSudoku(temp)));
            return s;

        }
        // methode de concatenation des caracteres (pour ecrire dans le tableau A1 par exemple)
        static string[] cross(string A, string B)
        {
            return (from a in A from b in B select "" + a + b).ToArray();
        }

        // Fonction qui permet d'extraire les valeurs evidante 
        public static Dictionary<string, string> parse_grid(string grid)
        {
            var grid2 = from c in grid where "0.-123456789".Contains(c) select c;
            var values = squares.ToDictionary(s => s, s => digits); //To start, every square can be any digit

            foreach (var sd in zip(squares, (from s in grid select s.ToString()).ToArray()))
            {
                var s = sd[0];
                var d = sd[1];

                if (digits.Contains(d) && assign(values, s, d) == null)
                {
                    return null;
                }
            }

            return values;
        }

        //"Using depth-first search and propagation, try all possible values."
        public static Dictionary<string, string> search(Dictionary<string, string> values)
        {
            if (values == null)
            {
                return null; // Failed earlier
            }
            if (all(from s in squares select values[s].Length == 1 ? "" : null))
            {
                return values; // Solved!
            }

            // Chose the unfilled square s with the fewest possibilities
            var s2 = (from s in squares where values[s].Length > 1 orderby values[s].Length ascending select s).First();

            return some(from d in values[s2]
                        select search(assign(new Dictionary<string, string>(values), s2, d.ToString())));
        }

        //Eliminate all the other values (except d) from values[s] and propagate.
        static Dictionary<string, string> assign(Dictionary<string, string> values, string s, string d)
        {
            if (all(
                    from d2 in values[s]
                    where d2.ToString() != d
                    select eliminate(values, s, d2.ToString())))
            {
                return values;
            }
            return null;
        }


        static bool all<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e == null) return false;
            }
            return true;
        }


        // Permet de retourner l'element de la sequence qui est vrai
        static T some<T>(IEnumerable<T> seq)
        {
            foreach (var e in seq)
            {
                if (e != null) return e;
            }
            return default;
        }


        // Eliminate d from values[s]; propagate when values or places <= 2.
        static Dictionary<string, string> eliminate(Dictionary<string, string> values, string s, string d)
        {
            if (!values[s].Contains(d))
            {
                return values;
            }
            values[s] = values[s].Replace(d, "");
            if (values[s].Length == 0)
            {
                return null; //Contradiction: removed last value
            }
            else if (values[s].Length == 1)
            {
                //If there is only one value (d2) left in square, remove it from peers
                var d2 = values[s];
                if (!all(from s2 in peers[s] select eliminate(values, s2, d2)))
                {
                    return null;
                }
            }

            //Now check the places where d appears in the units of s
            foreach (var u in units[s])
            {
                var dplaces = from s2 in u where values[s2].Contains(d) select s2;
                if (dplaces.Count() == 0)
                {
                    return null;
                }
                else if (dplaces.Count() == 1)
                {
                    // d can only be in one place in unit; assign it there
                    if (assign(values, dplaces.First(), d) == null)
                    {
                        return null;
                    }
                }
            }
            return values;
        }

        static string[][] zip(string[] A, string[] B)
        {
            var n = Math.Min(A.Length, B.Length);
            string[][] sd = new string[n][];
            for (var i = 0; i < n; i++)
            {
                sd[i] = new string[] { A[i].ToString(), B[i].ToString() };
            }
            return sd;
        }


    }

}