using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS3020_Assignment2_DCP
{
    public class Rope
    {
        string S;
        //Constructor
        public Rope(string S)
        {
            this.S = S;
        }

        //Inserting and Deleting
        public void Insert(string S, int i)
        {

        }
        public void Delete(int i, int j)
        {

        }

        //Substrings
        public string subString(int i, int j)
        {
            return S[i, j];
        }

        public int Find(string S)
        {
            int i = 0;
            if (S == null)
                return i; //Return the first occurence of the character c    
            return -1;
        }

        public char CharAt(int i)
        {
            return 'a';
        }

        public void Reverse()
        {
            //Possibly use for loop to find
        }

        public int Length()
        {
            return -1;
        }

        public string ToString()
        {
            return "";
        }

        public void PrintRope()
        {
            //Showing the root
        }
    }
}
