using System;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Nakama;
using UnityEngine.SceneManagement;

public class AuthManager
{
    private CustomNakamaConnection nakamaConnection;

    public AuthManager(CustomNakamaConnection nakamaConnection)
    {
        Debug.Log("Auth Manager is created");
        this.nakamaConnection = nakamaConnection;
    }

    public async Task AuthenticateUserDevice()
    {
        try
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            Debug.Log("Try to auth user device");
            nakamaConnection.nakamaSession =  await nakamaConnection.client.AuthenticateDeviceAsync(deviceId);
       
            Debug.Log("User device is authenticated");
        }catch(Exception ex)
        {
            // handle it in calling function
            Debug.Log("Exception in user auth using device");
            throw ex;
        }
    }
    

}
