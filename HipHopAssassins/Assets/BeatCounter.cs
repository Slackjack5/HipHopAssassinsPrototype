using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCounter : MonoBehaviour
{

  [SerializeField] private Sprite[] spriteArray;

  // Start is called before the first frame update
  void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if(GlobalVariables.currentBeat==1)
    {
    gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[0];
    }
    else if (GlobalVariables.currentBeat == 2)
    {
      gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[1];
    }
    else if (GlobalVariables.currentBeat == 3)
    {
      gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[2];
    }
    else if (GlobalVariables.currentBeat == 4)
    {
      gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[3];
    }
  }
}
