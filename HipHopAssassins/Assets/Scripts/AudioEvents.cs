using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EZCameraShake;

public class AudioEvents : MonoBehaviour
{
  public AK.Wwise.Event rhythmHeckinEvent;
  public UnityEvent OnLevelEnded;
  [HideInInspector] public static float secondsPerBeat;
  [HideInInspector] public static float secondsPerBar;
  [HideInInspector] public static float BPM;
  public UnityEvent OnEveryGrid;
  public UnityEvent OnEveryBeat;
  public UnityEvent OnEveryBar;
  public UnityEvent OnR;
  public UnityEvent OnSubtractTime;
  public int GridCount = 0;
  public int gridCounter = 0;
  public bool startCounting = false;
  private bool playerReady = false;
  private bool musicReady = false;
  private bool isMusicMuted = false;
  //Timing
  public int[] songTiming;
  [HideInInspector]public int currentBar= GlobalVariables.currentBar;
  [HideInInspector] public int currentBeat= GlobalVariables.currentBeat;
  [HideInInspector] public int currentGrid= GlobalVariables.currentGrid;
  public bool gameStarted = GlobalVariables.gameStarted;

  //id of the wwise event - using this to get the playback position
  static uint playingID;

  private void Start()
  {
    playingID = rhythmHeckinEvent.Post(gameObject, (uint)(AkCallbackType.AK_MusicSyncAll | AkCallbackType.AK_EnableGetMusicPlayPosition), MusicCallbackFunction);
    GlobalVariables.gameStarted = false;
  }

  private void Update()
  {
    currentBar = GlobalVariables.currentBar;
    currentBeat = GlobalVariables.currentBeat;
    currentGrid = GlobalVariables.currentGrid;
  }

  public void EndAudio()
  {
    //End All Audio
    AkSoundEngine.PostEvent("Stop_Audio", gameObject);
    AkSoundEngine.PostEvent("Stop_All", gameObject);
  }
  public void PauseAudio()
  {
    //End All Audio
   // AkSoundEngine.PostEvent("Pause_All", gameObject);
    AkSoundEngine.PostEvent("Pause_Audio", gameObject);
  }
  public void ResumeAudio()
  {
    //End All Audio
    AkSoundEngine.PostEvent("Resume_Audio", gameObject);
  }
  public void FadeAudio()
  {
    //End All Audio
    AkSoundEngine.PostEvent("Fade_Audio", gameObject);
  }

  public void ToggleMute()
  {
    if (isMusicMuted)
    {
      isMusicMuted = false;
      AkSoundEngine.PostEvent("Reset_Music", gameObject);
    }
    else
    {
      isMusicMuted = true;
      AkSoundEngine.PostEvent("Mute_Music", gameObject);
    }
  }

  void MusicCallbackFunction(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
  {

    AkMusicSyncCallbackInfo _musicInfo = (AkMusicSyncCallbackInfo)in_info;

    switch (_musicInfo.musicSyncType)
    {
      case AkCallbackType.AK_MusicSyncUserCue:

        CustomCues(_musicInfo.userCueName, _musicInfo);

        secondsPerBeat = _musicInfo.segmentInfo_fBeatDuration;
        secondsPerBar = _musicInfo.segmentInfo_fBarDuration;
        BPM = _musicInfo.segmentInfo_fBeatDuration * 60f;
        break;
      case AkCallbackType.AK_MusicSyncBeat:


        OnEveryBeat.Invoke();
        break;
      case AkCallbackType.AK_MusicSyncBar:
        //I want to make sure that the secondsPerBeat is defined on our first measure.
        if (GlobalVariables.gameStarted == false) { GlobalVariables.gameStarted = true; } //If the game hasn't started yet, start it on beat 1
        secondsPerBeat = _musicInfo.segmentInfo_fBeatDuration;
        secondsPerBar = _musicInfo.segmentInfo_fBarDuration;
        Debug.Log("Seconds Per Bar: " + secondsPerBar);
        Debug.Log("Seconds Per Beat: " + secondsPerBeat);
        OnEveryBar.Invoke();
        break;

      case AkCallbackType.AK_MusicSyncGrid:
        OnEveryGrid.Invoke();
        break;
      default:
        break;

    }

  }

  public void IncreaseBar()
  {
    if(GlobalVariables.currentBar <4)//Insert Time Signature
    {
      GlobalVariables.currentBar += 1;
    }
    else
    {
      GlobalVariables.currentBar = 1;
    }
      
  }

  public void IncreaseBeat()
  {
    if (GlobalVariables.currentBeat < 4)//Insert Time Signature
    {
      GlobalVariables.currentBeat += 1;
    }
    else
    {
      GlobalVariables.currentBeat = 1;
    }

  }

  public void IncreaseGrid()
  {
    if (GlobalVariables.currentGrid < 4)//Insert Time Signature
    {
      GlobalVariables.currentGrid += 1;
    }
    else
    {
      GlobalVariables.currentGrid = 1;
    }

  }


  public void CustomCues(string cueName, AkMusicSyncCallbackInfo _musicInfo)
  {
    switch (cueName)
    {
      case "R":
        if(playerReady==true)
        {
          AkSoundEngine.SetSwitch("GamePlay_Switch", "Active", gameObject);
          
        }
        break;
      case "Ready":
        OnSubtractTime.Invoke();
        Debug.Log("Yay: Ready");
        break;
      case "3":
        OnSubtractTime.Invoke();
        Debug.Log("Yay: 3");
        break;
      case "2":
        OnSubtractTime.Invoke();
        Debug.Log("Yay: 2");
        break;
      case "1":
        OnSubtractTime.Invoke();
        Debug.Log("Yay: 1");
        break;
      case "Go":
        OnSubtractTime.Invoke();
        Debug.Log("Yay: Go!");
        break;
      default:
        break;
    }
  }

  public void Bump()
  {
    //Camera Shake
    CameraShaker.Instance.ShakeOnce(4f, 0.1f, .1f, .1f);
  }

  public void StartLevel()
  {
    playerReady = true;
  }


}

public static class GlobalVariables
{
  public static int currentBar;
  public static int currentBeat;
  public static int currentGrid;
  public static bool gameStarted;
}
