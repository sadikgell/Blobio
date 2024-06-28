using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private float _speedScaler;
    
    [SerializeField]
    private float _minSpeed = 2f;
    
    
    public float currentScore;
    private float lastScore;
    private float _scoreScaling=0;
    private float _scoreDifferenceEnemy = 0.1f; 

    [SerializeField]
    private BlobFactory _blobFactory;
    [SerializeField]
    private EnemyFactory _enemyFactory;
    
    private Vector3 _currentPosition;
    private Vector3 _desiredPosition;
    
    private void Start()
    {
         _currentPosition = transform.position;
        currentScore = gameObject.transform.localScale.x;
        _speedScaler = _speed;
    }


    private void Update()
    {
        if (currentScore == lastScore) return; 
        
        _speed = Mathf.Max(_minSpeed, _speedScaler / Mathf.Sqrt(currentScore));
        
    }

    private void FixedUpdate()
    {
        Vector3 direction = InputManager.GetDirection();
        _desiredPosition = _currentPosition + direction * (_speed * Time.deltaTime);
        Move(direction);
    }
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blob"))
        {
            Blob blob = other.GetComponentInChildren<Blob>();
            Collect(blob);
            currentScore += _scoreScaling;
            
        }
        else if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInChildren<Enemy>();
            EnemyCollision(enemy);
            currentScore += _scoreScaling;
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
        currentScore += _scoreScaling;
        ScaleChange(_scoreScaling);
        _blobFactory.RemoveBlob(blob);
        Destroy(blob.transform.parent.gameObject);
        
    }
    
    private void EnemyCollision(Enemy enemy)
    {
        bool ScoreCheck = currentScore > enemy.score + _scoreDifferenceEnemy;
        
        if (ScoreCheck)
        {
            _scoreScaling += enemy.score;
            currentScore += _scoreScaling;    
            ScaleChange(_scoreScaling);
            _enemyFactory.RemoveEnemy(enemy);
            Destroy(enemy.transform.parent.gameObject);
        }
        else
        {
            ScoreCheck = enemy.score > currentScore + _scoreDifferenceEnemy;
            if (!ScoreCheck) return;
            Destroy(gameObject);
        }
    }
    
    
}
