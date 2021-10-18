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
  public TextMeshProUGUI scoreText;
  public GameObject PlayerVisual;



  private State state;
  //A way for us to visualize the Rhythm
  public GameObject rhythmDetector;
  public GameObject counterCube;
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

  //Numbers
  private bool numbersGenerated;
  private int[] randomNumbers=new int[6];
  public int currentNumber;
  //States
  private int currentState;
  public bool gameplayStarted;

  //Scoring
  private bool rhythmMiss;
  private bool lockedOut;
  public int score;
  //Cameras
  public GameObject cameraClose;
  public GameObject cameraFar;
  private bool cameraSwapped;

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
    GenerateNumbers();
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
        cameraSwapped = false;
        state = State.PlayerRhythm;
      }
      else if (currentState == 2)
      {
        timeStart = 0;
        patternChosen = false;
        numbersGenerated = false;
        cameraSwapped = false;
        state = State.EnemyRhythm;
      }
    }

  }

  // Update is called once per frame
  void Update()
  {
    //Visualization
    textBox.text = timeStart.ToString("F2");
    scoreText.text = score.ToString();
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
        if (!cameraSwapped)
        {
          //Camera
          cameraClose.SetActive(false);
          cameraFar.SetActive(true);
          cameraSwapped = true;
        }
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
          //Generate Numbers
          GenerateNumbers();
          //Initializing Hit Points
          hitPoint1 = 0.0f;
          hitPoint2 = (AudioEvents.secondsPerBeat+(AudioEvents.secondsPerBeat/2));
          hitPoint3 = (AudioEvents.secondsPerBar- AudioEvents.secondsPerBeat);

          //Audio
          playAudio();
          //Visual
          DisplayOpening();
          timeStart += Time.deltaTime; //Start Counting
        }
        else if (currentPattern == 2) //Current Rhthym
        {
          //Initializing Hit Points
          hitPoint1 = 0.0f;
          hitPoint2 = (AudioEvents.secondsPerBeat);
          hitPoint3 = (AudioEvents.secondsPerBeat * 3);


          //Audio
          playAudio();
          
          //Visual
          DisplayOpening();

          timeStart += Time.deltaTime; //Start Counting
        }
        else if (currentPattern == 3)
        {
          //Initializing Hit Points
          hitPoint1 = 0.0f;
          hitPoint2 = ((AudioEvents.secondsPerBeat*2)+ (AudioEvents.secondsPerBeat / 2));
          hitPoint3 = (AudioEvents.secondsPerBeat * 3);


          //Audio
          playAudio();

          //Visual
          DisplayOpening();
          timeStart += Time.deltaTime; //Start Counting

        }

          break;
      case State.PlayerRhythm: ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //Camera
        if(!cameraSwapped)
        {
          AkSoundEngine.PostEvent("Play_Woosh", gameObject);
          cameraClose.SetActive(true);
          cameraFar.SetActive(false);
          cameraSwapped = true;
        }


        currentState = 2;
        //Visual
        DisplayOpening();
        //Pattern 1 5:4 p
        if (currentPattern == 1) //Current Rhthym
        {
          PlayerTiming();
        }
        else if (currentPattern == 2)
        {
          PlayerTiming();
        }
        else if (currentPattern == 3)
        {
          PlayerTiming();
        }

        timeStart += Time.deltaTime; //Start Counting
        break;
    }
    
  }
  
  public void PlayerTiming()
  {
    //Visual
    if (timeStart <= hitPoint1 + leaniancy) //First hit Timing
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      rhythmMiss = false;
      RhythmTiming();
    }
    else if (timeStart >= hitPoint2 - leaniancy && timeStart <= hitPoint2 + leaniancy) //Second hit Timing
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      rhythmMiss = false;
      RhythmTiming();
    }
    else if (timeStart >= hitPoint3 - leaniancy && timeStart <= hitPoint3 + leaniancy) //Third hit Timing
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      rhythmMiss = false;
      RhythmTiming();
    }
    else if (timeStart >= hitPoint4 - leaniancy && timeStart <= hitPoint4 + leaniancy) //Third hit Timing
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      rhythmMiss = false;
      RhythmTiming();
    }
    else if (timeStart >= hitPoint5 - leaniancy && timeStart <= hitPoint5 + leaniancy) //Third hit Timing
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      rhythmMiss = false;
      RhythmTiming();
    }
    else if (timeStart >= hitPoint6 - leaniancy && timeStart <= hitPoint6 + leaniancy) //Third hit Timing
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      rhythmMiss = false;
      RhythmTiming();
    }
    else
    {
      textBox.color = Color.red;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.black;
      rhythmMiss = true;
      lockedOut = false;
      RhythmTiming();
    }
  }
  public void RhythmTiming()
  {
    if (Input.GetKeyDown("1")) //If the player presses space
    {
      AkSoundEngine.PostEvent("Play_Dono1", gameObject);
      if (rhythmMiss==false) //If they should mose, subtract a point. If they are in time give a point
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
    else if (Input.GetKeyDown("2")) //If the player presses space
    {
      AkSoundEngine.PostEvent("Play_Dono2", gameObject);
      if (rhythmMiss == false) //If they should mose, subtract a point. If they are in time give a point
      {
        if (lockedOut)
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

    else if (Input.GetKeyDown("3")) //If the player presses space
    {
      AkSoundEngine.PostEvent("Play_Dono3", gameObject);
      if (rhythmMiss == false) //If they should mose, subtract a point. If they are in time give a point
      {
        if (lockedOut)
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
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      currentNumber = Random.Range(1, 4);
    }
    else if (timeStart >= hitPoint2 - leaniancy && timeStart <= hitPoint2 + leaniancy)
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      currentNumber = Random.Range(1, 4);
    }
    else if (timeStart >= hitPoint3 - leaniancy && timeStart <= hitPoint3 + leaniancy)
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      currentNumber = Random.Range(1, 4);
    }
    else if (timeStart >= hitPoint4 - leaniancy && timeStart <= hitPoint4 + leaniancy)
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      currentNumber = Random.Range(1, 4);
    }
    else if (timeStart >= hitPoint5 - leaniancy && timeStart <= hitPoint5 + leaniancy)
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      currentNumber = Random.Range(1, 4);
    }
    else if (timeStart >= hitPoint6 - leaniancy && timeStart <= hitPoint6 + leaniancy)
    {
      textBox.color = Color.green;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.green;
      currentNumber = Random.Range(1, 4);
    }
    else
    {
      textBox.color = Color.red;
      PlayerVisual.GetComponent<SpriteRenderer>().color = Color.black;
    }
  }

  public void GenerateNumbers()
  {
    if (numbersGenerated == false)
    {
      randomNumbers[0] = 1;
      randomNumbers[1] = 2;
      randomNumbers[2] = 3;
      randomNumbers[3] = 1;
      randomNumbers[4] = 2;
      randomNumbers[5] = 3;
      numbersGenerated = true;
    }
  }


  public void ChangeCube()
  {
    if (currentNumber == 1) { counterCube.GetComponent<BeatCounter>().ChangeTo1(); AkSoundEngine.PostEvent("Play_Announcer1", gameObject); }

    if (currentNumber == 2) { counterCube.GetComponent<BeatCounter>().ChangeTo2(); AkSoundEngine.PostEvent("Play_Announcer2", gameObject); }

    if (currentNumber == 3) { counterCube.GetComponent<BeatCounter>().ChangeTo3(); AkSoundEngine.PostEvent("Play_Announcer3", gameObject); }
  }

  public void playAudio()
  {
    if (timeStart == hitPoint1) { AkSoundEngine.PostEvent("Play_Beep", gameObject); ChangeCube(); }
    if (timeStart >= hitPoint2 && timeStart <= hitPoint2 + .1)
    {
      if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; ChangeCube(); }
    }
    if (timeStart >= hitPoint3 && timeStart <= hitPoint3 + .1)
    {
      if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; ChangeCube(); }
    }
    if (timeStart >= hitPoint4 && timeStart <= hitPoint4 + .1)
    {
      if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; ChangeCube(); }
    }
    if (timeStart >= hitPoint5 && timeStart <= hitPoint5 + .1)
    {
      if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; ChangeCube(); }
    }
    if (timeStart >= hitPoint6 && timeStart <= hitPoint6 + .1)
    {
      if (noisePlayed == false) { AkSoundEngine.PostEvent("Play_Beep", gameObject); noisePlayed = true; loggedTime = timeStart; ChangeCube(); }
    }
  }

  }
