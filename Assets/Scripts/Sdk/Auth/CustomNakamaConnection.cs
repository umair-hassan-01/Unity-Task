using Nakama;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class CustomNakamaConnection : MonoBehaviour
{
    public static CustomNakamaConnection instance;
    public IClient client;
    public ISession nakamaSession;
    public ISocket socket;
    public string channelId;
    
    private string Scheme = GameConstants.SCHEME;
    private string Host = GameConstants.HOST;
    private int Port = GameConstants.PORT;
    private string ServerKey =GameConstants.SERVER_KEY;


    public static CustomNakamaConnection Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CustomNakamaConnection>();
                if(Instance == null)
                {
                    GameObject connectionObject = new GameObject("CustomNakamaConnection");
                    instance = connectionObject.AddComponent<CustomNakamaConnection>();
                    DontDestroyOnLoad(connectionObject);

                }
            }

            return instance;
        }
    }

    public async Task connectSocket()
    {
        try
        {
            await socket.ConnectAsync(nakamaSession, true);
            Debug.Log("Socket is connected");
        }
        catch (Exception ex)
        {
            Debug.Log("Exception while connecting to socket = " + ex.ToString());
            throw ex;
        }
    }

    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        Debug.Log("Oh let's get the new client");
        client = new Nakama.Client(Scheme, Host, Port, ServerKey, UnityWebRequestAdapter.Instance);
        socket = client.NewSocket();
        Debug.Log(socket.ToString());

    }
}
