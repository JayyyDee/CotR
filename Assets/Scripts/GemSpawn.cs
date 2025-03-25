using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemSpawn : MonoBehaviour //This class waits until the game state is on game playing before spawning the gem.
{
    private void Start() {
        this.gameObject.SetActive(false);
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e) {
        if (GameManager.Instance.isGamePlaying()) {
            this.gameObject.SetActive(true);
        }
    }

}
