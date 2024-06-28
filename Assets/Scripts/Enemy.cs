using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3.5f;
    public float score;
    
    private float _scoreDifference = 0.2f; 
    
    
    private void Start()
    {
        score = gameObject.transform.localScale.x;
    }
}
