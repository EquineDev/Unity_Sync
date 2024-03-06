using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;

public class ConnectionManager : Singleton<ConnectionManager>, INetworkRunnerCallbacks
{

	public NetworkRunnerHandle NetworkRunnerHandle { get; private set; } 
	public String SceneMap { get; private set; } 
	public List<SessionInfo> Session { get; private set; } = new List<SessionInfo>();
	public GameMode SessionLobbyType { get; private set; } = GameMode.Shared;
	public QTMManager QTMManager { get; private set; }
	
    [SerializeField] private NetworkPrefabRef m_playerPrefab;
    private Dictionary<PlayerRef, NetworkObject> m_spawnedPlayers = new Dictionary<PlayerRef, NetworkObject>();
    [SerializeField] 
    private NetworkRunnerHandle m_networkHandler { get;  }



    public void ConnectToLobby(string name, SessionLobby type)
    {
	    m_networkHandler.ConnectToSesssion(name);
    }

   
    #region Photon Fussion Calls




    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
	    if (runner.IsServer)
	    {
		    Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1, 0);
		    NetworkObject networkPlayerObject = runner.Spawn(m_playerPrefab, spawnPosition, Quaternion.identity, player);
		    m_spawnedPlayers.Add(player, networkPlayerObject);
		    
		    RpcInfo rpcInfo = new RpcInfo();
		    rpcInfo.Source = player;
		    rpcInfo.Channel = RpcChannel.Reliable;
		    QTMManager.RPC_LateJoinSync(rpcInfo);
		    Debug.Log("Player Has Joined Session:" + player.PlayerId);
	    }
	    else
	    {
		    Debug.Log("Joined");
	    }
	    
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
	    if (m_spawnedPlayers.TryGetValue(player, out NetworkObject networkObject))
	    {
		    runner.Despawn(networkObject);
		    m_spawnedPlayers.Remove(player);
		    Debug.Log("Player Has Left Session:" + player.PlayerId);
	    }
	    else
	    {
		    Debug.Log("Leaving");
	    }
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        
    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server");
       
    }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
	    Debug.Log("Session List Updated");
	    Session.Clear();
	    Session = sessionList;
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        
    }
    
    #endregion

    #region private 

    private static (string, string) ShutdownReasonToHuman(ShutdownReason reason)
	{
		switch (reason)
		{
			case ShutdownReason.Ok:
				return (null, null);
			case ShutdownReason.Error:
				return ("Error", "Shutdown was caused by some internal error");
			case ShutdownReason.IncompatibleConfiguration:
				return ("Incompatible Config", "Mismatching type between client Server Mode and Shared Mode");
			case ShutdownReason.ServerInRoom:
				return ("Room name in use", "There's a room with that name! Please try a different name or wait a while.");
			case ShutdownReason.DisconnectedByPluginLogic:
				return ("Disconnected By Plugin Logic", "You were kicked, the room may have been closed");
			case ShutdownReason.GameClosed:
				return ("Game Closed", "The session cannot be joined, the game is closed");
			case ShutdownReason.GameNotFound:
				return ("Game Not Found", "This room does not exist");
			case ShutdownReason.MaxCcuReached:
				return ("Max Players", "The Max CCU has been reached, please try again later");
			case ShutdownReason.InvalidRegion:
				return ("Invalid Region", "The currently selected region is invalid");
			case ShutdownReason.GameIdAlreadyExists:
				return ("ID already exists", "A room with this name has already been created");
			case ShutdownReason.GameIsFull:
				return ("Game is full", "This lobby is full!");
			case ShutdownReason.InvalidAuthentication:
				return ("Invalid Authentication", "The Authentication values are invalid");
			case ShutdownReason.CustomAuthenticationFailed:
				return ("Authentication Failed", "Custom authentication has failed");
			case ShutdownReason.AuthenticationTicketExpired:
				return ("Authentication Expired", "The authentication ticket has expired");
			case ShutdownReason.PhotonCloudTimeout:
				return ("Cloud Timeout", "Connection with the Photon Cloud has timed out");
			default:
				Debug.LogWarning($"Unknown ShutdownReason {reason}");
				return ("Unknown Shutdown Reason", $"{(int)reason}");
		}
	}

	private static (string,string) ConnectFailedReasonToHuman(NetConnectFailedReason reason)
	{
		switch (reason)
		{
			case NetConnectFailedReason.Timeout:
				return ("Timed Out", "");
			case NetConnectFailedReason.ServerRefused:
				return ("Connection Refused", "The lobby may be currently in-game");
			case NetConnectFailedReason.ServerFull:
				return ("Server Full", "");
			default:
				Debug.LogWarning($"Unknown NetConnectFailedReason {reason}");
				return ("Unknown Connection Failure", $"{(int)reason}");
		}
	}

    #endregion

    #region override

    protected override void Init()
    {
	    base.Init();
	    DontDestroyOnLoad(this);
    }

    #endregion
}
