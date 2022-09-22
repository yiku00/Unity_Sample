using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ProtoContract]
[Serializable]
public class HTTPMessage : MonoBehaviour
{

	public enum Code
	{
		Code_Unknown = 0,
		Code_OK,
		Code_InvalidSession,
		Code_InvalidSignature,
		Code_InvalidTicket,
		Code_EmptyResponse,
		Code_InvalidProtocol,
		Code_InvalidDataHash,
		Code_InvalidVersion,
		Code_InvalidLive,
		Code_HttpError,
		Code_CurlError,
	};


	private string mServlet;
	private Code mCode;
	private string mType;
	private char mData;
	private int mSize;
	private System.Object mTarget;
	private bool mWait;
	private bool mRetry;
	private long mState;

    public Code Code_OK { get; }

    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
