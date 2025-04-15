using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchOverUI : MonoBehaviour
{
    public static MatchOverUI Instance { get; private set; }

    [SerializeField] private Button createGameButton;

    private void Start() {
        Instance = this;
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();

        createGameButton.onClick.AddListener(() =>
        {
            AkUnitySoundEngine.PostEvent("Play_1MENU_BACK__itemnumber", this.gameObject);
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGameOverActive()) {
           Show();
        }
        else {
            Hide();
        }
    }
    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}
