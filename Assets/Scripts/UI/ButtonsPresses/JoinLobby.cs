using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class JoinLobby : MonoBehaviour
{
    private TextDisplay m_roomName;
    private TextDisplay m_playerCount;
    private GameObject m_joinSessionObject;
    private GameObject m_joinWindowList;
    #region public

    public void Setup(string name, string roomcount, ref GameObject WindowListObj ,ref GameObject PasswordObj,  ref string key )
    {
        m_roomName.UpdateText(name);
        m_playerCount.UpdateText(roomcount);
        m_joinSessionObject = PasswordObj;
        m_joinWindowList = WindowListObj;
        m_joinSessionObject.GetComponent<JoinLobbyPassword>().Setup(ref key, ref name );
    }

    public void OpenPasswordWindow()
    {
        m_joinSessionObject.SetActive(true);
        m_joinWindowList.SetActive(false);
    }
    #endregion
}
