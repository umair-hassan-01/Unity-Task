using System;
using UnityEngine;
using System.Threading.Tasks;
using Nakama;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class AuthManager
{
    private CustomNakamaConnection nakamaConnection;
    private User user;
    public AuthManager(CustomNakamaConnection nakamaConnection)
    {
        Debug.Log("Auth Manager is created");
        this.nakamaConnection = nakamaConnection;
        user = new User();
    }

    public async Task AuthenticateUserDevice()
    {
        try
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            Debug.Log("Try to auth user device");
            string userName = "umair.hassan03";
            bool createNew = true;

            nakamaConnection.nakamaSession =  await nakamaConnection.client.AuthenticateDeviceAsync(deviceId , userName , createNew);
            await nakamaConnection.connectSocket();

            Debug.Log("User device is authenticated");
        }catch(Exception ex)
        {
            // handle it in calling function
            Debug.Log("Exception in user auth using device");
            throw ex;
        }
    }
    

}
