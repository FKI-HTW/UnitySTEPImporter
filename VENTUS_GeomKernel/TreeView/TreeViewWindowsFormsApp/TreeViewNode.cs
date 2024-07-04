using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using BHTTreeView;

namespace TreeViewWindowsFormsApp
{
    public class TreeViewNode<T>
    {
        private Label mLabel = null;
        private Button mButton = null;
        private TreeNode<T> mNodeRef = null;
        private Form mFormRef = null;
        private TreeViewNode<T> mParentRef = null;
        private readonly List<TreeViewNode<T>> mChildren = new List<TreeViewNode<T>>();
        private bool mUnfold = false;
        private int mChildIndex = -1;
        private int mRowIndex = 0;

        private static TreeViewNode<T> mActiveNodeRef = null;

        private const int mBtnSize = 25;

        public TreeViewNode(TreeNode<T> node, Form form)
        {
            InititializeNode(node, form);
            int lastRowIndex = mRowIndex;
            Uncover(ref lastRowIndex);
        }

        private TreeViewNode(TreeNode<T> node, Form form, TreeViewNode<T> parent, int index)
        {
            mParentRef = parent;
            mChildIndex = index;
            InititializeNode(node, form);
        }
        private void InititializeNode(TreeNode<T> node, Form form)
        {
            mNodeRef = node;
            mFormRef = form;

            mLabel = new Label();
            mLabel.AutoSize = true;
            mLabel.Text = node.Name;
            mLabel.Name = node.Name;
            mLabel.Height = mBtnSize;
            mLabel.Visible = false;
            mLabel.Click += Label_Click;
            form.Controls.Add(mLabel);

            if (!node.Atomic)
            {
                mButton = new Button();
                mButton.Size = new System.Drawing.Size(mBtnSize, mBtnSize);
                mButton.Name = node.Name;
                mButton.Text = mUnfold ? "-" : "+";
                mButton.Enabled = !(node.Children.Count == 0);
                mButton.Visible = false;
                mButton.Click += Button_Click;
                form.Controls.Add(mButton);

                for (int iChild = 0; iChild < node.Children.Count; ++iChild)
                    mChildren.Add(new TreeViewNode<T>(node.Children[iChild], form, this, iChild));
            }
        }

        private void Label_Click(System.Object sender, System.EventArgs e)
        {
            mLabel.BackColor = Color.LightBlue;
            if (mActiveNodeRef != null)
                mActiveNodeRef.mLabel.BackColor = mFormRef.BackColor;
            mActiveNodeRef = this;
        }

        private void Button_Click(System.Object sender, System.EventArgs e)
        {
            // toggle visibility of children
            mUnfold = !mUnfold;
            mButton.Text = mUnfold ? "-" : "+";

            int lastRowIndex = mRowIndex;
            if (mUnfold)
            {
                foreach (var child in mChildren)
                {
                    lastRowIndex++;
                    child.Uncover(ref lastRowIndex);
                }
            }
            else
            {
                foreach (var child in mChildren)
                {
                    child.Cover();
                }
            }

            // adapt location of all nodes below the current one
            UpdateAllNext(lastRowIndex);
            mFormRef.Refresh();
        }

        private void Uncover(ref int lastRowIndex)
        {
            UpdateLocation(lastRowIndex);
            UpdateVisibility(true);
            if (mUnfold)
            {
                foreach (var child in mChildren)
                {
                    lastRowIndex++;
                    child.Uncover(ref lastRowIndex);
                }
            }
        }

        private void Cover()
        {
            UpdateVisibility(false);
            foreach (var child in mChildren)
                child.Cover();
        }

        private void UpdateAllNext(int lastRowIndex)
        {
            if (mParentRef != null)
            {
                mParentRef.UpdateChildren(mChildIndex + 1, ref lastRowIndex);
                mParentRef.UpdateAllNext(lastRowIndex);
            }
        }

        private void UpdateChildren(int childIndexStart, ref int lastRowIndex)
        {
            if (mUnfold)
            {
                for (int iChild = childIndexStart; iChild < mChildren.Count; ++iChild)
                {
                    lastRowIndex++;
                    mChildren[iChild].UpdateLocation(lastRowIndex);
                    mChildren[iChild].UpdateChildren(0, ref lastRowIndex);
                }
            }
        }

        private void UpdateLocation(int rowIndex)
        {
            mRowIndex = rowIndex;
            if (mButton != null)
                mButton.Location = new System.Drawing.Point(mNodeRef.Depth * mBtnSize, rowIndex * mBtnSize);
            mLabel.Location = new System.Drawing.Point((mNodeRef.Depth + 1) * mBtnSize, rowIndex * mBtnSize);
        }

        private void UpdateVisibility(bool visible)
        {
            if (mButton != null)
                mButton.Visible = visible;
            mLabel.Visible = visible;
        }
    }
}