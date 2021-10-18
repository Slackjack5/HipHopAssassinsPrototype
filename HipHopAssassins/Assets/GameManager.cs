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
  private float hitPoint4;
  private float hitPoint5;
  private float hitPoint6;
  //States
  private int currentState;
  public bool gameplayStarted;

  //Scoring
  private bool rhythmMiss;
  private bool lockedOut;
  public int score;

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
    currentState = 1;
    textBox.text = timeStart.ToString("F2");
  }

  public void SwapTurns()
  {

    if (GlobalVariables.gameStarted == true)
    {
      Debug.Log("swapping turns");
      //Reset Varaibles and Swap Turns
      if (currentState == 1)
      {
        timeStart = 0;
        state = State.PlayerRhythm;
      }
      else if (currentState == 2)
      {
        timeStart = 0;
        patternChosen = false;
        state = State.EnemyRhythm;
      }
    }

  }

  // Update is called once per frame
  void Update()
  {

    textBox.text = timeStart.ToString("F2");
    //Once the First beat happens Start the Game
    if (GlobalVariables.gameStarted == true)
    {
      if (gameplayStarted == false) { gameplayStarted=true; state = State.EnemyRhythm; }
    }


    switch (state)
    {
      case State.Waiting:
        currentState = 0;
        break;
      case State.EnemyRhythm:
        currentState = 1;
        if (patternChosen == false) { currentPattern = Random.Range(1, 4); patternChosen = true; Debug.Log("Pattern Chosen: "+currentPattern); }

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
          //Audio
          if (timeStart == hitPoint1) { AkSoundEngine.PostEvent("Play_Beep", gameObject); }
          if (timeStart >= hitPoint2 && timeStart <= hitPoint2 + .1) 
          { 
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint3 && timeStart <= hitPoint3 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          //Visual
          DisplayOpening();
          timeStart += Time.deltaTime; //Start Counting
        }
        else if (currentPattern == 2) //Current Rhthym
        {
          //Initializing Hit Points
          hitPoint1 = 0.0f;
          hitPoint2 = (AudioEvents.secondsPerBar / 6);
          hitPoint3 = ((AudioEvents.secondsPerBar / 6)*2);
          hitPoint4 = ((AudioEvents.secondsPerBar / 6) * 3);
          hitPoint5 = ((AudioEvents.secondsPerBar / 6) * 4);
          hitPoint6 = ((AudioEvents.secondsPerBar / 6) * 5);

          //Audio

          if (timeStart == hitPoint1) { AkSoundEngine.PostEvent("Play_Beep", gameObject); }
          if (timeStart >= hitPoint2 && timeStart <= hitPoint2 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint3 && timeStart <= hitPoint3 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint4 && timeStart <= hitPoint4 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint5 && timeStart <= hitPoint5 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint6 && timeStart <= hitPoint6 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          //Visual
          DisplayOpening();

          timeStart += Time.deltaTime; //Start Counting
        }
        else if (currentPattern == 3)
        {
          //Initializing Hit Points
          hitPoint1 = 0.0f;
          hitPoint2 = (AudioEvents.secondsPerBar / 6);
          hitPoint3 = ((AudioEvents.secondsPerBar / 6) * 2);
          hitPoint4 = ((AudioEvents.secondsPerBeat) * 2);
          hitPoint5 = ((AudioEvents.secondsPerBeat) * 3);
          hitPoint6 = 10000; //UNABTAINABLE
          //Audio

          if (timeStart == hitPoint1) { AkSoundEngine.PostEvent("Play_Beep", gameObject); }
          if (timeStart >= hitPoint2 && timeStart <= hitPoint2 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint3 && timeStart <= hitPoint3 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint4 && timeStart <= hitPoint4 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }
          if (timeStart >= hitPoint5 && timeStart <= hitPoint5 + .1)
          {
            if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; }
          }

          //Visual
          DisplayOpening();
          timeStart += Time.deltaTime; //Start Counting

        }

          break;
      case State.PlayerRhythm: ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        currentState = 2;
        //Visual
        DisplayOpening();
        //Pattern 1 5:4 p
        if (currentPattern == 1) //Current Rhthym
        {

          //Visual
          if (timeStart <= hitPoint1 + leaniancy) //First hit Timing
          {
            textBox.color = Color.green;
            rhythmMiss = false;
            RhythmTiming();
          }
          else if (timeStart >= hitPoint2 - leaniancy && timeStart <= hitPoint2 + leaniancy) //Second hit Timing
          {
            textBox.color = Color.green;
            rhythmMiss = false;
            RhythmTiming();
          }
          else if (timeStart >= hitPoint3 - leaniancy && timeStart <= hitPoint3 + leaniancy) //Third hit Timing
          {
            textBox.color = Color.green;
            rhythmMiss = false;
            RhythmTiming();
          }
          else
          {
            textBox.color = Color.red;
            rhythmMiss = true;
            lockedOut = false;
            RhythmTiming();
          }
        }

        timeStart += Time.deltaTime; //Start Counting
        break;
    }
    
  }

  public void RhythmTiming()
  {
    if (Input.GetKeyDown("space")) //If the player presses space
    {
      if(rhythmMiss==false) //If they should mose, subtract a point. If they are in time give a point
      {
        if(lockedOut)
        {

        }
        else
        {
          score += 1;
          lockedOut = true;
        }
      }
      else
      {
        score -= 1;
        lockedOut = true;
      }
    }
  }

  public void DisplayOpening()
  {
    //Visual
    if (timeStart <= hitPoint1 + leaniancy)
    {
      textBox.color = Color.green;
    }
    else if (timeStart >= hitPoint2 - leaniancy && timeStart <= hitPoint2 + leaniancy)
    {
      textBox.color = Color.green;
    }
    else if (timeStart >= hitPoint3 - leaniancy && timeStart <= hitPoint3 + leaniancy)
    {
      textBox.color = Color.green;
    }
    else if (timeStart >= hitPoint4 - leaniancy && timeStart <= hitPoint4 + leaniancy)
    {
      textBox.color = Color.green;
    }
    else if (timeStart >= hitPoint5 - leaniancy && timeStart <= hitPoint5 + leaniancy)
    {
      textBox.color = Color.green;
    }
    else if (timeStart >= hitPoint6 - leaniancy && timeStart <= hitPoint6 + leaniancy)
    {
      textBox.color = Color.green;
    }
    else
    {
      textBox.color = Color.red;
    }
  }
  }
