using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Nakama;

public class ChatScript : MonoBehaviour
{
    private ChatModule chatModule;
    private CustomNakamaConnection nakamaInstance;

    public GameObject senderMessageTemplate;
    public GameObject receiveMessageTemplate;
    public GameObject messageContent;

    private Queue<IApiChannelMessage> pendingMessages;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Chat is starting");
        chatModule = new ChatModule();
        nakamaInstance = CustomNakamaConnection.Instance;
        this.pendingMessages = new Queue<IApiChannelMessage>();
    }

    private void Update()
    {
        if (pendingMessages != null)
        {
            lock (pendingMessages)
            {
                while (pendingMessages.Count > 0)
                {
                    populateChatContent(pendingMessages.Peek());
                    pendingMessages.Dequeue();
                }
            }
        }

    }

    public void populateChatContent(IApiChannelMessage channelMessage)
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
            
            if (messageTile != null)
            {
                messageTile.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = parsedMessage.message;
                messageTile.transform.GetChild(2).GetComponent<Text>().text = channelMessage.Username;
            }
            
        }catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void submitChatMessage()
    {
        try
        {
            Debug.Log("I will send chat message");
            var inputField = GameObject.Find("SendMessageField");
            string message = inputField.GetComponent<InputField>().text;
            
            chatModule.sendMessage(nakamaInstance.channelId, message , nakamaInstance.socket);
            inputField.GetComponent<InputField>().text = "";

        }catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public async void populateOldMessages(string channelId)
    {
        try
        {
            var result = await nakamaInstance.client.ListChannelMessagesAsync(nakamaInstance.nakamaSession, channelId, 100, true);

            lock (pendingMessages)
            {
                foreach (var message in result.Messages)
                {
                    pendingMessages.Enqueue(message);
                }
            }

        }catch(Exception ex)
        {
            throw ex;
        }
    }

    // setup initial chat environment(joining group , group name , group id , fetching old messages etc.)
    public async void setChatEnvironment()
    {
        try
        {
            string groupId = GameConstants.GLOBAL_CHAT_ROOM;
            bool persistence = true;
            bool hidden = false;
            var channel = await nakamaInstance.socket.JoinChatAsync(groupId, ChannelType.Room, persistence, hidden);
            nakamaInstance.channelId = channel.Id;

            // handles incoming messages in channel
            nakamaInstance.socket.ReceivedChannelMessage += message =>
            {
                lock (pendingMessages)
                {
                    pendingMessages.Enqueue(message);
                }
            };

            this.populateOldMessages(channel.Id);

        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

}
