using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusStageConnection : UpdateConnectionStatus
{
    private void OnEnable()
    {
        ConnectionManager.Instance.GetQTMManager().ConnectAction += ConnectionStatus;
        ConnectionManager.Instance.GetQTMManager().TryingConnection += TryingConnection;
    }

    private void OnDisable()
    {
        ConnectionManager.Instance.GetQTMManager().ConnectAction -= ConnectionStatus;
        ConnectionManager.Instance.GetQTMManager().TryingConnection -= TryingConnection;
    }
}
