
using Fusion;
using QualisysRealTime.Unity;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]

public class QTMForcePlate : NetworkBehaviour, IQTMObjectInterface
{
    [SerializeField] private LineRenderer m_forceArrow;
    [SerializeField] private LineRenderer m_momentArrow;

    private ForceVector m_forceVector = null;


    private void Update()
    {
        if (m_forceVector != null && !ConnectionManager.Instance.GetQTMManager().m_Connected)
            return;
        RPC_SendData(true, m_forceVector.Force, m_forceVector.ApplicationPoint);
        RPC_SendData(false, m_forceVector.Moment, m_forceVector.ApplicationPoint);
    }

    #region public

    public void SetupObject(string objectName)
    {

        m_forceVector = RTClient.GetInstance().GetForceVector(objectName);
    }

    #endregion

    #region RPC Calls

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_SendData(bool isForce, Vector3 force, Vector3 location)
    {
        if (m_forceArrow == null | m_momentArrow == null)
        {
            Debug.LogError("Missing LineRenderer");
            return;
        }
           
        if (isForce)
        {
            UpdateArrow(ref m_forceArrow, ref location,  VisualDownscaleForce(force) );
        }
        else
        {
            UpdateArrow(ref m_momentArrow, ref location,  VisualDownscaleMoment(force) );
            
        }

    }
    

    #endregion
    
    #region  private

    private void UpdateArrow( ref LineRenderer lineRenderer, ref Vector3 position,  Vector3 directionAndMagnitude )
    { 
        Vector3 endPos = position + directionAndMagnitude;
        Vector3 beginPos = position;
                    
        float headLength = 0.15f;
        float headWidth = 0.1f;
        float stemWidth = headWidth / 4.0f;

        float minLength = headLength;
        float length = Vector3.Distance (beginPos,  endPos);
                    
        lineRenderer.enabled = length >= minLength;
                    
        if(lineRenderer.enabled)
        {   
            
            float breakpoint = headLength / length;
                        
            
            lineRenderer.positionCount = 4;
            lineRenderer.SetPosition (0, beginPos);
            lineRenderer.SetPosition (1, Vector3.Lerp(beginPos,  endPos, 0.999f - breakpoint));
            lineRenderer.SetPosition (2, Vector3.Lerp (beginPos,  endPos, 1 - breakpoint));
            lineRenderer.SetPosition (3,  endPos);
            lineRenderer.widthCurve = new AnimationCurve (
                new Keyframe (0, stemWidth),
                new Keyframe (0.999f - breakpoint, stemWidth),
                new Keyframe (1 - breakpoint, headWidth),
                new Keyframe (1, 0f));
        }
    }
    private Vector3 VisualDownscaleForce(Vector3 v)
    {
        return v / -500.0f;
    }

    private  Vector3 VisualDownscaleMoment(Vector3 v)
    {
        return v / -100.0f;
    }
    
    #endregion
    
    

}
