using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using Nakama;
using Nakama.TinyJson;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    private CustomNakamaConnection nakamaInstance;
    private LeaderBoardScript leaderBoardScript;

    public GameObject tilePrefab;
    public GameObject leaderBoardContent;
    public GameObject userInformationContainer;

    public Sprite[] icons;

    private ControlUtils controlUtils;
    private User userObj;

    private string[] MenuItems = {
        GameConstants.LEADERBOARD_ID,
        GameConstants.MAIN_MENU_ID ,
        GameConstants.CHAT_CANVAS,
        GameConstants.EDIT_USER_CANVAS
    };

    // Use this for initialization
    void Start()
    {
        nakamaInstance = CustomNakamaConnection.Instance;
        leaderBoardScript = gameObject.AddComponent<LeaderBoardScript>();
        controlUtils = new ControlUtils();
        userObj = new User();
        OpenMainMenu();

    }

    private void Update()
    {
        if(nakamaInstance.client == null || nakamaInstance.nakamaSession == null || nakamaInstance.nakamaSession.IsExpired)
        {
            SceneManager.LoadScene(GameConstants.LOADING_SCENE);
        }

    }

    public async void OpenLeaderBoard()
    {
        try
        {
            // first enable the leaderboard canvas
            controlUtils.SetComponentActive(0,this.MenuItems);
            if (!tilePrefab || !leaderBoardContent)
            {
                Debug.LogError("tileprefab or leaderboard content is null in main menu script");
                return;
            }

            leaderBoardScript.setTilePrefab(tilePrefab, leaderBoardContent);
            Payload payload = await leaderBoardScript.loadLeaderBoardData(GameConstants.GLOBAL_LEADER_BOARD);
            await leaderBoardScript.PopulateLeaderBoard(payload);
            
        }
        catch(Exception ex)
        {
            Debug.Log("Exception in OpenLeaderBoard = " + ex.ToString());
        }
    }

    public async void OpenMainMenu()
    {
        try
        {
            controlUtils.SetComponentActive(1 , this.MenuItems);
            // fetch user account and display user data also
            var currentUser = await userObj.fetchUserAccount(nakamaInstance.nakamaSession.UserId);
            userInformationContainer.transform.GetChild(1).GetComponent<Text>().text = currentUser.displayName;
            userInformationContainer.transform.GetChild(0).GetComponent<Image>().sprite = GameObject.Find("BasicSceneControls").GetComponent<MainMenuScript>().icons[currentUser.avatarUrl];
        }
        catch (Exception ex)
        {
            Debug.Log("Exception in OpenMainMenu = " + ex.ToString());
        }
    }

    public void openEditUserCanvas()
    {
        try
        {
            controlUtils.SetComponentActive(3, this.MenuItems);
        }
        catch (Exception ex)
        {
            Debug.Log("Exception in openEditUserCanvas = " + ex.ToString());
        }
    }

    
    public void enterGlobalChat()
    {
        try
        {
            Debug.Log("try to enable canvas at index " + 2);
            Debug.Log(this.MenuItems);
            controlUtils.SetComponentActive(2, this.MenuItems);
           
        }
        catch (Exception ex)
        {
            Debug.Log("Issue in enterGlobalChat = " + ex.ToString());
        }
    }

}
