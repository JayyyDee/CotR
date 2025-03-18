using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Loader.Load(Loader.Scene.LobbyScene);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) { 
            Application.Quit();
        }
    }
}
