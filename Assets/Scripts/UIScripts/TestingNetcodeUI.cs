using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class TestingNetcodeUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;

    private void Awake()
    {
        //When clicking on the HOST button, start hosting a game.
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            Hide();
        });

        //When clicking on the CLIENT button, join a game as a client.
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            Hide();
        });
    }

    //Hide the UI after 
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
