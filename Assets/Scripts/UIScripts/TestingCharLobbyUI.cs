using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestingCharLobbyUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] public TextMeshProUGUI roomCode;

    private void Awake() {
        readyButton.onClick.AddListener(() =>
        {
            CharacterLobbyReady.Instance.SetPlayerReady();
        });

        //roomCode.text = GameMultiplayer.Instance.GetRoomCode().ToString();
    }

    private void Update() {
        roomCode.text = GameMultiplayer.Instance.GetRoomCode().ToString();
    }

}
