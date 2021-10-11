using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{

  public Transform pfHealthBar;
    // Start is called before the first frame update
    private void Start()
    {
    HealthSystem healthSystem = new HealthSystem(100);

    Transform healthBarTransform = Instantiate(pfHealthBar, new Vector3(0, 1), Quaternion.identity);
    HealthBar healthBar = healthBarTransform.GetComponent<HealthBar>();
    healthBar.Setup(healthSystem);
    Debug.Log("Health: " + healthSystem.GetHealth());

    }

}
