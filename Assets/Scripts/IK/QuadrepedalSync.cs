using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
[RequireComponent(typeof(NetworkObject))]
public class QuadrepedalSync : NetworkBehaviour
{
    
    private Tree<Transform> m_skeletonTree;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #region RPC Calls

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_SendData()
    {
    }

    #endregion

    #region Private

    private void GrabData()
    {
        
    }

    #endregion

}
