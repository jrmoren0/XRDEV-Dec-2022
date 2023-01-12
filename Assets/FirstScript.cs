using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirstScript : MonoBehaviour
{
  private string message = "Hello World!";
  
  private float currentyear = 2022;

  private float birthyear = 1990;

  int counter = 0;


    // Start is called before the first frame update
    void Start()
    {
        CalculateAge();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CalculateAge()
    {
        Debug.Log("My Age Is: " + (currentyear - birthyear));
    }




    private void OnEnable()
    {
        
        counter += 1;
        Debug.Log("Count Value Is: " + (counter));
    }

}
