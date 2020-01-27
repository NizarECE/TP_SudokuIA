using System;
using System.Collections.Generic;
using aima.core.search.csp;

namespace SatisfactionContraintesUniverselles
{
    public class SudokuCSP : CSP

    {
        private int[][] m_board;
        public int[][]  Board
        {
		    get { return m_board; }
            set { m_board = value; }
	    }
        public SudokuCSP()
        {
            this.m_board= new int [9][];
            

        }

    }
}
