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

  private enum State
  {
    Idle,
    Sliding,
    Busy,
  }


  private void Awake()
  {
    characterBase = GetComponent<Character_Base>();
    state = State.Idle;
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

        float reachedDistance = 1f;
        if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
        {
          //Arrived at Slide Target Position
          transform.position = slideTargetPosition;
          onSlideComplete();
        }
        break;

    }
  }
  public Vector3 GetPosition()
  {
    return transform.position;
  }
  public void Attack( CharacterBattle targetCharacterBattle, Action onAttackComplete)
  {

    Vector3 slideTargetPosition = targetCharacterBattle.GetPosition ()+ (GetPosition() - targetCharacterBattle.GetPosition().normalized * 10f); //Place the character offset the target
    Vector3 startingPosition = GetPosition();
    //Slide to Target
    SlideToPosition(targetCharacterBattle.GetPosition(), () =>
    {
      //Arrived At Target Attack
      state = State.Busy;
      Vector3 attackdir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
      //Play Animation using said direction
      //once attack animation finishes
      SlideToPosition(startingPosition, () =>
      {
        //Slide Back Completed, Back to Idle
        state = State.Idle;
        onAttackComplete();
      });
      
    });
  }

  private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
  {
    this.slideTargetPosition = slideTargetPosition;
    this.onSlideComplete = onSlideComplete;
    state = State.Sliding;
  }
}
