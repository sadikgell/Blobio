using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    private float _horizontalInput;
    private float _verticalInput;
    
    public static Vector3 GetDirection()
    {
        float verticalAxis = Input.GetAxis("Vertical");
        float horizontalAxis = Input.GetAxis("Horizontal");
        return new Vector3(horizontalAxis, 0, verticalAxis);
    }
    
    
}
