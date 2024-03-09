using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static COIS3020_Assignment2_DCP.Rope.Node;

namespace COIS3020_Assignment2_DCP {

    public class RopeNode
    {
        public string Value { get; set; } // For leaf nodes, this holds the substring
        public int Weight { get; set; } // Holds the total length of all characters in the left subtree
        public RopeNode Left { get; set; }
        public RopeNode Right { get; set; }
        public bool IsLeaf() => Value != null;

        public RopeNode(string value)
        {
            Value = value;
            Weight = value != null ? value.Length : 0;
            Left = Right = null;
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
            for (int i = 0; i < S.Length; )
            {
                int length = Math.Min(10, S.Length - i);
                nodes.Add(new RopeNode(S.Substring(i, length)));
                i += length;
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
            if (i < 0 || i > S.Length)
            {
                throw new ArgumentOutOfRangeException("i", "Index is out of range");
            }

            //making new node using string S
            Rope R1 = new Rope(S);

            // Split nodes if necessary to create space for the new substring
            SplitNode splitResult = SplitNode(Root, i);
            RopeNode R2 = splitResult.Left;
            RopeNode R3 = splitResult.Right;

            // Concatenate R1, R2, and R3 to form the new rope
            RopeNode newRoot = Concatenate(ConcatenateNodes(R1.Root, R2), R3);

            // Update the root of the rope
            Root = newRoot;
        }
        public void Delete(int i, int j)
        {
            if (i < 0 || j >= S.Length || i > j)
            {
                throw new ArgumentOutOfRangeException("i or j", "Indices are out of range");
            }

            // Split the current rope at indices i - 1 and j to give ropes R1, R2, and R3
            SplitNodeResult splitResult1 = SplitNode(Root, i);
            SplitNodeResult splitResult2 = SplitNode(splitResult1.Right, j - i + 1);

            RopeNode R1 = splitResult1.Left;
            RopeNode R2 = splitResult2.Left;
            RopeNode R3 = splitResult2.Right;

            // Concatenate R1 and R3
            RopeNode newRoot = ConcatenateNodes(R1, R3);

            // Update the root of the current rope
            Root = newRoot;
        }

        //Substrings
        public string subString(int i, int j)
        {
            if (i < 0 || j >= S.Length || i > j)
            {
                throw new ArgumentOutOfRangeException("i or j", "Indices are out of range");
            }

            // Traverse the rope to find the substring
            StringBuilder substringBuilder = new StringBuilder();
            TraverseForSubstring(Root, i, j, substringBuilder);

            // Return the constructed substring
            return substringBuilder.ToString();
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
            if (i < 0 || i >= S.Length)
            {
                throw new ArgumentOutOfRangeException("i", "Index is out of range");
            }

            // Return the character at index i
            return S[i];
        }

        public void Reverse()
        {
            //Possibly use for loop to find
            char[] charArray = S.ToCharArray();
            Array.Reverse(charArray);
            S = new string(charArray);
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
                PrintRope(node.Left);
                Console.WriteLine("Right Child:");
                PrintRope(node.Right);
            }
        }

        //Node
        public class Node
        {
            public string s { get; set; }
            public int Length { get; set; }
            public RopeNode Left { get; set; }
            public RopeNode Right { get; set; }
            public Node()
            {
                this.s = s;
                this.Length = 0;
                this.Left = Left;
                this.Right = Right;
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

        // Split
        private RopeNode Split(RopeNode node, int index, out RopeNode leftRoot)
        {
            if (node == null)
            {
                leftRoot = null;
                return null;
            }

            if (node.IsLeaf())
            {
                if (index >= node.Weight) // Entire node is in the left part
                {
                    leftRoot = node;
                    return null; // Nothing on the right
                }
                else if (index == 0) // Entire node is in the right part
                {
                    leftRoot = null;
                    return node;
                }
                else
                {
                    // Splitting a leaf node
                    string leftData = node.Data.Substring(0, index);
                    string rightData = node.Data.Substring(index);

                    leftRoot = new RopeNode(leftData);
                    return new RopeNode(rightData);
                }
            }

            if (index < node.Weight)
            {
                // Split index is within the left subtree
                RopeNode tempRight;
                RopeNode newLeft = Split(node.Left, index, out tempRight);
                leftRoot = newLeft;

                if (tempRight == null)
                {
                    return node.Right; // Right subtree remains unchanged
                }
                else
                {
                    node.Left = tempRight; // Update left child of the node
                                           // Recalculate weight for the current node as it may have changed
                    node.Weight = CalculateTotalWeight(tempRight);
                    return node; // Node now represents the right part
                }
            }
            else
            {
                // Split index is within the right subtree or exactly at the weight
                index -= node.Weight; // Adjust index relative to the right subtree
                RopeNode tempLeft;
                RopeNode newRight = Split(node.Right, index, out tempLeft);

                if (tempLeft == null)
                {
                    leftRoot = node; // Left part includes the current node and its left subtree
                    return newRight; // Right part is unchanged
                }
                else
                {
                    // Create a new node for the left part including the original left subtree
                    // and the left part of the split right subtree
                    leftRoot = new RopeNode(null)
                    {
                        Left = node.Left,
                        Right = tempLeft,
                        Weight = node.Weight // Weight remains the same for the left part
                    };
                    return newRight; // The right part of the split right subtree
                }
            }
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

        public Node CompressPath(Node node)
        {
            if (node is LeafNode || node == null)
            {
                return node;
            }
            node.Left = CompressPath(node.Left);
            node.Right = CompressPath(node.Right);
            if (node.Left == null && node.Right != null) //Check if right child exists
                return node.Right;
            else if (node.Left != null && node.Right == null) //Check if left child exists
                return node.Left;
            return node;
        }

        private Node CombineSiblings(Node node)
        {
            if(node is InternalNode internalNode)
            {
                if(internalNode.Left != null && internalNode.Right != null && 
                    (internalNode.Left.Length + internalNode.Right.Length <= 5))
                {
                    Node combinedNode = new InternalNode()
                    {
                        Left = internalNode.Left,
                        Right = internalNode.Right
                    };

                    return combinedNode;
                }
                else
                {
                    internalNode.Left = CombineSiblings(internalNode.Left);
                    internalNode.Right = CombineSiblings(internalNode.Right);
                }
            }
            return node;
        }

    }
}
