using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHTTreeView;

namespace TreeViewConsoleApp
{

    class Program
    {
        static void ShowAll(TreeNode<string> node)
        {
            string text = "";
            for (int i = 0; i < node.Depth; ++i)
                text += "  ";
            if (node.Atomic)
                text += "*";
            else
                text += "+";
            text += node.Name;
            Console.WriteLine(text);

            foreach (var child in node.Children)
            {
                ShowAll(child);
            }

        }

        static void Test1()
        {
            // Fill tree
            TreeNode<string>  tree = new TreeNode<string>("Test", "Test", false);
            TreeNode<string> Child1 = tree.AddChild("Gemuese", "Gemuese", false);
            TreeNode<string> Child11 = Child1.AddChild("Moehren", "Moehren", true);
            TreeNode<string> Child12 = Child1.AddChild("Erbsen", "Erbsen", true);
            TreeNode<string> Child13 = Child1.AddChild("Kohlrabi", "Kohlrabi", true);
            TreeNode<string> Child2 = tree.AddChild("Fleisch", "Fleisch", false);
            TreeNode<string> Child21 = Child2.AddChild("Rind", "Rind", false);
            TreeNode<string> Child211 = Child21.AddChild("Roulade", "Roulade", true);
            TreeNode<string> Child212 = Child21.AddChild("Steak", "Steak", true);
            TreeNode<string> Child22 = Child2.AddChild("Gefluegel", "Gefluegel", false);
            TreeNode<string> Child221 = Child22.AddChild("Huhn", "Huhn", false);
            TreeNode<string> Child222 = Child22.AddChild("Pute", "Pute", false);
            TreeNode<string> Child2221 = Child222.AddChild("Steak", "Steak", true);
            TreeNode<string> Child2222 = Child222.AddChild("Keule", "Keule", true);
            TreeNode<string> Child3 = tree.AddChild("Kartoffeln", "Kartoffeln", true);

            // Show tree
            ShowAll(tree);
        }

        static void Main(string[] args)
        {
            Test1();
        }
    }
}
