using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System;
using Nakama;
using UnityEngine.SceneManagement;

public class ScreenLoading : MonoBehaviour
{
    private string[] items = { "LoginCanvas", "LoadingCanvas", "ErrorCanvas" };
    private ControlUtils controlUtils;

    public async Task<bool> authenticateUser()
    {
        try
        {
            AuthManager manager = new AuthManager(CustomNakamaConnection.Instance);
            await manager.AuthenticateUserDevice();
            Debug.Log("User is authenticated");
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log("Exception in authenticate user from screen loading");
            Debug.Log(ex.ToString());
        }

        return false;
    }

    public async void openLoadingCanvas()
    {
        try
        {
            StartCoroutine(StartLoading());

        }catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public void openLoginCanvas()
    {
        try
        {
            Debug.Log("Start logging in user");
            controlUtils.SetComponentActive(0, this.items);

        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    IEnumerator StartLoading()
    {
        controlUtils.SetComponentActive(1, this.items);
        yield return null;

        AuthManager manager = new AuthManager(CustomNakamaConnection.Instance);
        Task<bool> authTask = this.authenticateUser();
        yield return new WaitUntil(() => authTask.IsCompleted);

        if (authTask.Result)
        {
            Debug.Log("user is good to go");
            // load main menu scene
            SceneManager.LoadScene(GameConstants.MAIN_MENU_SCENE);
        }
        else
        {
            Debug.Log("error in auth");
            // show the error canvas
            controlUtils.SetComponentActive(2, this.items);
        }

        yield return null;
    }

    // Use this for initialization
    async void Start()
    {
        controlUtils = new ControlUtils();
    }

    // Update is called once per frame for screen loading
    void Update()
    {
       
    }
}
