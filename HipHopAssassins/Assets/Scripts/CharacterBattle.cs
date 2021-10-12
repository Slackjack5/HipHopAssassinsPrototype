using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EZCameraShake;
public class CharacterBattle : MonoBehaviour
{

  private Character_Base characterBase;
  private State state;
  private Vector3 slideTargetPosition;
  private Action onSlideComplete;
  private GameObject selectionCircleGameObject;
  private HealthSystem healthSystem;
  public GameObject healthBarObject;
  private GameObject thisHealthBar;
  private CharacterBattle currentTarget;
  private Action currentAction;
  private int attacksRemaining;
  private Vector3 startingPosition;
  private bool secondBeatPlayed = false;
  private int attackPattern = 1;
  private int currentBeat;
  private int savedBeat;


  [SerializeField] private GameObject enemyHitParticle;
  [SerializeField] private GameObject playerHitParticle;
  [SerializeField] private Sprite[] spriteArray;
  private enum State
  {
    Idle,
    Sliding,
    Busy,
    Rhythm,
    Attacking,
  }


  private void Awake()
  {
    characterBase = GetComponent<Character_Base>();
    state = State.Idle;
    selectionCircleGameObject = transform.Find("SelectionCircle").gameObject;
    HideSelectionCircle();
  }
  public void Setup(bool isPlayerTeam)
  {
    if (isPlayerTeam)
    {
      //Change How they Look
      gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
    }
    else
    {
      //Change How They Look
      gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[1];
    }

    healthSystem = new HealthSystem(100);

    //Spawn Health bar above units
    Vector3 position = new Vector3(GetPosition().x,GetPosition().y+1,GetPosition().z);
    thisHealthBar = Instantiate(healthBarObject, position, Quaternion.identity);
    thisHealthBar.transform.parent = gameObject.transform;
    healthSystem.onHealthChanged += HealthSystem_onHealthChanged;
    startingPosition = GetPosition();
  }

  private void HealthSystem_onHealthChanged(object sender, EventArgs e)
  {
    thisHealthBar.transform.localScale = new Vector3(healthSystem.GetHealthPercent(), 1);
  }

  private void Update()
  {
    switch(state)
    {
      case State.Idle:
        break;
      case State.Busy:
        if (savedBeat==currentBeat) 
        {
          //Play Animation using said direction
          //once attack animation finishes
          currentTarget.Damage(10); //Target Hit
          CameraShaker.Instance.ShakeOnce(10f, 6f, .1f, .1f);
          Instantiate(playerHitParticle, currentTarget.GetPosition(), Quaternion.identity);
          AkSoundEngine.PostEvent("Play_Hit", gameObject);
          SlideToPosition(startingPosition, () =>
          {
            //Slide Back Completed, Back to Idle
            state = State.Idle;
            currentAction();
          });
        }
        currentBeat = GlobalVariables.currentBeat;
        break;
      case State.Sliding:
        float slideSpeed = 10f; //Set Slide Speed
        transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime; //Slide Character to whom they are attacking

        float reachedDistance = 2f;
        if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
        {
          //Arrived at Slide Target Position
          //transform.position = slideTargetPosition;
          onSlideComplete();
        }
        break;
      case State.Rhythm:
        if(GlobalVariables.currentBeat == 2 && secondBeatPlayed==false) 
        { 
          AkSoundEngine.PostEvent("Play_SwordSheath", gameObject);
          secondBeatPlayed = true;
          state = State.Attacking;
        }
     
          break;
      case State.Attacking:

        //Track Players Rhythm
        if (Input.GetKeyDown(KeyCode.Space)) //On Space Inpute
        {
          if (attacksRemaining > 0)
          {
            //Debug.Log("Player Attacked");
            //Play Animation using said direction
            //once attack animation finishes
            PerformAttack(currentTarget);

          }
          else
          {
            SlideToPosition(startingPosition, () =>
            {
              //Slide Back Completed, Back to Idle
              state = State.Idle;
              currentAction();
              secondBeatPlayed = false;
            });
          }

        }

        //If the measure ends, kick them off combat
        if (GlobalVariables.currentBeat == 4 && GlobalVariables.currentGrid == 4)
        {
          attacksRemaining = 0;
          SlideToPosition(startingPosition, () =>
          {
            //Slide Back Completed, Back to Idle
            state = State.Idle;
            currentAction();
            secondBeatPlayed = false;
          });
        }
        break;
    }

    if(healthSystem.GetHealth()==0)
    {
      Destroy(gameObject);
    }
  }
  public Vector3 GetPosition()
  {
    return transform.position;
  }

