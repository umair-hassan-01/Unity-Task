using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Nakama;
using System.Threading.Tasks;


public class NakamaServerConnection
{
    private string scheme = "http";
    private string host = "";
    private int port = 7350;
    private string ServerKey = "defaultkey";


    private IClient Client;
    private ISession Session;

    private String SessionKey = "randomSessionKey";
    private String name = "nothing now";

    public async Task getLocalClient()
    {
        
    }
}
