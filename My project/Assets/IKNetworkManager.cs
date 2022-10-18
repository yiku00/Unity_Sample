using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Net.Sockets;
using System.Net;
using System.IO;

public class IKNetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isAtStartup = true;
    static string targetURL = "http://192.168.0.161:8200/poker_server/gateway_servlet";     // 내부 서버
    private Socket clientSocket = null;
    private TcpClient ClientTCPSocket = null;
    private NetworkStream stream;
    private StreamWriter STWriter;
    private StreamReader STReader;
    bool IsSocketReady = false;

    static string DefaultServerIP = "192.168.0.161";
    static int DefaultServerPort = 8200;

    static string ClientIP = "";
    static int ClientPort = 0;

    void Start()
    {
        InitClientSocket();
        //StartCoroutine(test());
        
        test();
    }

    void Update()
    {
        if(IsSocketReady && stream.DataAvailable)
        {
            //There is Some Data From Server
            string data = STReader.ReadLine();
            if(data != null)
            {
                //Handle Data From Server
                Debug.Log("Data From Server = " + data);
            }
        }
    }
    
    private void InitClientSocket()
    {
        //서버로 연결 요청
        try
        {
            Debug.Log("Connecting to Server");
            this.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress serverIPAdress = IPAddress.Parse(DefaultServerIP);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIPAdress, DefaultServerPort);
            this.clientSocket.Connect(serverEndPoint);
            this.ClientTCPSocket = new TcpClient(DefaultServerIP, DefaultServerPort);
            stream = ClientTCPSocket.GetStream();
            STWriter = new StreamWriter(stream);
            STReader = new StreamReader(stream);
            IsSocketReady = true;
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
            this.STWriter.Close();
            this.STReader.Close();

            this.STWriter = null;
            this.STReader = null;
            this.clientSocket = null;

            IsSocketReady = false;
        }
    }

    public void SendPacket(byte[] packet)
    {
        if (!IsSocketReady)
        {
            Debug.Log("Socket Is Null");
            return;
        }

        byte[] sendData = (packet);
        byte[] prefSize = new byte[1];
        prefSize[0] = (byte)sendData.Length;    //버퍼의 가장 앞부분에 이 버퍼의 길이에 대한 정보가 있는데 이것을 
        Debug.Log("Packet To Send ="+ packet.ToString());
        clientSocket.Send(prefSize);    //먼저 보낸다.
        clientSocket.Send(sendData);
        STWriter.Write(sendData);
        STWriter.Flush();

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

    [System.Obsolete]
    public IEnumerator WWW_WebRequestGet()
    {
        string url = targetURL;

        //www방식 통신
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

        //www방식 통신
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
    [System.Obsolete]
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
        SendPacket(result);
    }

}
