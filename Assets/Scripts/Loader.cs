using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader { //This class is not attach to anything, can load even if we switch scene.

    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene,
        TestScene
    }

    private static Scene targetScene;
    
    //Make the active scene the loading scene while the next scene is loading.
    public static void Load(Scene targetScene) {
        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }
    //When the next scene is done loading, send it to the next loading scene.
    public static void LoaderCallback() {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
