using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class QuadrepedalSync : NetworkBehaviour
{
    private Tree<Transform> m_skeletonTree;

    [SerializeField] private FABRIKSettings m_FABRIK;

    [Header("Body")] [SerializeField] private Transform m_hip;
    [SerializeField] private Transform m_withers;
    [SerializeField] private Transform m_neck;
    [SerializeField] private Transform m_head;

    [Header("Front Legs")] [SerializeField]
    private Transform m_shoulderL;

    [SerializeField] private Transform m_foreArmL;
    [SerializeField] private Transform m_forelegL;
    [SerializeField] private Transform m_forefootL;

    [SerializeField] private Transform m_shoulderR;
    [SerializeField] private Transform m_foreArmR;
    [SerializeField] private Transform m_forelegR;
    [SerializeField] private Transform m_forefootR;

    [Header("Hind Legs")] [SerializeField] private Transform m_hipL;
    [SerializeField] private Transform m_thighL;
    [SerializeField] private Transform m_hindlegL;
    [SerializeField] private Transform m_hindfootL;

    [SerializeField] private Transform m_hipR;
    [SerializeField] private Transform m_thighR;
    [SerializeField] private Transform m_hindlegR;
    [SerializeField] private Transform m_hindfootR;


    void Start()
    {
        TreeNode<Transform> hipNode = new TreeNode<Transform>(m_hip);
        m_skeletonTree = new Tree<Transform>(m_hip);

        // Withers
        TreeNode<Transform> withersNode = new TreeNode<Transform>(m_withers);
        hipNode.AddChild(withersNode);

        // Neck
        TreeNode<Transform> neckNode = new TreeNode<Transform>(m_neck);
        hipNode.AddChild(neckNode);

        // Head
        TreeNode<Transform> headNode = new TreeNode<Transform>(m_head);
        hipNode.AddChild(headNode);

        // Front Left Leg
        TreeNode<Transform> shoulderLNode = new TreeNode<Transform>(m_shoulderL);
        TreeNode<Transform> foreArmLNode = new TreeNode<Transform>(m_foreArmL); 
        TreeNode<Transform> forelegLNodeL = new TreeNode<Transform>(m_forelegL);
        TreeNode<Transform> forefootLNodeL = new TreeNode<Transform>(m_forefootL);
        hipNode.AddSibling(shoulderLNode);
        shoulderLNode.AddChild(foreArmLNode); 
        shoulderLNode.AddChild(forelegLNodeL);
        shoulderLNode.AddChild(forefootLNodeL);

        // Front Right Leg
        TreeNode<Transform> shoulderRNode = new TreeNode<Transform>(m_shoulderR);
        TreeNode<Transform> foreArmRNode = new TreeNode<Transform>(m_foreArmR); 
        TreeNode<Transform> forelegRNodeR = new TreeNode<Transform>(m_forelegR);
        TreeNode<Transform> forefootRNodeR = new TreeNode<Transform>(m_forefootR);
        hipNode.AddSibling(shoulderRNode);
        shoulderRNode.AddChild(foreArmRNode); 
        shoulderRNode.AddChild(forelegRNodeR);
        shoulderRNode.AddChild(forefootRNodeR);

        // Hind Left Leg
        TreeNode<Transform> hipLNode = new TreeNode<Transform>(m_hipL);
        TreeNode<Transform> thighLNode = new TreeNode<Transform>(m_thighL); 
        TreeNode<Transform> hindlegLNodeL = new TreeNode<Transform>(m_hindlegL);
        TreeNode<Transform> hindfootLNodeL = new TreeNode<Transform>(m_hindfootL);
        hipNode.AddSibling(hipLNode);
        hipLNode.AddChild(thighLNode); 
        hipLNode.AddChild(hindlegLNodeL);
        hipLNode.AddChild(hindfootLNodeL);

        // Hind Right Leg
        TreeNode<Transform> hipRNode = new TreeNode<Transform>(m_hipR);
        TreeNode<Transform> thighRNode = new TreeNode<Transform>(m_thighR); 
        TreeNode<Transform> hindlegRNodeR = new TreeNode<Transform>(m_hindlegR);
        TreeNode<Transform> hindfootRNodeR = new TreeNode<Transform>(m_hindfootR);
        hipNode.AddSibling(hipRNode);
        hipRNode.AddChild(thighRNode); 
        hipRNode.AddChild(hindlegRNodeR);
        hipRNode.AddChild(hindfootRNodeR);
    }

    // Update is called once per frame
    void Update()
    {
        //  if(ConnectionManager.Instance.GetQTMManager().m_Connected && Object.HasStateAuthority )
        //  {
        //      GrabData();
        //   }
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
        Tree<Transform> HorseTree = TreeSerializationHelper.DeserializeTree<Transform>(Data);
        Transform[] transformsBones = new Transform[4];

        // Root (Hip)
        m_hip.position = HorseTree.m_Root.m_Data.position;
        m_hip.rotation = HorseTree.m_Root.m_Data.rotation;
        transformsBones[0] = m_hip;

        // Withers
        m_withers.position = HorseTree.m_Root.m_Children[0].m_Data.position;
        m_withers.rotation = HorseTree.m_Root.m_Children[0].m_Data.rotation;
        transformsBones[1] = m_withers;

        // Neck
        m_neck.position = HorseTree.m_Root.m_Children[1].m_Data.position;
        m_neck.rotation = HorseTree.m_Root.m_Children[1].m_Data.rotation;
        transformsBones[2] = m_neck;
            
        m_head.position = HorseTree.m_Root.m_Children[2].m_Data.position;
        m_head.rotation = HorseTree.m_Root.m_Children[2].m_Data.rotation;
        transformsBones[3] = m_head;
        
        FABRIKSolver.Solve(ref m_head, ref transformsBones, ref m_FABRIK);

        // Front Left Leg
        m_shoulderL.position = HorseTree.m_Root.m_Siblings[0].m_Data.position;
        m_shoulderL.rotation = HorseTree.m_Root.m_Siblings[0].m_Data.rotation;
        transformsBones[0] = m_shoulderL;

        m_foreArmL.position = HorseTree.m_Root.m_Siblings[0].m_Children[0].m_Data.position; 
        m_foreArmL.rotation = HorseTree.m_Root.m_Siblings[0].m_Children[0].m_Data.rotation; 
        transformsBones[1] = m_foreArmL;

        m_forelegL.position = HorseTree.m_Root.m_Siblings[0].m_Children[1].m_Data.position;
        m_forelegL.rotation = HorseTree.m_Root.m_Siblings[0].m_Children[1].m_Data.rotation;
        transformsBones[2] = m_forelegL;

        m_forefootL.position = HorseTree.m_Root.m_Siblings[0].m_Children[2].m_Data.position;
        m_forefootL.rotation = HorseTree.m_Root.m_Siblings[0].m_Children[2].m_Data.rotation;
        transformsBones[3] = m_forefootL; 

        FABRIKSolver.Solve(ref m_forefootL, ref transformsBones, ref m_FABRIK);

      
        // Front Right Leg
        m_shoulderR.position = HorseTree.m_Root.m_Siblings[1].m_Data.position;
        m_shoulderR.rotation = HorseTree.m_Root.m_Siblings[1].m_Data.rotation;
        transformsBones[0] = m_shoulderR;

        m_foreArmR.position = HorseTree.m_Root.m_Siblings[1].m_Children[0].m_Data.position; 
        m_foreArmR.rotation = HorseTree.m_Root.m_Siblings[1].m_Children[0].m_Data.rotation; 
        transformsBones[1] = m_foreArmR; 

        m_forelegR.position = HorseTree.m_Root.m_Siblings[1].m_Children[1].m_Data.position;
        m_forelegR.rotation = HorseTree.m_Root.m_Siblings[1].m_Children[1].m_Data.rotation;
        transformsBones[2] = m_forelegR;

        m_forefootR.position = HorseTree.m_Root.m_Siblings[1].m_Children[2].m_Data.position;
        m_forefootR.rotation = HorseTree.m_Root.m_Siblings[1].m_Children[2].m_Data.rotation;
        transformsBones[3] = m_forefootR; 
        

        FABRIKSolver.Solve(ref m_forefootR, ref transformsBones, ref m_FABRIK);
        
        // Hind Left Leg
        m_hipL.position = HorseTree.m_Root.m_Siblings[2].m_Data.position;
        m_hipL.rotation = HorseTree.m_Root.m_Siblings[2].m_Data.rotation;
        transformsBones[0] = m_hipL;

        m_thighL.position = HorseTree.m_Root.m_Siblings[2].m_Children[0].m_Data.position; 
        m_thighL.rotation = HorseTree.m_Root.m_Siblings[2].m_Children[0].m_Data.rotation; 
        transformsBones[1] = m_thighL; 

        m_hindlegL.position = HorseTree.m_Root.m_Siblings[2].m_Children[1].m_Data.position;
        m_hindlegL.rotation = HorseTree.m_Root.m_Siblings[2].m_Children[1].m_Data.rotation;
        transformsBones[2] = m_hindlegL;

        m_hindfootL.position = HorseTree.m_Root.m_Siblings[2].m_Children[2].m_Data.position;
        m_hindfootL.rotation = HorseTree.m_Root.m_Siblings[2].m_Children[2].m_Data.rotation;
        transformsBones[3] = m_hindfootL; 

        FABRIKSolver.Solve(ref m_hindfootL, ref transformsBones, ref m_FABRIK);

        // Hind Right Leg
        m_hipR.position = HorseTree.m_Root.m_Siblings[3].m_Data.position;
        m_hipR.rotation = HorseTree.m_Root.m_Siblings[3].m_Data.rotation;
        transformsBones[0] = m_hipR;

        m_thighR.position = HorseTree.m_Root.m_Siblings[3].m_Children[0].m_Data.position; 
        m_thighR.rotation = HorseTree.m_Root.m_Siblings[3].m_Children[0].m_Data.rotation;
        transformsBones[1] = m_thighR;

        m_hindlegR.position = HorseTree.m_Root.m_Siblings[3].m_Children[1].m_Data.position;
        m_hindlegR.rotation = HorseTree.m_Root.m_Siblings[3].m_Children[1].m_Data.rotation;
        transformsBones[2] = m_hindlegR;

        m_hindfootR.position = HorseTree.m_Root.m_Siblings[3].m_Children[2].m_Data.position;
        m_hindfootR.rotation = HorseTree.m_Root.m_Siblings[3].m_Children[2].m_Data.rotation;
        transformsBones[3] = m_hindfootR;

        FABRIKSolver.Solve(ref m_hindfootR, ref transformsBones, ref m_FABRIK);
    }

    #endregion
}