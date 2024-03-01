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
            lobby.m_roomName.UpdateText(info.Name);
            lobby.m_playerCount.UpdateText(info.PlayerCount + "/" + info.MaxPlayers);
           
            
        }
    }
}
