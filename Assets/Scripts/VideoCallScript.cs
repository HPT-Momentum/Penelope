using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using agora_gaming_rtc;
using UnityEngine.UI;

public class VideoCallScript : MonoBehaviour
{
    private string channelName;
    private string channelToken;
    private uint playerUid;
    
    private IRtcEngine mRtcEngine;

    public void Setup(string agoraAppID, string channelName, string channelToken, uint playerUid)
    {
        this.channelName = channelName;
        this.channelToken = channelToken;
        this.playerUid = playerUid;
        
        mRtcEngine = IRtcEngine.GetEngine(agoraAppID);

        mRtcEngine.OnUserJoined = OnUserJoined;
        mRtcEngine.OnUserOffline = OnUserOffline;
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;

        Join();
    }

    void Join()
    {
        mRtcEngine.EnableVideo();
        mRtcEngine.EnableVideoObserver();
        
        // the agoraUid cannot be 0 so the playerUid is incremented with 1
        var agoraUid = playerUid + 1;
        mRtcEngine.JoinChannelByKey(channelToken, channelName, "", agoraUid);
    }

    void Leave()
    {
        mRtcEngine.LeaveChannel();
        mRtcEngine.DisableVideo();
        mRtcEngine.DisableVideoObserver();
    }

    void OnJoinChannelSuccessHandler(string channelName, uint uid, int elapsed)
    {
        Debug.LogFormat("Joined channel {0} successful, my Agora uid = {1}", channelName, uid);
    }

    void OnLeaveChannelHandler(RtcStats stats)
    {
        PlayerScript[] players = FindObjectsOfType<PlayerScript>();
        
        foreach (PlayerScript player in players)
        {
            VideoSurface remoteView = player.playerFace.GetComponent<VideoSurface>();
            remoteView.SetEnable(false);
        }
    }

    void OnUserJoined(uint agoraUid, int elapsed)
    {
        PlayerScript[] players = FindObjectsOfType<PlayerScript>();
        
        foreach (PlayerScript player in players)
        {
            // the agoraUid cannot be 0 so the playerUid is incremented with 1
            if (player.playerUid+1 == agoraUid)
            {
                if (player.playerFace.GetComponent<VideoSurface>() == null)
                {
                    VideoSurface remoteView = player.playerFace.AddComponent<VideoSurface>();
                    remoteView.SetForUser(agoraUid);
                    remoteView.SetEnable(true);
                    remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);
                    remoteView.SetGameFps(30);
                }
            }
        }
    }

    void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        PlayerScript[] players = FindObjectsOfType<PlayerScript>();
        
        foreach (PlayerScript player in players)
        {
            if (player.playerUid == uid)
            {
                VideoSurface remoteView = player.playerFace.GetComponent<VideoSurface>();
                remoteView.SetEnable(false);
            }
        }
    }
    
    void OnApplicationQuit()
    {
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy(); 
            mRtcEngine = null;
        }
    }
}
