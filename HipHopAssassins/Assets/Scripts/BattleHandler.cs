using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
  [SerializeField] private Transform pfCharacterBattle;
  private CharacterBattle playerCharacterBattle;
  private CharacterBattle enemyCharacterBattle;
  private State state;
  private CharacterBattle activeCharacterBattle;
  public Texture2D enemyTexture;
  public Texture2D PlayerTexture;



  private enum State
  {
    WaitingForPlayer,
    Busy,
  }







  private void Start()
  {
    playerCharacterBattle = SpawnCharacter(true);
    enemyCharacterBattle = SpawnCharacter(false);

    SetActiveCharacterBattle(playerCharacterBattle);
    //Set State
    state = State.WaitingForPlayer;
  }

  private void Update()
  {
    if(state==State.WaitingForPlayer) //If We are in Waiting for Player State
    {
      if (Input.GetKeyDown(KeyCode.Space) && GlobalVariables.currentBeat==1) //On Space Input and is on First Beat
      {
        state = State.Busy; //Change State to Busy
        AkSoundEngine.PostEvent("Play_Grunt", gameObject);
        playerCharacterBattle.AttackRhythm(enemyCharacterBattle, () => //Play Attack animation
        {
          ChooseNextActivePlayer(); //Change State back to waiting for player once animation finishes
        });
      }
    }
  }
  private CharacterBattle SpawnCharacter(bool isPlayerTeam)
  {
    Vector3 position;
    if(isPlayerTeam)
    {
      position = new Vector3(-5, 0);
    }
    else
    {
      position = new Vector3(+5, 0);
    }
    Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
    CharacterBattle characterbattle = characterTransform.GetComponent<CharacterBattle>();
    characterbattle.Setup(isPlayerTeam);

    return characterbattle;
  }

  private void SetActiveCharacterBattle(CharacterBattle characterbattle)
  {
    if (activeCharacterBattle != null)
    {
      activeCharacterBattle.HideSelectionCircle();
    }

    activeCharacterBattle = characterbattle;
    activeCharacterBattle.ShowSelectionCircle();

  }

  private void ChooseNextActivePlayer()
  {
    if(TestBattleOver())
    {
      return;
    }

    if (activeCharacterBattle==playerCharacterBattle)
    {
      SetActiveCharacterBattle(enemyCharacterBattle);
      state = State.Busy;

      enemyCharacterBattle.Attack(playerCharacterBattle, () => //Play Attack animation
      {
        ChooseNextActivePlayer(); //Change State back to waiting for player once animation finishes
      });
    }
    else
    {
      SetActiveCharacterBattle(playerCharacterBattle);
      state = State.WaitingForPlayer;
    }
  }

  private bool TestBattleOver()
  {
    if (playerCharacterBattle.IsDead())
    {
      //Player Dead, Enemy Wins
      Debug.Log("Enemy Wins");
      return true;
    }
    if (enemyCharacterBattle.IsDead())
    {
      //Enemy Dead, Player Wins
      Debug.Log("Player Wins");

      enemyCharacterBattle = SpawnCharacter(false);

      SetActiveCharacterBattle(playerCharacterBattle);
      //Set State
      state = State.WaitingForPlayer;

      return true;
    }
    return false;
  }

}
