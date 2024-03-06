using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusStageConnection : UpdateConnectionStatus
{
    private void OnEnable()
    {
        ConnectionManager.Instance.QTMManager.ConnectAction += ConnectionStatus;
        ConnectionManager.Instance.QTMManager.TryingConnection += TryingConnection;
    }

    private void OnDisable()
    {
        ConnectionManager.Instance.QTMManager.ConnectAction -= ConnectionStatus;
        ConnectionManager.Instance.QTMManager.TryingConnection -= TryingConnection;
    }
}
