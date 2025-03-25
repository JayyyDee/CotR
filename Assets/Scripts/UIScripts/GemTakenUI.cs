using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemTakenUI : MonoBehaviour //This class sets active the UI to alert player that the gem is taken and starts a countdown.
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGemTakenActive()) {
            Show();
        }
        else {
            Hide();
        }
    }
    private void Update() {
        //Display countdown with the state its in, Mathf.Ceil to show whole numbers only
        countdownText.text = Mathf.Ceil(GameManager.Instance.GetGemTakenTimer()).ToString();
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}
