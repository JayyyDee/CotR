using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestingCharLobbyUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] public TextMeshProUGUI roomCode;
    [SerializeField] public Sprite differentSprite;
    private Image spriteRenderer;

    private void Start() {
        spriteRenderer = gameObject.GetComponent<Image>();
    }

    private void Awake() {
        readyButton.onClick.AddListener(() =>
        {
            AkUnitySoundEngine.PostEvent("Play_4MENU_SELECT2__itemnumber", this.gameObject);
            CharacterLobbyReady.Instance.SetPlayerReady();
            spriteRenderer.sprite = differentSprite;
        });
    }

    private void Update() {
        roomCode.text = GameMultiplayer.Instance.GetRoomCode().ToString();
    }

}
