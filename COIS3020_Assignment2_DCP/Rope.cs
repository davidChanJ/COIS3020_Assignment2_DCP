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

        //Node
        private class Node
        {
            public string s;
            public int length;
            public Node left, right;
            public Node root;
            public Node()
            {
                this.s = s;
                this.length = 0;
                this.left = left;
                this.right = right;
                this.root = root;
            }

            public Node Build(string s, int i, int j)
            {
                return;
            }
            public Node Concatenate(Node p, Node q)
            {
                return;
            }

            public Node Split(Node p)
            {
                //Making a divide line via root and point p, which is close to centre.

                //Creating 2 trees

                //Removing extra leaf nodes to balance
            }

            public Node Rebalance()
            {
                //Substr, concat(rope 1 and rope 2)
                    //Let
                       //If left:
                            //Do rope 1
                       //If right
                            //Do rope 2
                        // else
                            //subtract rope2 - rope1 - left rope
                    //in
                        //Concat left and right
                        Concatenate(left, right);
                return;
            }
        }
    }
}
