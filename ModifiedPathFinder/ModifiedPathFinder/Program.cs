// This project is made influenced from two source codes from the following links:
// https://www.geeksforgeeks.org/given-a-binary-tree-print-all-root-to-leaf-paths/
// https://www.geeksforgeeks.org/construct-binary-tree-string-bracket-representation/

using System;
using System.Collections.Generic;

namespace BinaryTreeTraverser
{
    public class Node
    {
        public string data;
        public Node left, right;

        public Node(string value)
        {
            data = value;
            left = right = null;
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

        public virtual void StorePathsRecur(Node node, string[] path, int pathLen, ref string str)
        {
            if (node == null)
                return;

            path[pathLen] = node.data;
            pathLen++;

            if (node.left == null && node.right == null)
                PrintArray(path, pathLen, ref str);
            else
            {
                StorePathsRecur(node.left, path, pathLen, ref str);
                StorePathsRecur(node.right, path, pathLen, ref str);
            }
        }

        public virtual void PrintArray(string[] ints, int len, ref string str)
        {
            for (int i = 0; i < len; i++)
                str += ints[i];
            str += ";";
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
            int value_digits = 0;
            Node root;

            while (str[si + value_digits] != '(' && str[si + value_digits] != ')')
                value_digits++;

            if ((str[si + value_digits] == '(' || str[si + value_digits] == ')') && si < (str.Length - 1))
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

                root = new Node(str[si].ToString());

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

            string str = "10(3(5(6222(4(10(11)())())(7(8(12)())()))())())(4(7(6(5(6)())(7(6)()))())())";
            string paths = "";
            Node root = tree.TreeFromString(str, 0, str.Length - 1);
            tree.StorePaths(root, ref paths);
            tree.PreOrder(root);
        }
    }
}
