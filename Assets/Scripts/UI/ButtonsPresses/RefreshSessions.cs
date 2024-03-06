using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class RefreshSessions : MonoBehaviour
{
    [SerializeField] 
    private Transform m_sessionContents;

    [SerializeField] 
    private GameObject m_sessionPrefab;
    [SerializeField] 
    private GameObject m_joinWindowList;
    [SerializeField] 
    private GameObject m_joinPasswordWindow;
    public void RefreshList()
    {
        foreach (Transform session in m_sessionContents)
        {
            Destroy(session.gameObject);
        }

        foreach (SessionInfo info in ConnectionManager.Instance.Session)
        {
            if(info.IsOpen == false || info.PlayerCount >= info.MaxPlayers)
                continue;
            GameObject obj = GameObject.Instantiate(m_sessionPrefab, m_sessionContents.transform);
            JoinLobby lobby = obj.GetComponent<JoinLobby>();
            string key = info.Properties.GetValueOrDefault("sessionKey").PropertyValue as string;
            lobby.Setup(info.Name,info.PlayerCount + "/" + info.MaxPlayers, ref m_joinWindowList,
                ref m_joinPasswordWindow, ref key );
        }
    }
}
