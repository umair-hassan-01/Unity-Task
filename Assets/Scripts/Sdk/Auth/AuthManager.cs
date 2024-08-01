using System;
using UnityEngine;
using System;
using System.Threading.Tasks;
using Nakama;

public class AuthManager
{
    public AuthManager()
    {
        Debug.Log("Auth Manager is created");
    }

    private string Scheme = "http";
    private string Host = "localhost";
    private int Port = 7350;
    private string ServerKey = "defaultkey";

    private string UserSessionKey = "some_key_value_here";

    public IClient Client;
    public ISession Session;

    public async Task AuthenticateUserDevice()
    {
        
        try
        {
            Client = new Nakama.Client(this.Scheme, this.Host, this.Port, this.ServerKey, UnityWebRequestAdapter.Instance);

            Client.Timeout = 3 * 60;

            Debug.Log("Time out = " + Client.Timeout);

            var AuthToken = PlayerPrefs.GetString(UserSessionKey);

            Debug.Log("Auth token was " + AuthToken);
            if (!String.IsNullOrEmpty(AuthToken))
            {
                // then user already has a session
                var ExistingSession = Nakama.Session.Restore(AuthToken);
                if (!ExistingSession.IsExpired)
                {
                    this.Session = ExistingSession;
                    Debug.Log("Got existing Session");
                }
            }

            // if there is no session the authenticate the user and make a new session
            if(this.Session == null)
            {
                string DeviceId = SystemInfo.deviceUniqueIdentifier;
                Debug.Log("Devide id was " + DeviceId);
                this.Session = await Client.AuthenticateDeviceAsync(DeviceId);

                // store auth token in player prefs so that we can verify it next time
                PlayerPrefs.SetString(this.UserSessionKey, this.Session.AuthToken);

            }
        }catch(Exception ex)
        {
            Debug.Log("Exception in user auth");
            Debug.Log("exception message = " + ex.Message);
        }
    }

}
