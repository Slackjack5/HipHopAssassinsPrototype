using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

  //Timer / Text Box
  public float timeStart;
  public TextMeshProUGUI textBox;




  private State state;
  //A way for us to visualize the Rhythm
  public GameObject rhythmDetector;
  //Choosing a Pattern
  private bool patternChosen;
  private int currentPattern;
  public int patternStage = 1;
  private bool noisePlayed;
  private float loggedTime;
  //Changable
  [Range(0.00f,0.50f)]
  public float leaniancy = 0.1f;
  //Hit Points
  private float hitPoint1;
  private float hitPoint2;
  private float hitPoint3;


  private enum State
  {
    Waiting,
    EnemyRhythm,
    PlayerRhythm,
    Scoring
  }
  // Start is called before the first frame update
  void Start()
  {
    //Wait Until the First Beat
    state = State.Waiting;
    textBox.text = timeStart.ToString("F2");
  }


    // Update is called once per frame
  void Update()
  {

    textBox.text = timeStart.ToString("F2");
    //Once the First beat happens Start the Game
    if (GlobalVariables.gameStarted == true)
    {
      state = State.EnemyRhythm;
    }


    switch (state)
    {
      case State.Waiting:
        break;
      case State.EnemyRhythm:
        if (patternChosen == false) { currentPattern = Random.Range(1, 2); patternChosen = true; Debug.Log("Pattern Chosen: "+currentPattern); }

        if (noisePlayed)
        {
          if(timeStart>=loggedTime+.3)
          {
            noisePlayed = false;
          }
        }

        //Pattern 1 (Quarter Note Triplets)
        if (currentPattern==1) //Current Rhthym
        {
          //Initializing Hit Points
          hitPoint1 = 0.0f;
          hitPoint2 = (AudioEvents.secondsPerBeat+(AudioEvents.secondsPerBeat/2));
          hitPoint3 = (AudioEvents.secondsPerBar- AudioEvents.secondsPerBeat);
          Debug.Log("Point 1:" + hitPoint1);
          Debug.Log("Point 2:" + hitPoint2);
          Debug.Log("Point 3:" + hitPoint3);
          if (timeStart == hitPoint1) { AkSoundEngine.PostEvent("Play_Beep", gameObject); }
          if (timeStart >= hitPoint2 && timeStart <= hitPoint2 + .1) 
          { 
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint3 && timeStart <= hitPoint3 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          timeStart += Time.deltaTime; //Start Counting
        }
        break;
    }
  }
}
