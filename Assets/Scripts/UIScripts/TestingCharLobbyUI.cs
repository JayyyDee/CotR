using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestingCharLobbyUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Button showCodeButton;
    [SerializeField] public TextMeshProUGUI roomCode;

    private void Awake() {
        readyButton.onClick.AddListener(() =>
        {
            CharacterLobbyReady.Instance.SetPlayerReady();
        });

        showCodeButton.onClick.AddListener(() =>
        {
            roomCode.text = GameMultiplayer.Instance.GetRoomCode().ToString();
        });
    }
}
