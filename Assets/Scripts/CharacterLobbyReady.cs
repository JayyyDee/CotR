using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CharacterLobbyReady : NetworkBehaviour
{

    public static CharacterLobbyReady Instance { get; private set; }

    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake() {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady() {
        SetPlayerReadyServerRpc();
    }

    //This creates a new playerID when entering a server. It then checks if that dictionnary has content
    //and if all players that are in the lobby are ready, it to switches to the countdown state.
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default) {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientReady = true;
        foreach (ulong clientid in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playerReadyDictionary.ContainsKey(clientid) || !playerReadyDictionary[clientid]) {
                //This player is not ready
                allClientReady = false;
                break;
            }
        }
        if (allClientReady) {
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }
}
