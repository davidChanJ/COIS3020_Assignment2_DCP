using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static COIS3020_Assignment2_DCP.Rope.Node;

namespace COIS3020_Assignment2_DCP {

    public class RopeNode
    {
        public string Value { get; set; } // For leaf nodes, this holds the substring
        public int Weight { get; set; } // Holds the total length of all characters in the left subtree
        public RopeNode Left { get; set; }
        public RopeNode Right { get; set; }

        public RopeNode(string value)
        {
            Value = value;
            Weight = value.Length;
        }
    }
    public class Rope
    {
        string S;
        private RopeNode Root { get; set; }
        //Constructor
        public Rope(string S)
        {
            this.S = S;
            Root = ConstructRope(S);
        }

        private RopeNode ConstructRope(string S)
        {
            // Base case for recursion: when S length is 10 or less
            if (S.Length <= 10)
            {
                return new RopeNode(S);
            }

            // Split S into roughly two equal parts
            List<RopeNode> nodes = new List<RopeNode>();
            for (int i = 0; i < S.Length; i += 10)
            {
                int length = Math.Min(10, S.Length - i);
                nodes.Add(new RopeNode(S.Substring(i, length)));
            }

            // Recursively combine nodes into a balanced tree
            while (nodes.Count > 1)
            {
                List<RopeNode> combinedNodes = new List<RopeNode>();
                for (int i = 0; i < nodes.Count; i += 2)
                {
                    if (i + 1 < nodes.Count)
                    {
                        RopeNode newNode = new RopeNode(null)
                        {
                            Left = nodes[i],
                            Right = nodes[i + 1],
                            Weight = nodes[i].Weight + (nodes[i].Left?.Weight ?? 0)
                        };
                        combinedNodes.Add(newNode);
                    }
                    else
                    {
                        combinedNodes.Add(nodes[i]); // Odd node out gets pushed to next level
                    }
                }
                nodes = combinedNodes; // Prepare for next level
            }

            return nodes[0]; // The root of the constructed rope
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
            string result = "abc";
            //Searches until finding a smaller part
            return result;
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
            PrintRope(Root);
        }

        private void PrintRope(RopeNode node)
        {
            //Showing the root
            //If it is a leaf
            if(node.Left != null || node.Right != null )
            {
                Console.WriteLine($"Lead Node: Length {node.Left.Weight}, String={node.Left.ToString()}");
            }
               
            //If it is a internal node
            else
            {
                Console.WriteLine("Internal Node: ");
                Console.WriteLine("Left Child: ");
                //PrintRope(internalNode.Left);
                Console.WriteLine("Right Child:");
                //PrintRope(internalNode.Right);
            }
        }

        //Node
        public class Node
        {
            public string s { get; set; }
            public int Length { get; set; }
            public Node()
            {
                this.s = s;
                this.Length = 0;

            }

            //Leaf and internal nodes
            public class LeafNode : Node
            {
                public int Length { get; set; }
                public string Text { get; set; }
                public LeafNode(int length, string text)
                {
                    Length = length;
                    Text = text;
                }
                public string GetString()
                {
                    return Text + Length.ToString();
                }
            }

            public class InternalNode : Node
            {
                public Node Left { get; set; }
                public Node Right { get; set; }
                //Constructor
                public InternalNode(int length)
                {
                    Left = null;
                    Right = null;
                }

                public string GetString()
                {
                    return Left.GetString() + Right.GetString();
                }
            }

            public string GetString()
            {
                return "";
            }

            //Making internal node & child nodes?
        }

        public Node Build(string s, int i, int e)
        {
            //if start(i) is larger than end(j):
            if (i > e)
                return null;

            //Base case
            int THERESHOLD = 5;
            int j = 5;
           
            if (e - i + 1 <= THERESHOLD)
                return new LeafNode( (i-e), " " )
                {
                    Length = j - s.Length + 1,
                    Text = s.Substring(i, e - i + 1)
                };
            //Mid point
            int mid = i + (e - i) / 2;

            Node leftNode = Build(s, i, mid);
            Node rightNode = Build(s, mid + 1, e);

            //Return to internalNode:
            return new InternalNode(S.Length)
            {
                Left = leftNode,
                Right = rightNode
            };
        }
        public Node Concatenate(Node p, Node q)
        {
            //Setting is p and q are null
            if (p == null) return q;
            if (q == null) return p;
            //Check to reblance
            if ((p.Length + q.Length) < p.Length)
            {
                Node tempp = p; tempp.Length = p.Length - 1;
                Node Left = Concatenate(p, tempp);
                Node Right = q;

                return new InternalNode(S.Length);
            }
            else
            {
                return new InternalNode(S.Length)
                {
                    Left = p,
                    Right = q
                };
            }
        }

        public Node Split(Node p)
        {
            //Making a divide line via root and point p, which is close to centre.

            //Creating 2 trees

            //Removing extra leaf nodes to balance
            return null;
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
            Node rt = new Node(); //No idea what replacement
            rt.Length = Root.Weight;
            rt.s = Convert.ToString(Root.Value);
            return rt = Rebalance(rt);
        }

        private Node Rebalance(Node node)
        {
            if(node is InternalNode internalNode)
            {
                Node left = Rebalance(internalNode.Left);
                Node right = Rebalance(internalNode.Right);

                return Concatenate(left, right);
            }

            //Returning
            return node;
            
        }
    }
}
