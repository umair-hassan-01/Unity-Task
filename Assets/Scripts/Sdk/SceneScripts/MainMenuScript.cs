using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class MainMenuScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenLeaderBoard()
    {
        try
        {
            Debug.Log("start hiding menu");
            GameObject.Find("MainMenuCanvas").SetActive(false);
            Debug.Log("first step done");
            GameObject.Find("LeaderBoardCanvas").SetActive(true);
            string som =    "LeaderBoardCanvas";
            Debug.Log("Done with hiding");
        }
        catch(Exception ex)
        {
            Debug.Log("Exception in OpenLeaderBoard = " + ex.ToString());
        }
    }
}
