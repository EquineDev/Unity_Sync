using System;
using System.Linq;
using Fusion;
using System.Threading.Tasks;
using Fusion.Sockets;
using UnityEngine;

public class NetworkRunnerHandle: MonoBehaviour
{
    [SerializeField]
    private NetworkRunner m_networkRunnerPrefab;

    private NetworkRunner m_networkRunner;
    // Start is called before the first frame update
    void Start()
    {
        m_networkRunner = Instantiate(m_networkRunnerPrefab);
        m_networkRunner.name = "NetworkRunner";

    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode, NetAddress address, 
        SceneRef sceneRef, string sessionName,  Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;
        return runner.StartGame(new StartGameArgs
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
