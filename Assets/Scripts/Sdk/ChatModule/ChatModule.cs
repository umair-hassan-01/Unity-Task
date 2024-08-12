using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Nakama.TinyJson;
using Nakama;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

public class ChatModule
{
    private Dictionary<string, KeyValuePair<string, int>> userInfoCache;
    private User userServices;
    // to parse incoming message data
    public class Message
    {
        public string message;
    }

    public ChatModule()
    {
        this.userInfoCache = new Dictionary<string, KeyValuePair<string, int>>();
        this.userServices = new User();
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

    // generate channel id for direct chat using id of both users....
    public string generateChannelId(string userA , string userB)
    {
        int compare = string.Compare(userA, userB);

        // firstId < secondId 
        if (compare == -1)
        {
            string temp = userA;
            userA = userB;
            userB = temp;
        }

        string channelId = userA + userB;
        /*string oldC = channelId;
        while (channelId.Length > 63)
            channelId = channelId.Substring(20);
        */
        string safeChannelId = channelId;
        using(var sha = SHA256.Create())
        {
            var bytes = Encoding.UTF8.GetBytes(channelId);
            channelId =  Convert.ToBase64String(sha.ComputeHash(bytes));
            // make SHA256 hashed value safe
            string tempId = channelId.Replace('/', '-');
            safeChannelId = tempId.Replace('\\', '-');
        }

        Debug.Log("Channel id " + safeChannelId);
        return safeChannelId;
    }

    // get user information[displayName , avatarUrl]
    public async Task<KeyValuePair<string , int>> getUserInfo(string userId)
    {
        try
        {
            KeyValuePair<string, int> userInformation;

            // first try cached memory...
            if (this.userInfoCache.ContainsKey(userId))
            {
                Debug.Log("Cache hit successfuly");
                userInformation = this.userInfoCache[userId];
            }
            else
            {
                // else fetch it from remote server and store in cache..
                var userAccount = await userServices.fetchUserAccount(userId);
                userInformation = new KeyValuePair<string, int>(userAccount.displayName, userAccount.avatarUrl);
                this.userInfoCache[userId] = userInformation;
            }

            return userInformation;
        }catch(Exception ex)
        {
            throw ex;
        }
    }

    // clear user information cache....
    public void clearUserInfoCache()
    {
        try
        {
            this.userInfoCache.Clear();
        }catch(Exception ex)
        {
            throw ex;
        }
    }

}
