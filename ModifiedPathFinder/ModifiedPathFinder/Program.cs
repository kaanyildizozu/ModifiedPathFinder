// This project is made influenced from two source codes from the following links:
// https://www.geeksforgeeks.org/given-a-binary-tree-print-all-root-to-leaf-paths/
// https://www.geeksforgeeks.org/construct-binary-tree-string-bracket-representation/

using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTreeTraverser
{
    public class Node
    {
        public string string_data;
        public int int_data;
        public Node left, right, skip;
        public bool isOptional = false;

        public Node(string value)
        {
            string_data = value;
            left = right = skip = null;
        }

        public Node(int value)
        {
            int_data = value;
            left = right = skip = null;
        }

        public Node(string value, bool optionalCheck)
        {
            string_data = value;
            left = right = skip = null;
            isOptional = optionalCheck;
        }

        public Node(int value, bool optionalCheck)
        {
            int_data = value;
            left = right = skip = null;
            isOptional = optionalCheck;
        }
    }

    public class BinaryTree
    {
        public Node root;

        public virtual void StorePaths(Node node, ref string str)
        {
            string[] path = new string[100];
            StorePathsRecur(node, path, 0, ref str);
        }

        public virtual void StoreIntPaths(Node node, ref List<int[]> possible_paths)
        {
            int[] path = new int[100];
            StoreIntPathsRecur(node, path, 0, ref possible_paths);
        }

        public virtual void StorePathsRecur(Node node, string[] path, int pathLen, ref string str)
        {
            if (node == null)
                return;
            
            if (node.isOptional)
            {
                path[pathLen] = "";
                pathLen++;

                if (node.left == null && node.right == null)
                    PrintAndStorePaths(path, pathLen, ref str);
                else
                {
                    StorePathsRecur(node.left, path, pathLen, ref str);
                    StorePathsRecur(node.right, path, pathLen, ref str);
                }
            }
            path[pathLen] = node.string_data;
            pathLen++;

            if (node.left == null && node.right == null)
                PrintAndStorePaths(path, pathLen, ref str);
            else
            {
                StorePathsRecur(node.left, path, pathLen, ref str);
                StorePathsRecur(node.right, path, pathLen, ref str);
            }
        }

        public virtual void StoreIntPathsRecur(Node node, int[] path, int pathLen, ref List<int[]> possible_paths)
        {
            if (node == null)
                return;

            if (node.isOptional)
            {
                if (node.left == null && node.right == null)
                {
                    PrintAndStoreIntPaths(path, pathLen);
                    possible_paths.Add(path);
                }
                else
                {
                    StoreIntPathsRecur(node.left, path, pathLen, ref possible_paths);
                    StoreIntPathsRecur(node.right, path, pathLen, ref possible_paths);
                }
            }
            path[pathLen] = node.int_data;
            pathLen++;

            if (node.left == null && node.right == null)
            {
                PrintAndStoreIntPaths(path, pathLen);
                possible_paths.Add(path);
            }
            else
            {
                StoreIntPathsRecur(node.left, path, pathLen, ref possible_paths);
                StoreIntPathsRecur(node.right, path, pathLen, ref possible_paths);
            }
        }

        public virtual void PrintAndStoreIntPaths(int[] ints, int len)
        {
            for (int i = 0; i < len; i++)
            {
                if(ints[i] == null)
                    Console.Write(ints[i]);
                Console.Write(ints[i] + " ");
            }
            Console.WriteLine("");
        }

        public virtual void PrintAndStorePaths(string[] ints, int len, ref string str)
        {
            for (int i = 0; i < len; i++)
            {
                if (string.IsNullOrEmpty(ints[i]))
                    str += ints[i];
                else
                {
                    str += ints[i];
                    Console.Write(ints[i] + " ");
                }
            }
            str += ";";
            Console.WriteLine("");
        }

        public virtual void PreOrder(Node node)
        {
            if (node == null)
                return;
            Console.Write(node.int_data);
            PreOrder(node.left);
            PreOrder(node.right);
        }

        public virtual int FindIndex(string str, int si, int ei)
        {
            if (si > ei)
                return -1;

            Stack<char> parenthesisStack = new Stack<char>();
            for (int i = si; i < ei; i++)
            {
                if (str[i] == '(')
                    parenthesisStack.Push(str[i]);

                else if (str[i] == ')')
                {
                    if (parenthesisStack.Peek() == '(')
                    {
                        parenthesisStack.Pop();

                        if (parenthesisStack.Count == 0)
                            return i;
                    }
                }
            }
            return -1;
        }

        public virtual Node TreeFromString(string str, int si, int ei)
        {
            if (si > ei)
                return null;

            int index = -1;
            int value_digits = 0;
            Node root;

            while (str[si + value_digits] != '(' && str[si + value_digits] != ')' && str[si + value_digits] != '*')
                value_digits++;

            if (str[si + value_digits] == '*')
            {
                string digits = str.Substring(si, value_digits); ;
                root = string.IsNullOrEmpty(digits) ? new Node("") : new Node(digits, true);

                if (si + value_digits + 2 <= ei && str[si + value_digits + 1] == '(')
                    index = FindIndex(str, si + value_digits + 1, ei);

                if (index != -1)
                {
                    root.left = TreeFromString(str, si + value_digits + 2, index - 1);
                    root.right = TreeFromString(str, index + 2, ei - 1);
                }
            }
            else if ((str[si + value_digits] == '(' || str[si + value_digits] == ')') && si < (str.Length - 1))
            {
                string digits = str.Substring(si, value_digits);
                root = string.IsNullOrEmpty(digits) ? new Node("") : new Node(digits);

                if (si + value_digits + 1 <= ei && str[si + value_digits] == '(')
                    index = FindIndex(str, si + value_digits, ei);

                if (index != -1)
                {
                    root.left = TreeFromString(str, si + value_digits + 1, index - 1);
                    root.right = TreeFromString(str, index + 2, ei - 1);
                }
            }
            else
            {
                string digits = str.Substring(si, value_digits);
                root = string.IsNullOrEmpty(digits) ? new Node("") : new Node(digits);

                if (si + value_digits + 1 <= ei && str[si + value_digits] == '(')
                    index = FindIndex(str, si + value_digits, ei);

                if (index != -1)
                {
                    root.left = TreeFromString(str, si + value_digits + 1, index - 1);
                    root.right = TreeFromString(str, index + value_digits + 1, ei - 1);
                }
            }


            return root;
        }

        public virtual Node CreateBinaryTree(int[] nodes, int[] parents,int[] optionalNodes)
        {
            // For now, I am assuming that the root node can not be optional.
            Node root = InitializeBinaryTree(nodes[0]);
            bool createdSuccessfully = false;

            for (int i = 1; i < nodes.Length; i++)
            {
                createdSuccessfully = InsertNodeToBinaryTree(root, nodes[i], parents[i], optionalNodes.Contains(nodes[i]));    
            }
            
            return createdSuccessfully ? root : null;
        }

        public virtual bool InsertNodeToBinaryTree(Node parent_root, int node, int parent, bool isOptional)
        {
            bool isInserted = false;
            if (parent_root.int_data == parent)
            {
                if (parent_root.left == null && parent_root.right != null)
                {
                    parent_root.left = new Node(node, isOptional);
                    isInserted = true;
                }
                else if (parent_root.left != null && parent_root.right == null)
                {
                    parent_root.right = new Node(node, isOptional);
                    isInserted = true;
                }
                else if (parent_root.left == null && parent_root.right == null)
                {
                    parent_root.left = new Node(node, isOptional);
                    isInserted = true;
                }
                else
                    Console.Write("No room for third childs!!!");
            }
            else
            {
                if (parent_root.left != null)
                {
                    if (!(isInserted = InsertNodeToBinaryTree(parent_root.left, node, parent, isOptional)) && parent_root.right != null)
                        isInserted = InsertNodeToBinaryTree(parent_root.right, node, parent, isOptional);
                }
                else if (parent_root.right != null)
                {
                    isInserted = InsertNodeToBinaryTree(parent_root.right, node, parent, isOptional);
                }
                else
                {
                    //Console.Write("Parentless child :(" + node.ToString());
                    //Console.WriteLine("");
                }
            }
            return isInserted;
        }

        public virtual Node InitializeBinaryTree(int root_value)
        {
            return new Node(root_value);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch kronometer = new Stopwatch();
            kronometer.Start();
            BinaryTree tree = new BinaryTree();

            int[] nodes = new int[] { 1, 2, 13, 4, 3, 5, 11, 14, 6, 7, 12, 15, 9, 8, 16, 10, 22, 18, 17, 19, 20, 21, 24, 23, 25, 26 };
            int[] parents = new int[] { -1, 1, 1, 2, 2, 4, 3, 13, 5, 5, 11, 14, 6, 7, 15, 9, 9, 16, 16, 18, 19, 10, 22, 22, 17, 25 };
            int[] optional_nodes = new int[] { 4, 3, 6, 7, 10, 23, 8, 14, 15, 18, 17, 25, 26 };

            List<int[]> possible_paths = new List<int[]>();
            Node root = tree.CreateBinaryTree(nodes, parents, optional_nodes);
            tree.StoreIntPaths(root, ref possible_paths);

            kronometer.Stop();
            Console.Write("Arrays method: " + kronometer.ElapsedMilliseconds + "TotalPaths: " + possible_paths.Count);

            Console.WriteLine("");

            kronometer.Reset();
            kronometer.Start();
            BinaryTree tree2 = new BinaryTree();
            string str = "1(2(4*(5(6*(9(10*()(21))(22(24)(23*)))())(7*()(8*)))())(3*()(11(12)())))(13()(14*(15*()(16(18*(19(20)())())(17*(25*(26*)())())))()))";
            Node root2 = tree.TreeFromString(str, 0, str.Length - 1);            
            string paths = "";
            tree.StorePaths(root2, ref paths);
            kronometer.Stop();
            Console.Write("String Method: " + kronometer.ElapsedMilliseconds);

        }
    }
}
