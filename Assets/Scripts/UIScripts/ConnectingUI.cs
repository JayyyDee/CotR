using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectingUI : MonoBehaviour
{

    private void Start() {
        GameMultiplayer.Instance.OnTryingToJoinGame += GameMulitplayer_OnTryingToJoinGame;
        GameMultiplayer.Instance.OnFailedToJoinGame += Instance_OnFailedToJoinGame;
        Hide();
    }

    private void Instance_OnFailedToJoinGame(object sender, System.EventArgs e) {
        Hide();
    }

    private void GameMulitplayer_OnTryingToJoinGame(object sender, System.EventArgs e) {
        Show();
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    //Destroy this UI when loading into the character lobby scene.
    private void Hide() {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        GameMultiplayer.Instance.OnTryingToJoinGame -= GameMulitplayer_OnTryingToJoinGame;
        GameMultiplayer.Instance.OnFailedToJoinGame -= Instance_OnFailedToJoinGame;
    }
}
