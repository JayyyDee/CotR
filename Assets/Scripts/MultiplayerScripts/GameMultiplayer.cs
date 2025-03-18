using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMultiplayer : NetworkBehaviour
{
    public static GameMultiplayer Instance { get; private set; }

    private void Awake() {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void StartHost() {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    //This fonction makes it that the game doesnt allow late join and creates the player object.
    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse) {
        connectionApprovalResponse.Approved = true;
        //if (GameManager.Instance.isWaitingToStart()) {
        //    connectionApprovalResponse.Approved = true;
        //    connectionApprovalResponse.CreatePlayerObject = true;
        //} else {
        //    connectionApprovalResponse.Approved = false;
        //}
    }

    public void StartClient() {
        NetworkManager.Singleton.StartClient();
    }
}
