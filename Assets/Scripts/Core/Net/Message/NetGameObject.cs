using UnityEngine;
using System.Collections;
public class NetGameObject : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
        GameClientNet.NetMsg.Init(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameClientNet.NetMsg.RecvMsg();
    }

    //	public static bool ConnectServer(){
    //		return GameClientNet.NetManager.GetInstance().ConnectServer("192.168.1.9",9001,200);
    //	}
    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        GameClientNet.NetManager.GetInstance().CloseNet();
    }
}
