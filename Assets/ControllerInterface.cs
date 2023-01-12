using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInterface : MonoBehaviour
{

    [SerializeField]
    private ExamplePlayerController _controller;

    [SerializeField]
    private GameObject light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Gets input when the space bar is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.Jump();

        }

        //turns the lights on off if the L key is pressed
        if (Input.GetKeyDown(KeyCode.L)){

            if (light.active == true)
            {

                light.SetActive(false);
            }
            else
            {
                light.SetActive(true);
            }
        }




        ///Changes the movement direction based on wasd or arrow key presses
        Vector2 direction = new Vector2();
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        _controller.MoveDirection = direction;


        /// Allows for turning an Zooming via the mouse
        _controller.Turn(Input.GetAxis("Mouse X"));
        _controller.Zoom(Input.mouseScrollDelta.y);
        



    }
}
