using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class JoinLobby : MonoBehaviour
{
    public TextDisplay m_roomName;
    public TextDisplay m_playerCount;

    public void JoinSession()
    {
        ConnectionManager.Instance.ConnectToLobby(m_roomName.GetTextName(), SessionLobby.Custom);
    }
}
