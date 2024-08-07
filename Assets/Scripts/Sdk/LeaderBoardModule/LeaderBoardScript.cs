using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using Nakama.TinyJson;


public class LeaderBoardScript : MonoBehaviour
{
    private GameObject tilePrefab;
    private GameObject leaderBoardContent;
    
    public CustomNakamaConnection nakamaInstance;

    public void setTilePrefab(GameObject tilePrefab , GameObject leaderBoardContent)
    {
        this.tilePrefab = tilePrefab;
        this.leaderBoardContent = leaderBoardContent;
    }

    // Start is called before the first frame update
    public async Task PopulateLeaderBoard(Payload payload)
    {
         try
         {
            // first try to erase previous leaderboard
            this.eraseLeaderBoard();

            if(tilePrefab == null)
            {
                Debug.Log("NULL TILE PREFAB");
            }

            if(leaderBoardContent == null)
            {
                Debug.Log("NULL LEADERBOARD CONTENT");
            }
            if (payload.success)
            {
                foreach (Record record in payload.leaderBoardData.records)
                {
                    Debug.Log("added");
                    var leaderBoardScoreContainer = Instantiate(tilePrefab, leaderBoardContent.transform);
                    if (leaderBoardContent == null)
                    {
                        Debug.Log("leaderboard content is null");
                        continue;
                    }
                    leaderBoardScoreContainer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = record.rank.ToString();
                    leaderBoardScoreContainer.transform.GetChild(0).GetComponent<Image>().enabled = true;

                    leaderBoardScoreContainer.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = record.username;
                    leaderBoardScoreContainer.transform.GetChild(1).GetComponent<Image>().enabled = true;

                    leaderBoardScoreContainer.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = record.score.ToString();
                    leaderBoardScoreContainer.transform.GetChild(2).GetComponent<Image>().enabled = true;
                }
            }

         }
         catch (Exception ex)
         {
             Debug.Log("Exception in leader board population = " + ex.ToString());
         }
        
    }

    public void eraseLeaderBoard()
    {
        try
        {
            if(leaderBoardContent == null)
            {
                Debug.Log("Leadreboard content is null");
            }
            foreach (Transform child in leaderBoardContent.transform)
            {
                Destroy(child.gameObject);
            }
        }catch(Exception ex)
        {
            Debug.Log("Exception while erasing leaderboard");
            throw ex;
        }
    }

    // load leaderboard data from remote server
    public async Task<Payload> loadLeaderBoardData(string leaderBoardId)
    {
        try
        {
            Debug.Log("Let's load some data from leaderboard remote server");
            var payload = new Dictionary<string, string> { { "leaderBoardId", leaderBoardId} };
            var response = await nakamaInstance.client.RpcAsync(nakamaInstance.nakamaSession, "getLeaderBoardRpc", payload.ToJson());

            Debug.Log(response.Payload.ToJson().ToString());
            Payload payload1 = JsonUtility.FromJson<Payload>(response.Payload);
            return payload1;
        }
        catch(Exception ex)
        {
            throw ex;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        try
        {
            // 553 -> 373
            Debug.Log("Awake in leaderboard");
            
        }
        catch(Exception ex)
        {
            Debug.Log("Exception in leader board awake = " + ex.ToString());
        }
    }
    void Start()
    {
        Debug.Log("Start LeaderBoard");
        nakamaInstance = CustomNakamaConnection.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
