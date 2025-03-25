using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GemCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start() {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsGemCountdownActive()) {
            Show();
        }
        else {
            Hide();
        }
    }
    private void Update() {
        //Display countdown with the state its in, Mathf.Ceil to show whole numbers only
        countdownText.text = Mathf.Ceil(GameManager.Instance.GetGemCountdownTimer()).ToString();
    }

    private void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
}

