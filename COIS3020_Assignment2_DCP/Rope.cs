
using Microsoft.VisualBasic;
using System.Text;

using static COIS3020_Assignment2_DCP.Rope.Node;

namespace COIS3020_Assignment2_DCP
{

    public class Rope
    {
        public Node Root;
        //private RopeNode Root { get; set; }
        private int SubstringMaxLength = 10;
        //Constructor
        public Rope(string S)
        {
            Root = Build(S);
        }

        private Node Build(string S)
        {
            // return empty node if S is empty
            if (S.Length <= 0) return new Node();

            // Base case for recursion: when S length is 10 or less
            if (S.Length <= SubstringMaxLength)
            {
                return new Node(S);
            }

            // Split S into roughly two equal parts
            List<Node> nodes = new List<Node>();
            for (int i = 0; i < S.Length;)
            {
                int length = Math.Min(SubstringMaxLength, S.Length - i);
                nodes.Add(new Node(S.Substring(i, length)));
                i += length;
            }

            // Recursively combine nodes into a balanced tree
            while (nodes.Count > 1)
            {
                List<Node> combinedNodes = new List<Node>();
                for (int i = 0; i < nodes.Count; i += 2)
                {
                    if (i + 1 < nodes.Count)
                    {
                        Node newNode = new Node(null)
                        {
                            Left = nodes[i],
                            Right = nodes[i + 1],
                            Weight = nodes[i].Weight + nodes[i + 1].Weight
                        };
                        //nodes[i].Parent = newNode;
                        //nodes[i + 1].Parent = newNode;
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

        private Node Concatenate(Node p, Node q)
        {
            if (p == null) return q;
            if (q == null) return p;
            if (p == null && q == null) return null;

            Node root = new Node();
            root.Left = p;
            root.Right = q;
            root.Weight = p.Weight + q.Weight;
            return root;
        }

        //Split the rope with root p at index i and return the root of the right subtree
        private (Node, Node) Split(Node p, int i)
        {
            // traverse to the node storing index i
            int index;
            Node tempNode;
            List<Node> path;
            (index, tempNode, path) = TravelToIndex(p, i);

            // left of the node being splited
            Node leftNode = new Node();
            leftNode.PartOfString = tempNode.PartOfString.Substring(0, index);
            leftNode.Weight = leftNode.PartOfString.Length;
            // right of the node being splited
            Node rightNode = new Node();
            rightNode.PartOfString = tempNode.PartOfString.Substring(index);
            rightNode.Weight = rightNode.PartOfString.Length;


            //Node a = new Node();
            //Node a2 = a;
            //List<Node> aList = new List<Node>();
            //Node b = new Node();
            //Node b2 = b;
            //List<Node> bList = new List<Node>();
            //for (int j =0; j < path.Count; j ++)
            //{
            //    if (path[j + 1].Equals(path[j].Left))
            //    {
            //        b.Right = path[j].Right;
            //    }
            //}

            Node leftNodeParent = new Node();
            leftNodeParent.Left = leftNode;
            leftNodeParent.Weight = leftNode.Weight;

            Node rightNodeParent = new Node();
            rightNodeParent.Right = rightNode;
            rightNodeParent.Weight = rightNode.Weight;
            for (int j = path.Count - 2; j > 0; j--)
            {
                Node leftNodeParent2 = new Node();
                Node rightNodeParent2 = new Node();
                if (path[j].Equals(path[j - 1].Left))
                {
                    //if (path[j + 1].Equals(path[j].Right))
                    //{
                    leftNodeParent2.Left = path[j].Left;
                    leftNodeParent2.Right = leftNodeParent;
                    leftNodeParent2.Weight = (leftNodeParent2.Right?.Weight ?? 0) + (leftNodeParent2.Left?.Weight ?? 0);

                    rightNodeParent2.Right = rightNodeParent;
                    rightNodeParent2.Weight = (rightNodeParent2.Right?.Weight ?? 0) + (rightNodeParent2.Left?.Weight ?? 0);
                    //} else
                    //{
                    //    leftNodeParent2.Left = leftNodeParent;
                    //}

                }
                else
                {
                    leftNodeParent2.Left = leftNodeParent;
                    leftNodeParent2.Weight = (leftNodeParent2.Right?.Weight ?? 0) + (leftNodeParent2.Left?.Weight ?? 0);

                    rightNodeParent2.Left = rightNodeParent;
                    rightNodeParent2.Right = path[j].Right;
                    rightNodeParent2.Weight = (rightNodeParent2.Right?.Weight ?? 0) + (rightNodeParent2.Left?.Weight ?? 0);
                }
                leftNodeParent = leftNodeParent2;
                rightNodeParent = rightNodeParent2;
            }

            if (path[1].Equals(path[0].Left))
            {
                Node temp2 = new Node();
                temp2.Left = leftNodeParent;
                temp2.Weight = leftNodeParent.Weight;

                Node temp3 = new Node();
                temp3.Left = rightNodeParent;
                temp3.Right = path[0].Right;
                temp3.Weight = (temp3.Right?.Weight ?? 0) + (temp3.Left?.Weight ?? 0);

                leftNodeParent = temp2;
                rightNodeParent = temp3;
            }
            Node finalLeftNode = removeDuplicateParent(leftNodeParent, leftNodeParent.Weight - 1);
            Node finalRightNode = removeDuplicateParent(rightNodeParent, 0);

            return (finalLeftNode, finalRightNode);
        }

        // OPTIMIZATION:
        // After a Split, compress the path back to the root to ensure that binary tree is full, i.e. each non-leaf node has two non-empty children 
        // for path traverse from node root to index i
        // remove parent with weight same with its child
        private Node removeDuplicateParent(Node node, int i)
        {
            // get the path to index i
            (_, _, List<Node> a) = TravelToIndex(node, i);
            for (int j = a.Count - 1; j >= 2; j--)
            {
                // parent have the same weight with its left/right child
                if (a[j].Weight == a[j - 1].Weight)
                {
                    if (a[j - 1].Equals(a[j - 2].Left))
                    {
                        a[j - 2].Left = a[j];
                        a[j - 1] = a[j];
                    }
                    else
                    {
                        a[j - 2].Right = a[j];
                        a[j - 1] = a[j];
                    }
                }
            }
            // if root weight == root left/right child weight
            if (a[0].Weight == a[1].Weight)
            {
                return a[1];
            }
            return a[0];
        }


        public void Insert(string S, int i)
        {
            // validate data
            if (i < 0 || i > Root.Weight)
            {
                throw new ArgumentOutOfRangeException("Index " + i + " is out of range");
            }
            int index;
            Node nodeToBeInsert;
            List<Node> path;

            (Node leftTree, Node rightTree) = Split(Root, i);
            Node insertNode = Build(S);

            Node tempLeftTree = Concatenate(leftTree, insertNode);
            Root = Concatenate(tempLeftTree, rightTree);
        }

        // Delete the substring S[i,j]
        public void Delete(int i, int j)
        {
            // validate data
            if (i < 0 || i > Root.Weight || j < 0 || j > Root.Weight)
            {
                throw new ArgumentOutOfRangeException("Index i " + i + " or j " + j + " is out of range");
            }
            // split the root at i to get leftTree and rightTree
            (Node leftTree, Node rightTree) = Split(Root, i);
            // split the rightTree at j - i to get the remainning rightTree
            (_, Node rightTree2) = Split(rightTree, j - i);
            Root = Concatenate(leftTree, rightTree2);
        }

        // given i, find the leaf node
        private (int, Node, List<Node>) TravelToIndex(Node node, int i)
        {
            // throw excpetion if out of range
            if (i < 0 || i > node.Weight)
            {
                throw new ArgumentOutOfRangeException("i", "Index is out of range");
            }
            // store path of node travelled
            List<Node> path = new List<Node>();
            Node tempRoot = node;
            // continue until leaf node is reached
            while (tempRoot.Left != null || tempRoot.Right != null)
            {
                // turn left if weight of the left child > i
                if (tempRoot.Left != null && tempRoot.Left.Weight > i)
                {
                    path.Add(tempRoot);
                    tempRoot = tempRoot.Left;

                }
                else
                {
                    // minus i the weight of left child and turn right
                    i -= tempRoot.Left?.Weight ?? 0;
                    path.Add(tempRoot);
                    tempRoot = tempRoot.Right;

                }
            }
            path.Add(tempRoot);
            Console.WriteLine(tempRoot.PartOfString[i]);
            // return index of i in that substring and node containing it
            return (i, tempRoot, path);
        }

        // get the substring of rope start at i and end at j
        public string Substring(int i, int j)
        {
            if (i + j >= Root.Weight)
            {
                Console.WriteLine("i + j >= length of rope is not allowed");
                return "";
            }

            // variable to store the result to be return
            string tempString = "";

            //loop until the entire substring is formed
            while (j >= i)
            {
                int index;
                Node node;
                // traverse to node storing index i
                (index, node, _) = TravelToIndex(Root, i);
                // if i + node.PartOfString.Length < j + 1, need to get next node after this
                if (node.PartOfString.Substring(index).Length < j - i + 1)
                {
                    // increment i by length of string
                    i += node.PartOfString.Substring(index).Length;
                    // add the string in the variable being returned
                    tempString += node.PartOfString.Substring(index);
                }
                // end of loop, the last node needed is got
                else
                {
                    // add the remaining required substring to the variable being returned
                    tempString += node.PartOfString.Substring(index, j - i + 1);
                    i = j + 1;
                }
                //tempString += node.PartOfString.Substring(index);
            }
            return tempString;
        }

        // find starting index of S in the rope
        public int Find(string S)
        {
            // check if S.length > 0
            if (S.Length == 0)
            {
                return -1;
            }
            // get the index of S[0] in the rope
            int indexOfFirstChar = this.IndexOf(S[0]);
            while (indexOfFirstChar != -1)
            {
                // get substring start at indexOfFirstChar with length S.length
                string candidate = this.Substring(indexOfFirstChar, indexOfFirstChar + S.Length);
                // if match return indexOfFirstChar
                if (candidate.Equals(S))
                {
                    return indexOfFirstChar;
                }
                indexOfFirstChar = this.IndexOf(S[0], indexOfFirstChar + 1);
            }
            //string entireString = Root.ToString();
            //int index = entireString.IndexOf(S);
            return -1;
        }

        // find character at index i
        public char CharAt(int i)
        {
            // check is i valid
            if (i < 0 || i > Root.Weight)
            {
                throw new ArgumentOutOfRangeException("i", "Index is out of range");
            }

            Node tempRoot = Root;
            // traverse to leaf node
            while (tempRoot.Left != null || tempRoot.Right != null)
            {
                // turn left if weight of left child < i
                if (tempRoot.Left != null && tempRoot.Left.Weight > i)
                {
                    tempRoot = tempRoot.Left;

                }
                else
                {
                    // minus i by weight of left child and turn right
                    i -= tempRoot.Left?.Weight ?? 0;
                    tempRoot = tempRoot.Right;

                }
            }
            // return the character at index i
            return tempRoot.PartOfString[i];
        }

        // find index of character in rope
        public int IndexOf(char c)
        {
            int i = 0;
            // loop for entire string
            while (i < Root.Weight)
            {
                int index;
                Node tempNode;
                // travel to node by i
                (index, tempNode, _) = TravelToIndex(Root, i);
                // compare string in tempNode one by one to check is = c
                for (int j = index; j < tempNode.PartOfString.Length; j++)
                {
                    // return if match
                    if (tempNode.PartOfString[j].Equals(c))
                    {
                        return i + j;
                    }
                }
                // increment i to search for next node
                i += tempNode.PartOfString.Length;
            }
            return -1;
        }

        // find index of character in rope after index
        private int IndexOf(char c, int startingIndex)
        {
            int i = startingIndex;
            // loop for entire string
            while (i < Root.Weight)
            {
                int index;
                Node tempNode;
                // travel to node by i
                (index, tempNode, _) = TravelToIndex(Root, i);
                // compare string in tempNode one by one to check is = c
                for (int j = index; j < tempNode.PartOfString.Length; j++)
                {
                    // return if match
                    if (tempNode.PartOfString[j].Equals(c))
                    {
                        return i + j;
                    }
                }
                // increment i to search for next node
                i += tempNode.PartOfString.Length;
            }
            return -1;
        }

        // reverse the string of the rope
        public void Reverse()
        {
            // do nothing if the rope is empty
            if (Root.Weight == 0) return;
            String S = Root.ToString();
            // extract the entire string
            char[] charArray = S.ToString().ToCharArray();
            // reverse the string
            Array.Reverse(charArray);
            // build a new rope for Root
            Root = Build(charArray.ToString());
        }

        // return the length of rope
        public int Length()
        {
            return Root.Weight;
        }

        // return string of rope
        public override string ToString()
        {
            string output = "";
            ToString(Root, ref output);
            return output;
        }

        // return string of rope
        private void ToString(Node node, ref string output)
        {
            // return if node is null
            if (node == null)
            {
                return;
            }
            // if is leaf node, extract its string and add to output
            if (node.Left == null && node.Right == null)
            {
                //Console.WriteLine(node.PartOfString);
                output += node.PartOfString;
                return;
            }

            // inorder traveral
            ToString(node.Left, ref output);
            ToString(node.Right, ref output);
            return;
        }

        private Node Rebalance()
        {
            String entireString = Root.ToString();
            Node balancedNode = Build(entireString);
            return balancedNode;
        }

        public void printRope()
        {
            PrintRope(Root, "");
        }
        private void PrintRope(Node node, string indent)
        {
            if (node == null)
            {
                return;
            }

            PrintRope(node.Right, indent + "  ");

            Console.WriteLine(indent + node.ToString());

            PrintRope(node.Left, indent + "  ");
        }

        public class Node
        {
            public string PartOfString { get; set; }
            public Node Left;
            public Node Right;
            //public Node Parent;
            public int Weight;

            public Node()
            {

            }

            public Node(string s)
            {
                this.PartOfString = s;
                if (s != null)
                {
                    this.Weight = s.Length;
                }
            }
        }

    }
}
