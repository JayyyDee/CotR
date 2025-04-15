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
            AkUnitySoundEngine.PostEvent("Play_3MENU_SELECT__itemnumber", this.gameObject);
            GameMultiplayer.Instance.StartHost();
        });

    }

    private void Start() {
        //When clicking on the CLIENT button, join a game.
        joinGameButton.onClick.AddListener(() =>
        {
            AkUnitySoundEngine.PostEvent("Play_3MENU_SELECT__itemnumber", this.gameObject);
            GameMultiplayer.Instance.StartClient(codeRoomText.text);
        });
    }
}
