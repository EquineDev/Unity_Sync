using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
[RequireComponent(typeof(NetworkObject))]
public class BipedalSync : NetworkBehaviour
{
    [SerializeField] private FABRIKSettings m_FABRIK;

    [Header("Left Arm")]
    [SerializeField] 
    private Transform m_upperArmL;
    [SerializeField] 
    private Transform m_lowerArmL;
    [SerializeField] 
    private Transform m_handL;
    
    [Header("Right Arm")]
    [SerializeField] 
    private Transform m_upperArmR;
    [SerializeField] 
    private Transform m_lowerArmR;
    [SerializeField] 
    private Transform m_handR;
    
    [Header("Left Leg")]
    [SerializeField] 
    private Transform m_upperLegL;
    [SerializeField] 
    private Transform m_lowerLegL;
    [SerializeField] 
    private Transform m_FootL;
    
    [Header("Right Leg")]
    [SerializeField] 
    private Transform m_upperLegR;
    [SerializeField] 
    private Transform m_lowerLegR;
    [SerializeField] 
    private Transform m_FootR;
    
    [Header("Body")]
    [SerializeField] 
    private Transform m_hip;
    [SerializeField] 
    private Transform m_neck;
    [SerializeField] 
    private Transform m_head;
    
    private Tree<Transform> m_skeletonTree;
    
    private void Start()
    {
        
        TreeNode<Transform> hipNode = new TreeNode<Transform>(m_hip);
        m_skeletonTree = new Tree<Transform>(m_hip);
        
        // Neck
        TreeNode<Transform> neckNode = new TreeNode<Transform>(m_neck);
        neckNode.m_Data = m_neck;
        hipNode.AddChild(neckNode);

        // Body
        TreeNode<Transform> headNode = new TreeNode<Transform>(m_head);
        neckNode.AddChild(headNode);
        
    // Left Arm
        TreeNode<Transform> leftArmNode = new TreeNode<Transform>(m_upperArmL);
        TreeNode<Transform> leftLowerArmNode = new TreeNode<Transform>(m_lowerArmL);
        TreeNode<Transform> leftHandNode = new TreeNode<Transform>(m_handL);
        hipNode.AddSibling(leftArmNode);
        leftArmNode.AddChild(leftLowerArmNode);
        leftArmNode.AddChild(leftHandNode);

    // Right Arm
        TreeNode<Transform> rightArmNode = new TreeNode<Transform>(m_upperArmR);
        TreeNode<Transform> rightLowerArmNode = new TreeNode<Transform>(m_lowerArmR);
        TreeNode<Transform> rightHandNode = new TreeNode<Transform>(m_handR);
        hipNode.AddSibling(rightArmNode);
        rightArmNode.AddChild(rightLowerArmNode);
        rightArmNode.AddChild(rightHandNode);

    // Left Leg
        TreeNode<Transform> leftLegNode = new TreeNode<Transform>(m_upperLegL);
        TreeNode<Transform> leftLowerLegNode = new TreeNode<Transform>(m_lowerLegL);
        TreeNode<Transform> leftFootNode = new TreeNode<Transform>(m_FootL);
        hipNode.AddSibling(leftLegNode);
        leftLegNode.AddChild(leftLowerLegNode);
        leftLegNode.AddChild(leftFootNode);

    // Right Leg
        TreeNode<Transform> rightLegNode = new TreeNode<Transform>(m_upperLegR);
        TreeNode<Transform> rightLowerLegNode = new TreeNode<Transform>(m_lowerLegR);
        TreeNode<Transform> rightFootNode = new TreeNode<Transform>(m_FootR);
        hipNode.AddSibling(rightLegNode);
        rightLegNode.AddChild(rightLowerLegNode);
        rightLegNode.AddChild(rightFootNode);

        
    }

    

    void Update()
    {
       
        //  if(ConnectionManager.Instance.GetQTMManager().m_Connected && Object.HasStateAuthority )
        //  {

        //   }
        // Transform[] bones = new Transform[] { upperArm, lowerArm, hand }; // Assuming thirdBone is the third bone in your chain
        // FABRIKSolver.Solve(ref targetObject, ref bones, ref m_FABRIK);

    }

 
    #region RPC Calls

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_SendData(byte[] Data)
    {
        SetTransforms(Data);
    }

    #endregion

    #region Private

    private void GrabData()
    {
        byte[] serializedTree = TreeSerializationHelper.SerializeTree(m_skeletonTree);
        RPC_SendData(serializedTree);
    }

