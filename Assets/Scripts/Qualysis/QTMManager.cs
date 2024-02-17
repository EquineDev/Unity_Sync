using System.Collections;
using QTMRealTimeSDK;
using QualisysRealTime.Unity;
using UnityEngine;


public class QTMManager : TrackerManager<QTMManager>
{
    private RTClientUpdater m_clientStreamer;
    private DiscoveryResponse m_response;


    #region Public

    public RTConnectionState GetClientConnectionState()
    {
        return RTClient.GetInstance().ConnectionState;
    }

    public override void SetConnection(string ip = "127.0.0.1", short port = -1)
    {
        StartCoroutine(Connect(ip, port));
    }

    #endregion

    #region protect

    protected override void SetupObject(GameObject obj, string objectName)
    {
        base.SetupObject(obj, objectName);
        if (gameObject.TryGetComponent<IQTMObjectInterface>(out IQTMObjectInterface setup))
        {
            setup.SetupObject(objectName);
        }
    }

    #endregion

    IEnumerator Connect(string ip, short port)
    {
        TryingConnection?.Invoke();

        m_response.IpAddress = ip;
        RTClient.GetInstance().StartConnecting(m_response.IpAddress, port, true, true, false, true, false, true, true);


        while (RTClient.GetInstance().ConnectionState == RTConnectionState.Connecting)
        {
            yield return null;
        }

        m_Connected = RTClient.GetInstance().ConnectionState == RTConnectionState.Connected;
        ConnectAction?.Invoke(m_Connected);
    }
}