using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private Transform _moonPivot;


    [SerializeField]
    float _speed;

    [SerializeField]
    float _speedIncease; 

    [SerializeField]
    private Vector3 _direction;
  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

       // _speed = _speed + _speedIncease * Time.deltaTime;

       // Debug.Log(_speed);
        _moonPivot.Rotate(_speed *Time.deltaTime,0,0);

       
    }
}
