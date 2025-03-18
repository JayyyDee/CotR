using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class GameManager : NetworkBehaviour {
    
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalPlayerReadyChanged;

   private enum State {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver,
    }

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalPlayerReady;
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(10f);
    private bool isGamePaused = false;
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake() {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn() {
        state.OnValueChanged += State_OnValueChanged;
    }

    private void State_OnValueChanged(State previousValue, State newValue) {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        if (!IsServer) {
            return;
        }
        //When a condition is met for each state, switch to the next.
        switch (state.Value) {
            case State.WaitingToStart:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    isLocalPlayerReady = true;
                    OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
                    SetPlayerReadyServerRpc();                
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value < 0f) {
                    state.Value = State.GamePlaying;
 
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0f) {
                    state.Value = State.GameOver;

                }
                break;
            case State.GameOver:
                break;
        }
        //Debug.Log(state);
    }

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
            state.Value = State.CountdownToStart;
        }
    }

    public bool isGamePlaying() {
        return state.Value == State.GamePlaying;
    }
    public bool IsLocalPlayerReady() {
        return isLocalPlayerReady;
    }
    public bool IsCountdownToStartActive() {
        return state.Value == State.CountdownToStart;
    }
    public bool IsGameOverActive() {
        return state.Value == State.GameOver;
    }
    public float GetCountdownToStartTimer() { 
        return countdownToStartTimer.Value;
    }
}
