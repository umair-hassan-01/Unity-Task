using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using Nakama;
using Nakama.TinyJson;
using UnityEngine.UI;

public class ControlUtils
{
    public ControlUtils()
    {
        
    }

    // a generic function to toggle canvas
    public void SetComponentActive(int componentIndex , string[]items)
    {
        try
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (i == componentIndex)
                {
                    GameObject.Find(items[i]).GetComponent<Canvas>().enabled = true;
                }
                else
                {
                    GameObject.Find(items[i]).GetComponent<Canvas>().enabled = false;
                }
            }

        }
        catch (Exception ex)
        {
            // catch this exception in calling function....
            throw ex;
        }
    }
}
