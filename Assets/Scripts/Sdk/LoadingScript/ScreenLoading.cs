using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;
using Nakama;
using UnityEngine.SceneManagement;

public class ScreenLoading : MonoBehaviour
{

    public async void authenticateUser()
    {
        try
        {
            AuthManager manager = new AuthManager(CustomNakamaConnection.Instance);
            await manager.AuthenticateUserDevice();
            Debug.Log("User is authenticated");
            SceneManager.LoadScene("MenuScene");
        }
        catch (Exception ex)
        {
            Debug.Log("Exception in authenticate user from screen loading");
            Debug.Log(ex.ToString());
        }
    }

    // Use this for initialization
    async void Start()
    {
        
    }

    // Update is called once per frame for screen loading
    void Update()
    {
       
    }
}
