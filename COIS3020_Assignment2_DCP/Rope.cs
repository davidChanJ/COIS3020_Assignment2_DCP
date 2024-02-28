using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS3020_Assignment2_DCP
{
    public class Rope
    {
        string S;
        private Node root;
        //Constructor
        public Rope(string S)
        {
            this.S = S;
            root = Build(S, 0, S.Length-1);
        }

        //Inserting and Deleting
        public void Insert(string S, int i)
        {
            //Getting data
        }
        public void Delete(int i, int j)
        {
            //Choosing where to delete

            //Search via series of Nodes(no array)
        }

        //Substrings
        public string subString(int i, int j)
        {
            //Search in big string first

            //Searches until finding a smaller part
            return S[i, j];
        }

        public int Find(string S)
        {
            int i = 0;
            //Using a loop, similar to for loop but series of arrays
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

        public int Length(Node node)
        {
            //Find if not null
            if(node == null)
                return 0;
            //See if node is leaf:
            if (node is LeafNode leaf)
                return leaf.Length;
            else if(node is InternalNode internalNode) //Check if it is internam node:
                return Length(internalNode.Left) + Length(internalNode.Right);
            //Goes until the end
            return -1;
        }

        public string ToString()
        {
            return "";
        }

        public void PrintRope()
        {
            PrintRope(root);
        }

        private void PrintRope(Node node)
        {
            //Showing the root
            if (Node is LeafNode leaf)
                Console.WriteLine($"Lead Node: Length {leaf.Length}, String={leaf.Text}");
            else if (Node is InternalNode internalNode)
            {
                Console.WriteLine("Internal Node: ");
                Console.WriteLine("Left Child: ");
                PrintRope(internalNode.Left);
                Console.WriteLine("Right Child:");
                PrintRope(internalNode.Right);
            }
        }

        //Node
        public class Node
        {
            public string s { get; set; }
            public int Length { get; set; }
            public Node left, right;
            public Node root { get; set; }
            public Node()
            {
                this.s = s;
                this.Length = 0;
                this.left = left;
                this.right = right;
                this.root = root;
            }

            //Making internal node & child nodes?
        }

        public Node Build(string s, int i, int e)
        {
            //if start(i) is larger than end(j):
            if (i > e)
                return null;

            //Base case
            if (e - i + 1 <= THERESHOLD)
                return new LeafNode
                {
                    Length = j - s + 1,
                    TextReader = s.Substring(i, e - i + 1)
                };
            //Mid point
            int mid = i + (e - i) / 2;

            Node leftNode = Build(s, i, mid);
            Node rightNode = Build(s, mid + 1, e);

            //Return to internalNode:
            return new InternalNode
            {
                left = leftNode,
                right = rightNode
            };
        }
        public Node Concatenate(Node p, Node q)
        {
            //Setting is p and q are null
            if (p == null) return q;
            if (q == null) return p;
            //Check to reblance
            if (Random.Next(p.Length + q.Length) < p.Length)
            {
                left = Concatenate(p, (p.Length - 1));
                right = q;
            }
            else
            {
                return new InternalNode
                {
                    left = p,
                    right = q
                };
            }
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
