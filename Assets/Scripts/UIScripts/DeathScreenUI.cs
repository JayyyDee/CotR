using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreenUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;

    private void Start() {
        gameObject.SetActive(false);

        mainMenuButton.onClick.AddListener(() =>
        {
            AkUnitySoundEngine.PostEvent("Play_1MENU_BACK__itemnumber", this.gameObject);
            gameObject.SetActive(false);
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }

    public void Death() {
        gameObject.SetActive(true);
    }

}
