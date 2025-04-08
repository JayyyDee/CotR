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
        GemCountdown,
        GamePlaying,
        GemTaken,
        GameOver,
    }

    [SerializeField] private Transform playerPrefab;
    [SerializeField] private GameObject gem;

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gemCountdownTimer = new NetworkVariable<float>(15f);
    private NetworkVariable<float> gemTakenTimer = new NetworkVariable<float>(20f);
    private bool isLocalPlayerReady;
    private bool isGamePaused = false;
    private Dictionary<ulong, bool> playerReadyDictionary;

    private uint musicPlayingID;
    private string currentMusicEvent = "";

    private void Awake() {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn() {
        state.OnValueChanged += State_OnValueChanged;

        if (IsServer) {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut) {
        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds) {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientID, true);
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue) {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && isWaitingToStart()) {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
            Debug.Log("Space");
        }

        if (!IsServer) {
            return;
        }

        switch (state.Value) {
            case State.WaitingToStart: //When all of the players in the lobby is ready, change state to start countdown
                gameObject.GetComponent<MusicManager>().SwitchMusic("Play_Musique_Lobby_Full_Onetime__itemnumber");
                break;
            case State.CountdownToStart: //When the countdown finished, switch to game playing
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value < 0f) {
                    state.Value = State.GemCountdown;
                }
                gameObject.GetComponent<MusicManager>().SwitchMusic("");
                break;
            case State.GemCountdown: //When the countdown finished, switch to game playing
                //gameObject.GetComponent<MusicManager>().SwitchMusic("Play_Musique_Combat_Full_Onetime__itemnumber"); //Musique3
                gemCountdownTimer.Value -= Time.deltaTime;
                if (gemCountdownTimer.Value < 0f) {
                    state.Value = State.GamePlaying;
                }
                break;
            case State.GamePlaying://When the gem is no longer active finished, switch to gem taken state
                //gameObject.GetComponent<MusicManager>().SwitchMusic("Play_Musique_Combat_Full_Onetime__itemnumber"); //Musique3
                if (gem.activeSelf == false) {
                    state.Value = State.GemTaken;
                }
                break;
            case State.GemTaken:
                //gameObject.GetComponent<MusicManager>().SwitchMusic("Play_Musique_Combat_Full_Onetime__itemnumber"); //Musique3
                gemTakenTimer.Value -= Time.deltaTime;
                if (gemTakenTimer.Value < 0f) {
                    state.Value = State.GameOver;
                }
                if (gem.activeSelf == true) {
                    gemTakenTimer.Value = 20f; //Reset value when active
                    state.Value = State.GamePlaying;
                }
                break;
            case State.GameOver:
                //gameObject.GetComponent<MusicManager>().SwitchMusic("");
                break;
        }
        //Debug.Log(state);
    }

    //This creates a new playerID when entering a server. It then checks if that dictionnary has content
    //and if all players that are in the lobby are ready, it to switches to the countdown state.
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default) {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        Debug.Log(playerReadyDictionary.Count);

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
    private void SwitchMusic(string newMusicEvent) {
        if (currentMusicEvent == newMusicEvent)
            return;

        // Stop la musique actuelle si elle existe
        if (!string.IsNullOrEmpty(currentMusicEvent)) {
            AkUnitySoundEngine.StopPlayingID(musicPlayingID);
        }

        // Joue la nouvelle
        musicPlayingID = AkUnitySoundEngine.PostEvent(newMusicEvent, gameObject);
        currentMusicEvent = newMusicEvent;
    }

    public bool IsLocalPlayerReady() {
        return isLocalPlayerReady;
    }
    public bool isWaitingToStart() {
        return state.Value == State.WaitingToStart;
    }
    public bool isGamePlaying() {
        return state.Value == State.GamePlaying;
    }
    public bool IsCountdownToStartActive() {
        return state.Value == State.CountdownToStart;
    }
    public bool IsGemCountdownActive() {
        return state.Value == State.GemCountdown;
    }
    public bool IsGemTakenActive() {
        return state.Value == State.GemTaken;
    }
    public bool IsGameOverActive() {
        return state.Value == State.GameOver;
    }
    public float GetCountdownToStartTimer() { 
        return countdownToStartTimer.Value;
    }
    public float GetGemCountdownTimer() {
        return gemCountdownTimer.Value;
    }
    public float GetGemTakenTimer() {
        return gemTakenTimer.Value;
    }
}
