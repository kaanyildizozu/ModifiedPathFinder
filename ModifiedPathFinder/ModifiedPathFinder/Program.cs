// This project is made influenced from two source codes from the following links:
// https://www.geeksforgeeks.org/given-a-binary-tree-print-all-root-to-leaf-paths/
// https://www.geeksforgeeks.org/construct-binary-tree-string-bracket-representation/

using System;
using System.Collections.Generic;

namespace BinaryTreeTraverser
{
    public class Node
    {
        public int data;
        public Node left, right;

        public Node(int item)
        {
            data = item;
            left = right = null;
        }
    }

    public class BinaryTree
    {
        public Node root;

        public virtual void PrintPaths(Node node)
        {
            int[] path = new int[100];
            PrintPathsRecur(node, path, 0);
        }

        public virtual void PrintPathsRecur(Node node, int[] path, int pathLen)
        {
            if (node == null)
                return;

            path[pathLen] = node.data;
            pathLen++;

            if (node.left == null && node.right == null)
                printArray(path, pathLen);
            else
            {
                PrintPathsRecur(node.left, path, pathLen);
                PrintPathsRecur(node.right, path, pathLen);
            }
        }

        public virtual void printArray(int[] ints, int len)
        {
            for (int i = 0; i < len; i++)
                Console.Write(ints[i] + " ");

            Console.WriteLine("");
        }

        public virtual void PreOrder(Node node)
        {
            if (node == null)
                return;
            Console.Write(node.data);
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
            Node root;

            if (str[si + 1] != '(' && str[si + 1] != ')' && si < (str.Length - 1))
            {

                char[] chars = { str[si], str[si + 1] };
                string digits = new string(chars);
                root = new Node(System.Convert.ToInt32(digits));

                if (si + 2 <= ei && str[si + 2] == '(')
                    index = FindIndex(str, si + 2, ei);

                if (index != -1)
                {
                    root.left = TreeFromString(str, si + 3, index - 1);
                    root.right = TreeFromString(str, index + 2, ei - 1);
                }
            }
            else
            {

                root = new Node(str[si] - '0');

                if (si + 1 <= ei && str[si + 1] == '(')
                    index = FindIndex(str, si + 1, ei);

                if (index != -1)
                {
                    root.left = TreeFromString(str, si + 2, index - 1);
                    root.right = TreeFromString(str, index + 2, ei - 1);
                }
            }


            return root;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BinaryTree tree = new BinaryTree();
            //tree.root = new Node(10);
            //tree.root.left = new Node(8);
            //tree.root.right = new Node(2);
            //tree.root.left.left = new Node(3);
            //tree.root.left.right = new Node(5);
            //tree.root.right.left = new Node(2);
            //tree.PrintPaths(tree.root);

            string str = "1(3(5(6(4(10(11)())())(7(8(12)())()))())())(4(7(6(5(6)())(7(6)()))())())";
            Node root = tree.TreeFromString(str, 0, str.Length - 1);
            tree.PreOrder(root);
        }
    }
}
