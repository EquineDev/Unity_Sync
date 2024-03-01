using Fusion;
using MalbersAnimations;
using UnityEngine;


public class NetworkUser : NetworkBehaviour, IPlayerLeft
{
    
    public static Network User { get; set; }
    // Start is called before the first frame update
    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasStateAuthority)
        {
          
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        throw new System.NotImplementedException();
    }
}
