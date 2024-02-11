using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SAVING DATA TO FILE USAGE for SQL Database
public static class BoneCompress 
{
    public static Bone CompressBone(ref Bone target, ref Bone source)
    {
        Bone compressedBone = new Bone();
        compressedBone.Location = target.Location - source.Location;

        return compressedBone;
    }

    public static Bone UncompressBone(ref Bone target, ref Bone source)
    {
        Bone uncompressedBone = new Bone();
        uncompressedBone.Location = target.Location + source.Location;

        return uncompressedBone;
    }
    
    public static Vector3 CalculateTransformDifference(TreeNode<Bone> startNode, TreeNode<Bone> endNode)
    {
        if (startNode == null || endNode == null)
        {
            Debug.LogError("Start or end node is null.");
            return Vector3.zero;
        }
        
        Vector3 difference = Vector3.zero;

        TreeNode<Bone> currentNode = startNode;

        while (currentNode != null && currentNode != endNode)
        {
            if (currentNode.m_Children.Count > 0)
            {
               
                difference += currentNode.m_Data.Location;
            }

            // traverse first child 
            if (currentNode.m_Children.Count > 0)
            {
                currentNode = currentNode.m_Children[0];
            }
                
            // traverse  next sibling
            else if (currentNode.NextSibling() != null)
            {
                currentNode = currentNode.NextSibling();
            }
            
            else
            {
                TreeNode<Bone> parent = currentNode.m_Parent;
                
                while (parent != null && parent.NextSibling() == null)
                {
                    parent = parent.m_Parent;
                }
                
                currentNode = parent?.NextSibling();
            }
        }

        return difference;
    }
    
    public static void AddTransformDifference(TreeNode<Bone> startNode, Vector3 difference)
    {
        TreeNode<Bone> currentNode = startNode;

        while (currentNode != null)
        {
            
            currentNode.m_Data.Location += difference;

            // traverse first child 
            if (currentNode.m_Children.Count > 0)
            {
                currentNode = currentNode.m_Children[0];
            }
               
            // traverse  next sibling
            else if (currentNode.NextSibling() != null)
            {
                currentNode = currentNode.NextSibling();
            }
              
         
            else
            {
                TreeNode<Bone> parent = currentNode.m_Parent;
                while (parent != null && parent.NextSibling() == null)
                {
                    parent = parent.m_Parent;
                }
                  

                currentNode = parent?.NextSibling();
            }
        }
    }
}
