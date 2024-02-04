using System;
using System.Collections.Generic;

[Serializable]
public class TreeNode<T>
{
    public T m_Data;
    public List<TreeNode<T>> m_Children;

    public TreeNode(T data)
    {
        this.m_Data = data;
        m_Children = new List<TreeNode<T>>();
    }

    public void AddChild(TreeNode<T> child)
    {
        m_Children.Add(child);
    }
}
[Serializable]
public class Tree<T>
{
    public TreeNode<T> m_Root;

    public Tree(T rootData)
    {
        m_Root = new TreeNode<T>(rootData);
    }
}