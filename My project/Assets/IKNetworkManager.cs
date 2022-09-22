using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor.PackageManager;

public class IKNetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAtStartup = true;
    static string targetURL = "http://192.168.0.161:8200/poker_server/gateway_servlet";     // ���� ����
    private Socket clientSocket = null;
    static string DefaultServerIP = "192.168.0.161";
    static int DefaultServerPort = 8200;
    void Start()
    {
        InitClientSocket();
        //StartCoroutine(test());

        test();
    }
    
    private void InitClientSocket()
    {
        this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress serverIPAdress = IPAddress.Parse(DefaultServerIP);
        IPEndPoint serverEndPoint = new IPEndPoint(serverIPAdress, DefaultServerPort);

        //������ ���� ��û
        try
        {
            Debug.Log("Connecting to Server");
            this.clientSocket.Connect(serverEndPoint);
        }
        catch (SocketException e)
        {
            Debug.Log("Connection Failed:" + e.Message);
        }
    }

    private void OnApplicationQuit()
    {
        if (this.clientSocket != null)
        {
            this.clientSocket.Close();
            this.clientSocket = null;
        }
    }

    public void Send(byte[] packet)
    {
        if (clientSocket == null)
        {
            return;
        }
        byte[] sendData = (packet);
        byte[] prefSize = new byte[1];
        prefSize[0] = (byte)sendData.Length;    //������ ���� �պκп� �� ������ ���̿� ���� ������ �ִµ� �̰��� 
        clientSocket.Send(prefSize);    //���� ������.
        clientSocket.Send(sendData);

    }

    public IEnumerator UnityWebRequestGet()
    {
        string url = targetURL;
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if(www.error == null)
        {
            
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error From Server");
        }

    }
    public IEnumerator WWW_WebRequestGet()
    {
        string url = targetURL;

        //www��� ���
        WWW www = new WWW(url);

        yield return www;

        if (www.error == null)
        {

            Debug.Log(www.text);
        }
        else
        {
            Debug.Log("Error From Server");
        }

    }
    public IEnumerator UnityWebRequestPost()
    {
        string url = targetURL;
        WWWForm form = new WWWForm();
        string id = "admin";
        string pw = "pw";
        form.AddField("Username", id);
        form.AddField("Password", pw);

        //www��� ���
        UnityWebRequest www = UnityWebRequest.Post(url,form);

        yield return www.SendWebRequest();

        if (www.error == null)
        {

            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("Error From Server");
        }
    }
    public IEnumerator WWW_WebRequestPost()
    {
        string url = "";
        WWWForm form = new WWWForm();
        string id = "admin";
        string pw = "pw";
        form.AddField("Username", id);
        form.AddField("Password", pw);
        WWW www = new WWW(url);
        yield return www;

        if(www.error == null)
        {
            Debug.Log(www.text);
        }
        else
        {
            Debug.Log("Error");
        }
    }

    public void test()
    {
        // serialize
        W_NoticeInfoReq.W_NoticeInfoReq req = new W_NoticeInfoReq.W_NoticeInfoReq() { };

        System.IO.MemoryStream reqStream = new System.IO.MemoryStream();
        ProtoBuf.Serializer.Serialize<W_NoticeInfoReq.W_NoticeInfoReq>(reqStream, req);
        //BinaryFormatter formatter = new BinaryFormatter();
        //formatter.Serialize(reqStream, req);

        byte[] result = new byte[reqStream.Length];
        reqStream.Position = 0;
        reqStream.Read(result, 0, result.Length);
        
        byte[] buffer = reqStream.GetBuffer();
        Send(result);
    }

}
