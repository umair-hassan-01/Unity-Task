using UnityEngine;
using System.Collections;
using System;
using System.Threading.Tasks;
using Nakama;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    private CustomNakamaConnection nakamaInstance;
    private User userSerives;

    public GameObject senderMessageTemplate;
    public GameObject receiveMessageTemplate;
    public GameObject messageContent;

    public Text groupName;
    public Image groupImage;

    public MainMenuScript mainMenuScript;


    public void Start()
    {
        nakamaInstance = CustomNakamaConnection.Instance;
        userSerives = new User();
    }

    // display a single message on screen....
    public async Task populateChatContent(IApiChannelMessage channelMessage , ChatModule chatModule)
    {
        try
        {
            GameObject messageTile = null;
            if (channelMessage.SenderId.Equals(nakamaInstance.nakamaSession.UserId))
            {
                messageTile = Instantiate(senderMessageTemplate, messageContent.transform);
            }
            else
            {
                messageTile = Instantiate(receiveMessageTemplate, messageContent.transform);
            }

            var parsedMessage = JsonUtility.FromJson<ChatModule.Message>(channelMessage.Content);

            // get details of current user to fill out UI....
            var userAccount = await chatModule.getUserInfo(channelMessage.SenderId);

            if (messageTile != null)
            {
                messageTile.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = parsedMessage.message;
                messageTile.transform.GetChild(2).GetComponent<Text>().text = userAccount.Key;
                messageTile.transform.GetChild(1).GetComponent<Image>().sprite = mainMenuScript.icons[userAccount.Value];
            }

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }



    // clear chat canvas
    public void clearChatCanvas()
    {
        // first remove old messages to avoid duplication
        foreach (Transform child in this.messageContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void setGroupBanner(string groupName , int avatarIndex)
    {
        this.groupName.text = groupName;
        this.groupImage.sprite = mainMenuScript.icons[avatarIndex];
    }
}
