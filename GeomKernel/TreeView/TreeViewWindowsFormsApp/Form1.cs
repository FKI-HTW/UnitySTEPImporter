using System.Windows.Forms;
using BHTTreeView;

namespace TreeViewWindowsFormsApp
{
    public class Form1 : Form
    {

        private TreeViewNode<string> mViewNode;

        private void InitializeComponent()
        {
            SuspendLayout();
            TreeNode<string> testNode = new TreeNode<string>("Test", "Test", false);
            TreeNode<string> Child1 = testNode.AddChild("Gemuese", "Gemuese", false);
            TreeNode<string> Child11 = Child1.AddChild("Moehren", "Moehren", true);
            TreeNode<string> Child12 = Child1.AddChild("Erbsen", "Erbsen", true);
            TreeNode<string> Child13 = Child1.AddChild("Kohlrabi", "Kohlrabi", true);
            TreeNode<string> Child2 = testNode.AddChild("Fleisch", "Fleisch", false);
            TreeNode<string> Child21 = Child2.AddChild("Rind", "Rind", false);
            TreeNode<string> Child211 = Child21.AddChild("Roulade", "Roulade", true);
            TreeNode<string> Child212 = Child21.AddChild("Steak", "Steak", true);
            TreeNode<string> Child22 = Child2.AddChild("Gefluegel", "Gefluegel", false);
            TreeNode<string> Child221 = Child22.AddChild("Huhn", "Huhn", false);
            TreeNode<string> Child222 = Child22.AddChild("Pute", "Pute", false);
            TreeNode<string> Child2221 = Child222.AddChild("Steak", "Steak", true);
            TreeNode<string> Child2222 = Child222.AddChild("Keule", "Keule", true);
            TreeNode<string> Child3 = testNode.AddChild("Kartoffeln", "Kartoffeln", true);

            mViewNode = new TreeViewNode<string>(testNode, this);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        public Form1()
        {
            InitializeComponent();
        }
    }
}
