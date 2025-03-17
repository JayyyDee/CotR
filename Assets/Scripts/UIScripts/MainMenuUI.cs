using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private void Update() {
        if (Input.GetKeyUp(KeyCode.Space)) {
            Loader.Load(Loader.Scene.GameScene);
        }

        if (Input.GetKeyUp(KeyCode.Escape)) { 
            Application.Quit();
        }
    }
}
