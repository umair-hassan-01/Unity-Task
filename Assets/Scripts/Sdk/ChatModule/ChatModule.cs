using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Nakama.TinyJson;
using Nakama;

public class ChatModule
{

    public ChatModule()
    {

    }

    // to parse incoming message data
    public class Message
    {
        public string message;
    }

    public bool validateChatMessage(string message)
    {
        if (message.Length > 5 && message.Length < 301)
            return true;
        return false;
    }

    public async void sendMessage(string channelId, string message , ISocket socket)
    {
        try
        {
            if (this.validateChatMessage(message))
            {
                var messageJsonContent = new Dictionary<string, string> { { "message", message } }.ToJson();
                
                var sendAck = await socket.WriteChatMessageAsync(channelId, messageJsonContent);

                Debug.Log(sendAck.ToString());
            }
            else
            {
                throw new Exception("message must have at least 6 characters and at most 300 characters");
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    // return the time to be displayed for a message
    public string getMessageTime(string actualTime)
    {
        // first convert into daytime
        DateTime dateTime;
        DateTime.TryParse(actualTime, out dateTime);

        int hour = dateTime.Hour % 12;
        if (hour == 0)
            hour = 12;
        return hour.ToString();
    }
}
