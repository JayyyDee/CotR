using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour {
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;
    [SerializeField] private TMP_InputField codeRoomText;


    private void Awake() {
        //When clicking on the HOST button, start hosting a game.
        createGameButton.onClick.AddListener(() =>
        {
            GameMultiplayer.Instance.StartHost();
        });

    }

    private void Start() {
        //When clicking on the CLIENT button, join a game.
        joinGameButton.onClick.AddListener(() =>
        {
            GameMultiplayer.Instance.StartClient(codeRoomText.text);
        });
    }
}
