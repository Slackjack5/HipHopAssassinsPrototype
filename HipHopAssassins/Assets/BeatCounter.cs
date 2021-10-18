using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatCounter : MonoBehaviour
{

  [SerializeField] private Sprite[] spriteArray;
  [SerializeField] private Animator myAnimator;
  private Vector3 defaultSize = new Vector3(1, 1, 1);
  public float shrinkSpeed = 0.1f;
  // Start is called before the first frame update
  void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {

    if (gameObject.transform.localScale.y > 1) 
    {

      gameObject.transform.localScale -= new Vector3(shrinkSpeed, shrinkSpeed, shrinkSpeed);

      //Debug.Log("Shrinking");
    }
    }
    

  public void ChangeTo1()
  {
    gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[0];
    gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
  }
  public void ChangeTo2()
  {
    gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[1];
    gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

  }
  public void ChangeTo3()
  {
    gameObject.GetComponent<SpriteRenderer>().sprite = spriteArray[2];
    gameObject.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
  }

}