  public void Damage(int damageAmount)
  {
    healthSystem.Damage(damageAmount);
    Debug.Log("Hit" + healthSystem.GetHealth());
  }

  public bool IsDead()
  {
    return healthSystem.IsDead();
  }
  public void AttackRhythm( CharacterBattle targetCharacterBattle, Action onAttackComplete)
  {

    Vector3 slideTargetPosition = targetCharacterBattle.GetPosition ()+ (GetPosition() - targetCharacterBattle.GetPosition().normalized * 10f); //Place the character offset the target
    Vector3 startingPosition = GetPosition();
    //Slide to Target
    SlideToPosition(targetCharacterBattle.GetPosition(), () =>
    {
      Debug.Log(targetCharacterBattle);
      //Arrived At Target Attack
      state = State.Rhythm;
      Vector3 attackdir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
      currentTarget = targetCharacterBattle;
      attacksRemaining = 2; //Set Maximum amount of attacks
      currentAction = onAttackComplete;
    });
  }

  public void Attack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
  {

    Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition().normalized * 10f); //Place the character offset the target
    savedBeat = GlobalVariables.currentBeat;
    //Vector3 startingPosition = GetPosition();
    //Slide to Target
    SlideToPosition(targetCharacterBattle.GetPosition(), () =>
    {
      Debug.Log(targetCharacterBattle);
      currentBeat = 10;
      //Arrived At Target Attack
      state = State.Busy;
      Vector3 attackdir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;

      currentTarget = targetCharacterBattle;
      currentAction = onAttackComplete;
    });
  }
  
  public void PerformAttack(CharacterBattle targetCharacterBattle)
  {
      //Vector3 attackdir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
      //Play Animation using said direction
      //once attack animation finishes
      //Attack Pattern 1
      if (attackPattern==1)
      {
      
        if(attacksRemaining==2)
        {
          if(GlobalVariables.currentBeat == 3 && (GlobalVariables.currentGrid == 1 || GlobalVariables.currentGrid == 2))
          {
          targetCharacterBattle.Damage(20); //Target Hit
          Instantiate(enemyHitParticle, targetCharacterBattle.GetPosition(), Quaternion.identity);
          AkSoundEngine.PostEvent("Play_SwordHit", gameObject);
          CameraShaker.Instance.ShakeOnce(10f, 6f, .1f, .1f);

        }
        else
        {
          AkSoundEngine.PostEvent("Play_SwordSwing", gameObject);
          
        }
        
      }
      else if(attacksRemaining == 1)
      {
        if (GlobalVariables.currentBeat == 4 && (GlobalVariables.currentGrid == 1 || GlobalVariables.currentGrid == 2))
        {
          targetCharacterBattle.Damage(20); //Target Hit
          Instantiate(enemyHitParticle, targetCharacterBattle.GetPosition(), Quaternion.identity);
          AkSoundEngine.PostEvent("Play_SwordHit", gameObject);
          CameraShaker.Instance.ShakeOnce(10f, 6f, .1f, .1f);
        }
        else
        {
          AkSoundEngine.PostEvent("Play_SwordSwing", gameObject);
        }
        
      }
    }
    
    attacksRemaining -= 1;
  }


  private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
  {
    this.slideTargetPosition = slideTargetPosition;
    this.onSlideComplete = onSlideComplete;
    state = State.Sliding;
  }

  public void HideSelectionCircle()
  {
    selectionCircleGameObject.SetActive(false);
  }

  public void ShowSelectionCircle()
  {
    selectionCircleGameObject.SetActive(true);
  }
}