    private void SetTransforms(byte[] Data)
    {
        Tree<Transform> BipedalTree = TreeSerializationHelper.DeserializeTree<Transform>(Data);
        Transform[] transformsBones = new Transform[3]; 
        
        m_hip.transform.position = BipedalTree.m_Root.m_Data.position;
        m_hip.transform.rotation = BipedalTree.m_Root.m_Data.rotation;
        transformsBones[0] = m_hip;
        
        m_neck.transform.position = BipedalTree.m_Root.m_Children[0].m_Data.position;
        m_neck.transform.rotation = BipedalTree.m_Root.m_Children[0].m_Data.rotation;
        transformsBones[1] = m_neck;
        
        m_head.transform.position = BipedalTree.m_Root.m_Children[1].m_Data.position;
        m_head.transform.rotation = BipedalTree.m_Root.m_Children[1].m_Data.rotation;
        transformsBones[2] = m_head;
        
        FABRIKSolver.Solve(ref m_head,ref transformsBones, ref m_FABRIK);
        
        m_upperLegR.transform.position = BipedalTree.m_Root.m_Siblings[3].m_Data.position;
        m_upperLegR.transform.rotation = BipedalTree.m_Root.m_Siblings[3].m_Data.rotation;
        transformsBones[0] = m_upperLegR;
        
        m_lowerLegR.transform.position = BipedalTree.m_Root.m_Siblings[3].m_Children[0].m_Data.position;
        m_lowerLegR.transform.rotation = BipedalTree.m_Root.m_Siblings[3].m_Children[0].m_Data.rotation;
        transformsBones[1] = m_lowerLegR;
        
        m_FootR.transform.position = BipedalTree.m_Root.m_Siblings[3].m_Children[1].m_Data.position;
        m_FootR.transform.rotation = BipedalTree.m_Root.m_Siblings[3].m_Children[1].m_Data.rotation;
        transformsBones[2] = m_FootR;
        
        FABRIKSolver.Solve(ref m_FootR,ref transformsBones, ref m_FABRIK);
        
        m_upperLegL.transform.position = BipedalTree.m_Root.m_Siblings[2].m_Data.position;
        m_upperLegL.transform.rotation = BipedalTree.m_Root.m_Siblings[2].m_Data.rotation;
        transformsBones[0] = m_upperLegL;
        
        m_lowerLegL.transform.position = BipedalTree.m_Root.m_Siblings[2].m_Children[0].m_Data.position;
        m_lowerLegL.transform.rotation = BipedalTree.m_Root.m_Siblings[2].m_Children[0].m_Data.rotation;
        transformsBones[1] = m_lowerLegL;
        
        m_FootL.transform.position = BipedalTree.m_Root.m_Siblings[2].m_Children[1].m_Data.position;
        m_FootL.transform.rotation = BipedalTree.m_Root.m_Siblings[2].m_Children[1].m_Data.rotation;
        transformsBones[2] = m_FootL;
        
        FABRIKSolver.Solve(ref m_FootL,ref transformsBones, ref m_FABRIK);
        
        m_upperArmR.transform.position = BipedalTree.m_Root.m_Siblings[1].m_Data.position;
        m_upperArmR.transform.rotation = BipedalTree.m_Root.m_Siblings[1].m_Data.rotation;
        transformsBones[0] = m_upperArmR;
        
        m_lowerArmR.transform.position = BipedalTree.m_Root.m_Siblings[1].m_Children[0].m_Data.position;
        m_lowerArmR.transform.rotation = BipedalTree.m_Root.m_Siblings[1].m_Children[0].m_Data.rotation;
        transformsBones[1] = m_lowerArmR;
        
        m_handR.transform.position = BipedalTree.m_Root.m_Siblings[1].m_Children[1].m_Data.position;
        m_handR.transform.rotation = BipedalTree.m_Root.m_Siblings[1].m_Children[1].m_Data.rotation;
        transformsBones[2] = m_handR;
        
        FABRIKSolver.Solve(ref m_handR,ref transformsBones, ref m_FABRIK);
        
        m_upperArmL.transform.position = BipedalTree.m_Root.m_Siblings[0].m_Data.position;
        m_upperArmL.transform.rotation = BipedalTree.m_Root.m_Siblings[0].m_Data.rotation;
        transformsBones[0] = m_upperArmL;
        
        m_lowerArmL.transform.position = BipedalTree.m_Root.m_Siblings[0].m_Children[0].m_Data.position;
        m_lowerArmL.transform.rotation = BipedalTree.m_Root.m_Siblings[0].m_Children[0].m_Data.rotation;
        transformsBones[1] = m_lowerArmL;
        
        m_handL.transform.position = BipedalTree.m_Root.m_Siblings[0].m_Children[1].m_Data.position;
        m_handL.transform.rotation = BipedalTree.m_Root.m_Siblings[0].m_Children[1].m_Data.rotation;
        transformsBones[2] = m_handL;
        
        FABRIKSolver.Solve(ref m_handL,ref transformsBones, ref m_FABRIK);
    }
    #endregion
   
    
   
}
