using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;
using Nakama;
using UnityEngine.SceneManagement;

public class ScreenLoading : MonoBehaviour
{

    private AuthManager Auth = new AuthManager();

    public async Task sayHelo()
    {
        Debug.Log("I will send it wait for some time bro");
        await Task.Delay(3 * 1000);
        // here load the new scene .....
        SceneManager.LoadScene(1);
        Debug.Log("say helo is finished");
    }

    public async Task AuthenticateUser()
    {
        try
        {
            Debug.Log("Start user auth");
            
            Debug.Log("Finish user auth");
            
        }catch(Exception e)
        {
            Debug.Log("Exception in auth");

        }
    }
    // Use this for initialization
    async void Start()
    {
        try
        {
            Debug.Log("Start User Auth");
            //await AuthenticateUser();
            await Auth.AuthenticateUserDevice();
            Debug.Log("finish user auth");
            SceneManager.LoadScene(1);

        }catch(Exception ex)
        {
            Debug.Log("Exception in user auth = " + ex.Message);
        }
    }

    // Update is called once per frame for screen loading
    void Update()
    {
       
    }
}
