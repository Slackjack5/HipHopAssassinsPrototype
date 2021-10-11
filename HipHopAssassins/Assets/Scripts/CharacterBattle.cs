using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

  private enum State
  {
    Idle,
    Sliding,
    Busy,
    Rhythm,
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
      gameObject.GetComponent<SpriteRenderer>().color = Color.cyan;
    }
    else
    {
      //Change How They Look
      gameObject.GetComponent<SpriteRenderer>().color = Color.red;
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
        //Track Players Rhythm
        if (Input.GetKeyDown(KeyCode.Space)) //On Space Inpute
        {
          if (attacksRemaining>0)
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
            });
          }

        }
          break;

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
      attacksRemaining = 3; //Set Maximum amount of attacks
      currentAction = onAttackComplete;
    });
  }

  public void Attack(CharacterBattle targetCharacterBattle, Action onAttackComplete)
  {

    Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition().normalized * 10f); //Place the character offset the target
    //Vector3 startingPosition = GetPosition();
    //Slide to Target
    SlideToPosition(targetCharacterBattle.GetPosition(), () =>
    {
      Debug.Log(targetCharacterBattle);
      //Arrived At Target Attack
      state = State.Busy;
      Vector3 attackdir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
      //Play Animation using said direction
      //once attack animation finishes
      //targetCharacterBattle.Damage(10); //Target Hit

      SlideToPosition(startingPosition, () =>
      {
        //Slide Back Completed, Back to Idle
        state = State.Idle;
        onAttackComplete();
      });
      
    });
  }

  public void PerformAttack(CharacterBattle targetCharacterBattle)
  {
      //Vector3 attackdir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
      //Play Animation using said direction
      //once attack animation finishes
      targetCharacterBattle.Damage(10); //Target Hit
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
