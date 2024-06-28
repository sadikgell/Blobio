using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public float speed = 10f;
    
    public float score;
    private float _scoreScaling=0;
    private float _scoreDifferenceEnemy = 0.1f; 

    [SerializeField]
    private BlobFactory _blobFactory;
    
    private Vector3 _currentPosition;
    private Vector3 _desiredPosition;
    
    private void Start()
    {
         _currentPosition = transform.position;
        score = gameObject.transform.localScale.x;
    }
    
    

    private void FixedUpdate()
    {
        
        Vector3 direction = InputManager.GetDirection();
        _desiredPosition = _currentPosition + direction * (speed * Time.deltaTime);
        Move(direction);
    }
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blob"))
        {
            Blob blob = other.GetComponentInChildren<Blob>();
            Collect(blob);
            score += _scoreScaling;
            
        }
        else if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInChildren<Enemy>();
            EnemyCollision(enemy);
            score += _scoreScaling;
        }
        
    }
    
    
    private void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        if (direction.sqrMagnitude > 1)
            direction.Normalize();

        _currentPosition = Vector3.Lerp(_currentPosition, _desiredPosition, 0.2f);
        transform.position = _currentPosition;
    }

    private void ScaleChange(float scaleChanger)
    {
        transform.localScale += new Vector3(scaleChanger, 0, scaleChanger);
        _scoreScaling = 0;
    }
    
    private void Collect(Blob blob)
    {
        _scoreScaling += blob.score;
        score += _scoreScaling;
        ScaleChange(_scoreScaling);
        _blobFactory.RemoveBlob(blob);
        Destroy(blob.transform.parent.gameObject);
        
    }
    
    private void EnemyCollision(Enemy enemy)
    {
        bool ScoreCheck = score > enemy.score + _scoreDifferenceEnemy;
        
        if (ScoreCheck)
        {
            _scoreScaling += enemy.score;
            score += _scoreScaling;    
            ScaleChange(_scoreScaling);
            Destroy(enemy.transform.parent.gameObject);
        }
        else
        {
            ScoreCheck = enemy.score > score + _scoreDifferenceEnemy;
            if (!ScoreCheck) return;
            Destroy(gameObject);
        }
    }
    
    
}
