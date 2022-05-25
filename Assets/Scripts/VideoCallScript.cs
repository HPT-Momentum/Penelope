using UnityEngine;
using agora_gaming_rtc;

public class VideoCallScript : MonoBehaviour
{
    private string channelName;
    private string channelToken;
    
    private IRtcEngine mRtcEngine;

    public void Setup(string agoraAppID, string channelName, string channelToken)
    {
        this.channelName = channelName;
        this.channelToken = channelToken;
        
        mRtcEngine = IRtcEngine.GetEngine(agoraAppID);

        mRtcEngine.OnUserJoined = OnUserJoined;
        mRtcEngine.OnUserOffline = OnUserOffline;
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccessHandler;
        mRtcEngine.OnLeaveChannel = OnLeaveChannelHandler;
    }

    public void Join(uint playerUid)
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
        Debug.Log($"New player joined Agora chat with id {agoraUid}");
        PlayerScript[] players = FindObjectsOfType<PlayerScript>();
        
        foreach (PlayerScript player in players)
        {
            Debug.Log($"Checking player with id {player.playerUid.Value}");
            // the agoraUid cannot be 0 so the playerUid is incremented with 1
            if (player.playerUid.Value+1 == agoraUid)
            {
                if (player.playerFace.GetComponent<VideoSurface>() == null)
                {
                    VideoSurface remoteView = player.playerFace.AddComponent<VideoSurface>();
                    remoteView.SetForUser(agoraUid);
                    remoteView.SetEnable(true);
                    remoteView.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);
                    remoteView.SetGameFps(30);
                    
                    Debug.Log($"VideoSurface added to player {player.playerUid.Value}");
                }
            }
        }
    }

    void OnUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        PlayerScript[] players = FindObjectsOfType<PlayerScript>();
        
        foreach (PlayerScript player in players)
        {
            if (player.playerUid.Value == uid)
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
