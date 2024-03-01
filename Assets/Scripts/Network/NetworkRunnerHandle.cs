using System;
using System.Linq;
using Fusion;
using System.Threading.Tasks;
using Fusion.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class NetworkRunnerHandle : MonoBehaviour
{
    public NetworkRunner NetworkRunnerClient { get; private set; }
    [SerializeField] private NetworkRunner m_networkRunnerPrefab;


    // Start is called before the first frame update
    void Start()
    {
        NetworkRunnerClient = this.AddComponent<NetworkRunner>()

    }

    public async void ConnectToSesssion(string Name)
    {
        if (!NetworkRunnerClient)
            return;

        await NetworkRunnerClient.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Client,
            SessionName =  Name
        });
    }
    public async void CreateSession (string Name)
    {
        if (!NetworkRunnerClient)
            return;
        
        var sceneManager = NetworkRunnerClient.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = NetworkRunnerClient.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        NetworkRunnerClient.ProvideInput = true;
        await NetworkRunnerClient.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Host,
            SessionName =  Name,
            PlayerCount =  2,
            IsOpen = true,
            SceneManager = sceneManager
        });
        if (NetworkRunnerClient.IsServer)
        {
            NetworkRunnerClient.LoadScene(ConnectionManager.Instance.SceneMap);
        }
    }


    protected virtual Task InitializeNetworkRunner( GameMode gameMode, NetAddress address, 
        SceneRef sceneRef, string sessionName,  Action<NetworkRunner> initialized)
    {
        var sceneManager = NetworkRunnerClient.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = NetworkRunnerClient.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        NetworkRunnerClient.ProvideInput = true;
        return NetworkRunnerClient.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            Address = address,
            Scene = sceneRef,
            IsVisible = true,
            PlayerCount =  2,
            IsOpen =  true,
            SessionName = sessionName,
            SceneManager = sceneManager
        });
        
    }
}
