using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using UnityEngine.UI;
using Nakama;
using System.Linq;
using System.Threading.Tasks;

public class ChatScript : MonoBehaviour
{
    private ChatModule chatModule;
    private CustomNakamaConnection nakamaInstance;

    public InputField messageInputTextField;
    public Text groupName;

    public MainMenuScript mainMenuScript;
    private ConcurrentQueue<IApiChannelMessage> pendingMessages;

    public ChatUI chatUI;

    public static int listening = 0;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Chat is starting");
        chatModule = new ChatModule();
        nakamaInstance = CustomNakamaConnection.Instance;
        this.pendingMessages = new ConcurrentQueue<IApiChannelMessage>();
    }


    private async void Update()
    {
        // check if socket is still connected....
        if (!nakamaInstance.socket.IsConnected)
        {
            await nakamaInstance.connectSocket(mainMenuScript);
        }

        IApiChannelMessage currentMessage;
        if(pendingMessages != null)
        {
            while(this.pendingMessages.Count > 0)
            {
                bool canDequeue = pendingMessages.TryDequeue(out currentMessage);
                await chatUI.populateChatContent(currentMessage , chatModule);
            }
        }
    }

    public void submitChatMessage()
    {
        try
        {
            string message = messageInputTextField.text;
            chatModule.sendMessage(nakamaInstance.channelId, message , nakamaInstance.socket);
            messageInputTextField.text = "";

        }catch(Exception ex)
        {
            Debug.Log(ex.ToString());
        }
    }

    public async void getOldMessages(string channelId)
    {
        try
        {
            // first clear the canvas....
            chatUI.clearChatCanvas();

            // also clear the cache
            chatModule.clearUserInfoCache();

            var result = await nakamaInstance.client.ListChannelMessagesAsync(nakamaInstance.nakamaSession, channelId, 100, true);
            foreach (var message in result.Messages)
            {
                pendingMessages.Enqueue(message);
            }

        }catch(Exception ex)
        {
            throw ex;
        }
    }

    public async void handleDirectChat(string otherUserId)
    {
        try
        {
            string firstId = nakamaInstance.nakamaSession.UserId;
            string secondId = otherUserId;
            string channelId = chatModule.generateChannelId(firstId, secondId);

            var userInfo = await chatModule.getUserInfo(otherUserId);
            chatUI.setGroupBanner(userInfo.Key, userInfo.Value);

            mainMenuScript.enterGlobalChat();
            await setChatEnvironment(channelId);
        }
        catch (Exception ex)
        {
            Debug.Log("Exception in handleDirectChat = " + ex);
        }
    }

    public async void handleGlobalChat()
    {
        try
        {
            chatUI.setGroupBanner("Global Room", 1);
            await setChatEnvironment(GameConstants.GLOBAL_CHAT_ROOM);
        }
        catch (Exception ex)
        {
            Debug.Log("Exception in handleGlobalChat = " + ex);
        }
    }

    // setup initial chat environment(joining group , group name , group id , fetching old messages etc.)
    public async Task setChatEnvironment(string channelId)
    {
        try
        {
            
            string groupId = channelId;
            bool persistence = true;
            bool hidden = false;
            var channel = await nakamaInstance.socket.JoinChatAsync(groupId, ChannelType.Room, persistence, hidden);
            nakamaInstance.channelId = channel.Id;
            Debug.Log("Channel id = " + channelId);

            this.getOldMessages(channel.Id);

            // handles incoming messages in channel
            if (listening == 0)
            {
                nakamaInstance.socket.ReceivedChannelMessage += message =>
                {
                    Debug.Log("Got message " + DateTime.Now);
                    // delegate message to execute on Main Thread
                    pendingMessages.Enqueue(message);
                };
                listening++;
            }

        }
        catch (Exception ex)
        {
            Debug.Log("Error in setChatEnvironment = " + ex);
        }
    }

}
