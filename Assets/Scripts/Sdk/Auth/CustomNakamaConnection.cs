using Nakama;
using UnityEngine;
using System;

public class CustomNakamaConnection : MonoBehaviour
{
    public static CustomNakamaConnection instance;
    public IClient client;
    public ISession nakamaSession;
    
    private string Scheme = "http";
    private string Host = "localhost";
    private int Port = 7350;
    private string ServerKey = "defaultkey";


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
    }
}
