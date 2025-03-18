using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingCharLobbyUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;

    private void Awake() {
        readyButton.onClick.AddListener(() =>
        {
            CharacterLobbyReady.Instance.SetPlayerReady();
        });
    }
}
