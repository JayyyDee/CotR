using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    [SerializeField] public GameObject mainCamera;

    private void Awake()
    {
        //When clicking on the HOST button, start hosting a game.
        hostButton.onClick.AddListener(() => {
            Debug.Log("HOSTING");
            NetworkManager.Singleton.StartHost();
            Hide();
            //Destroy original camera for the player camera
            GameObject.Destroy(mainCamera);
        });

        //When clicking on the CLIENT button, join a game as a client.
        clientButton.onClick.AddListener(() => {
            Debug.Log("CLIENT");
            NetworkManager.Singleton.StartClient();
            Hide();
            //Destroy original camera for the player camera
            GameObject.Destroy(mainCamera);
        });
    }

    //Hide the UI after 
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
