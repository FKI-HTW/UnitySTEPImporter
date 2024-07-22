using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BHTTreeView
{
    // copied from https://stackoverflow.com/questions/66893/tree-data-structure-in-c-sharp
    // 
    public class TreeNode<T>
    {
        private readonly T m_value;
        private readonly List<TreeNode<T>> m_children = new List<TreeNode<T>>();

        public TreeNode(T value, string name, bool atomic)
        {
            m_value = value;
            Name = name;
            Atomic = atomic;
            Depth = 0;
        }

        public TreeNode<T> this[int i]
        {
            get { return m_children[i]; }
        }
        public bool FindByName(string name, ref TreeNode<T> node)
        {
            bool found = false;
            if (name == Name)
            {
                node = this;
                found = true;
            }
            else
            {
                foreach (var child in m_children)
                {
                    found = child.FindByName(name, ref node);
                    if (found) break;
                }
            }
            
            return found;
        }

        public TreeNode<T> Parent { get; private set; }

        public T Value { get { return m_value; } }
        public bool Atomic { get; private set; }
        public string Name { get; private set; }
        public int Depth { get; private set; }

        public ReadOnlyCollection<TreeNode<T>> Children
        {
            get { return m_children.AsReadOnly(); }
        }
        public TreeNode<T> AddChild(T value, string name, bool atomic)
        {
            TreeNode<T> node = null;
            if (!Atomic)
            {
                node = new TreeNode<T>(value, name, atomic) { Parent = this, Depth = this.Depth + 1 };
                m_children.Add(node);
            }
            // else ASSERT_POISON

            return node;
        }

        //public TreeNode<T>[] AddChildren(params T[] values)
        //{
        //    return values.Select(AddChild).ToArray();
        //}

        public bool RemoveChild(TreeNode<T> node)
        {
            return m_children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in m_children)
                child.Traverse(action);
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(m_children.SelectMany(x => x.Flatten()));
        }
    }
}
