using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class TreeNode<T>
{
    public T m_Data;
    public List<TreeNode<T>> m_Children;
    public TreeNode<T> m_Parent;
    public List<TreeNode<T>> m_Siblings;
    public TreeNode()
    {
        m_Children = new List<TreeNode<T>>();
        m_Siblings = new List<TreeNode<T>>();
    }
    public TreeNode(T data)
    {
        this.m_Data = data;
        m_Children = new List<TreeNode<T>>();
        m_Siblings = new List<TreeNode<T>>();
    }

    public void AddChild(TreeNode<T> child)
    {
        m_Children.Add(child);
    }
    public void AddSibling(TreeNode<T> sibling)
    {
        m_Siblings.Add(sibling);
    }
    public TreeNode<T> NextSibling()
    {
        if (m_Parent == null)
            return null;

        int index = m_Parent.m_Children.IndexOf(this);
        
        if (index < m_Parent.m_Children.Count - 1)
        {
            return m_Parent.m_Children[index + 1];
        }
        else
        {
            return null;
        }
         
    }
}
[Serializable]
public class Tree<T>
{
    public TreeNode<T> m_Root;
    public Tree()
    {
    }
    public Tree(T rootData)
    {
        m_Root = new TreeNode<T>(rootData);
    }
    public void PrintTree(TreeNode<T> startNode)
    {
        PrintNode(startNode, 0);
    }
    
    private void PrintNode(TreeNode<T> node, int depth)
    {
        if (node == null)
            return;

        string indent = new string(' ', depth * 4); 
        string parentage = node.m_Parent == null ? "Root" : node.m_Parent.m_Data.ToString();
        string siblings = string.Join(", ", node.m_Siblings.Select(s => s.m_Data)); 
        Debug.Log($"{indent}Node: {node.m_Data}, Depth: {depth}, Parent: {parentage}, Siblings: [{siblings}]");

        //children
        foreach (var child in node.m_Children)
        {
            PrintNode(child, depth + 1);
        }
        //siblings
        foreach (var sibling in node.m_Siblings)
        {
            if (sibling != node)
            {
                PrintNode(sibling, depth);
            }
        }
    }
}