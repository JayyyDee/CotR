using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ConnectionResponseUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;

    private void Start() {
        GameMultiplayer.Instance.OnFailedToJoinGame += Instance_OnFailedToJoinGame;
        Hide();
    }

    private void Instance_OnFailedToJoinGame(object sender, System.EventArgs e) {
        Show();
        messageText.text = NetworkManager.Singleton.DisconnectReason;

        if (messageText.text == "") {
            messageText.text = "Failed to connect";
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
    //Destroy this UI when loading into the character lobby scene.
    private void OnDestroy() {
        GameMultiplayer.Instance.OnFailedToJoinGame -= Instance_OnFailedToJoinGame;
    }
}
