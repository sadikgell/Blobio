using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public float speed = 3.5f;
    
    
    [SerializeField]
    private float _score;
    private float _scoreScaling=0;
    private float _scoreDifferenceEnemy = 0.1f; 

    [SerializeField]
    private BlobFactory _blobFactory;
    
    private void Start()
    {
        _score = gameObject.transform.localScale.x;
    }

    private void Update()
    {
        Vector3 direction = InputManager.GetDirection();
        Move(direction);
    }
    


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blob"))
        {
            Blob blob = other.GetComponentInChildren<Blob>();
            Collect(blob);
            _score += _scoreScaling;
            
        }
        else if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponentInChildren<Enemy>();
            EnemyCollision(enemy);
            _score += _scoreScaling;
        }
        
    }
    
    
    private void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        
        if (direction.sqrMagnitude > 1)
            direction.Normalize();

        transform.position += direction * (speed * Time.deltaTime);
    }

    private void ScaleChange(float scaleChanger)
    {
        transform.localScale += new Vector3(scaleChanger, 0, scaleChanger);
        _scoreScaling = 0;
    }
    
    private void Collect(Blob blob)
    {
        _scoreScaling += blob.score;
        _score += _scoreScaling;
        ScaleChange(_scoreScaling);
        _blobFactory.RemoveBlob(blob);
        Destroy(blob.transform.parent.gameObject);
        
    }
    
    private void EnemyCollision(Enemy enemy)
    {
        bool ScoreCheck = _score > enemy.score + _scoreDifferenceEnemy;
        
        if (ScoreCheck)
        {
            _scoreScaling += enemy.score;
            _score += _scoreScaling;    
            ScaleChange(_scoreScaling);
            Destroy(enemy.transform.parent.gameObject);
        }
        else
        {
            ScoreCheck = enemy.score > _score + _scoreDifferenceEnemy;
            if (!ScoreCheck) return;
            Destroy(gameObject);
        }
    }
    
    
}
