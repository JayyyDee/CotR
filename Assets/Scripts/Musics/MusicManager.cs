using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;

    private uint musicPlayingID;
    private string currentMusicEvent = "";

    void Awake()
    {
        // Singleton pour ne pas dupliquer
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        HandleScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        HandleScene(scene.name);
    }

    private void HandleScene(string sceneName)
    {
        switch (sceneName)
        {
            case "MainMenuScene": //Scene1
                SwitchMusic("Play_Musique_Menu_Loop__itemnumber"); //Musique1
                break;
            case "LoadingScene": //Scene2
                SwitchMusic("Play_Musique_Menu_Loop__itemnumber"); //Musique1
                break;
            case "LobbyScene": //Scene3
                SwitchMusic("Play_Musique_Menu_Loop__itemnumber"); //Musique1
                break;
            case "CharacterLobbyScene": //Scene4
                SwitchMusic("Play_Musique_Menu_Loop__itemnumber"); //Musique1
                break;
            default:
                // Tu peux ajouter un event de musique par défaut si nécessaire
                break;
        }
    }

    private void SwitchMusic(string newMusicEvent)
    {
        if (currentMusicEvent == newMusicEvent)
            return;

        // Stop la musique actuelle si elle existe
        if (!string.IsNullOrEmpty(currentMusicEvent))
        {
            AkUnitySoundEngine.StopPlayingID(musicPlayingID);
        }

        // Joue la nouvelle
        musicPlayingID = AkUnitySoundEngine.PostEvent(newMusicEvent, gameObject);
        currentMusicEvent = newMusicEvent;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}