using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
  [SerializeField] private Transform pfCharacterBattle;
  private CharacterBattle playerCharacterBattle;
  private CharacterBattle enemyCharacterBattle;
  private State state;


  private enum State
  {
    WaitingForPlayer,
    Busy,
  }







  private void Start()
  {
    playerCharacterBattle = SpawnCharacter(true);
    enemyCharacterBattle = SpawnCharacter(false);
    //Set State
    state = State.WaitingForPlayer;
  }

  private void Update()
  {
    if(state==State.WaitingForPlayer) //If We are in Waiting for Player State
    {
      if (Input.GetKeyDown(KeyCode.Space)) //On Space Inpute
      {
        state = State.Busy; //Change State to Busy
        playerCharacterBattle.Attack(enemyCharacterBattle, () => //Play Attack animation
        {
          state = State.WaitingForPlayer; //Change State back to waiting for player once animation finishes
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
}
