using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro.Examples;

public class GameMultiplayer : NetworkBehaviour
{
    private const int MAX_PLAYER_AMOUNT = 6;
    public static GameMultiplayer Instance { get; private set; }

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;

    public string codeText = "";

    private void Awake() {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private async void Start() {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    //Allocate the relay, create the join code then creates a connection by going through the Unity Relay system.
    public async void StartHost() {
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(MAX_PLAYER_AMOUNT - 1);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

            NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
            NetworkManager.Singleton.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterLobbyScene);
            Debug.Log(joinCode);
            codeText = "Room Code : " + joinCode; //Put code here to have the right roomcode
        }  catch (RelayServiceException e) {
            Debug.Log(e);
        }

    }

    //Get the join code from the allocation then connects to the created game.
    public async void StartClient(string joinCode) {
        try {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

            OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e) {
            Debug.Log(e);
        }

    }
    //This fonction makes it that the game doesnt allow late join and creates the player object.
    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse) {
        if (SceneManager.GetActiveScene().name != Loader.Scene.CharacterLobbyScene.ToString()) {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started.";
            return;
        }
        if (NetworkManager.Singleton.ConnectedClientsIds.Count >= MAX_PLAYER_AMOUNT ) {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full.";
            return;
        }
        connectionApprovalResponse.Approved = true;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientID) {
        OnFailedToJoinGame?.Invoke(this, EventArgs.Empty);  
    }
    public string GetRoomCode() {
      return codeText;
    }
    public string GetPlayerCount() {
        return "Players in your lobby : " + NetworkManager.Singleton.ConnectedClientsIds.Count;
    }
}
