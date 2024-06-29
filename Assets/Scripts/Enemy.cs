using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float speed = 3.5f;
    private float _minSpeed = 2f;
    private float _speedScaler;

    public float currentScore;
    private float _scoreScaling = 0; 
    private float _scoreDifference = 0.2f;
    
    private Vector3 _currentPosition;
    private Vector3 _desiredPosition;
    
    
    [SerializeField]
    private EnemyFactory _enemyFactory;
    private Enemy _targetEnemy;

    
    private Player _targetPlayer;

    [SerializeField]
    private BlobFactory _blobFactory;

    private State _state;

    
    private float _chaseDistance = 5f;
    
    private Vector3 _wanderingDirection;
    private float _wanderingTimer = 0f;
    private float _timeToChangeDirection = 2f;


    private enum State
    {
        Wandering,
        Chasing
    }

    private void Start()
    {
        _currentPosition = transform.position;
        
        _speedScaler = speed;
        
        _enemyFactory = FindObjectOfType<EnemyFactory>();
        _blobFactory = FindObjectOfType<BlobFactory>();
        
        currentScore = gameObject.transform.localScale.x;
        _state = State.Wandering;
    }

    private void Update()
    {
        speed = Mathf.Max(_minSpeed, _speedScaler / Mathf.Sqrt(currentScore));
        
        if (!CheckForEntitiesNearby(_chaseDistance))
        {
            _targetPlayer = null;
            _targetEnemy = null;
            _state = State.Wandering;
            return; 
        }

        CheckForEntitesScore();
        
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.Wandering:
                Wandering();
                break;
            case State.Chasing:
                Chasing();
                break;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy otherEnemy = other.GetComponentInChildren<Enemy>();
            if (currentScore > otherEnemy.currentScore + _scoreDifference) EnemyCollision(otherEnemy);
            
        }
        else if (other.CompareTag("Blob"))
        {
            Blob blob = other.GetComponentInChildren<Blob>();
            Collect(blob);
        }
    }


    private void Wandering()
    {
        _wanderingTimer += Time.deltaTime;
        if (_wanderingTimer >= _timeToChangeDirection)
        {
            _wanderingDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _wanderingTimer = 0f;
        }

        MoveEnemy(_wanderingDirection);
    }

    private void Chasing()
    {
        if (_targetEnemy)
        {
            Vector3 directionToEnemy = (_targetEnemy.transform.position - transform.position).normalized;
            MoveEnemy(directionToEnemy);
        }
        else if (_targetPlayer)
        {
            Vector3 directionToPlayer = (_targetPlayer.transform.position - transform.position).normalized;
            MoveEnemy(directionToPlayer);
        }

    }
    
    private void CheckForEntitesScore()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _chaseDistance);

        foreach (var objCollider in colliders)
        {
            if (objCollider.GetComponent<Enemy>()) 
            {
                Enemy enemy = objCollider.GetComponentInChildren<Enemy>();
                if (currentScore > enemy.currentScore + _scoreDifference)
                {
                    _state = State.Chasing;
                    _targetEnemy = enemy;
                    _targetPlayer = null;
                }
            }
            else if (objCollider.GetComponent<Player>())
            {
                Player player = objCollider.GetComponentInChildren<Player>();
                if (currentScore > player.currentScore + _scoreDifference)
                {
                    _state = State.Chasing;
                    _targetPlayer = player;
                    _targetEnemy = null;
                }
            }
        }
    }
    
    private bool CheckForEntitiesNearby(float radius)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var objCollider in colliders)
        {
            if (objCollider.gameObject.GetInstanceID() == gameObject.GetInstanceID())
            {
                continue;
            }
            if (objCollider.GetComponent<Enemy>() || objCollider.GetComponent<Player>())
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _chaseDistance);
    }


    private void EnemyCollision(Enemy otherEnemy)
    {
        bool ScoreCheck = currentScore > otherEnemy.currentScore + _scoreDifference;
        
        if (ScoreCheck)
        {
            _scoreScaling += otherEnemy.currentScore;
            currentScore += _scoreScaling;    
            ScaleChange(_scoreScaling);
            _enemyFactory.RemoveEnemy(otherEnemy);
            Destroy(otherEnemy.transform.parent.gameObject);
        }
    }
    
    public void MoveEnemy(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        if (direction.sqrMagnitude > 1)
            direction.Normalize();

        _desiredPosition = _currentPosition + direction * (speed * Time.deltaTime);
        _currentPosition = Vector3.Lerp(_currentPosition, _desiredPosition, 0.2f);
        transform.position = _currentPosition;
    }
    
    private void Collect(Blob blob)
    {
        _scoreScaling += blob.score;
        currentScore += _scoreScaling;
        ScaleChange(_scoreScaling);
        _blobFactory.RemoveBlob(blob);
        Destroy(blob.transform.parent.gameObject);
    }

    private void ScaleChange(float scaleChanger)
    {
        transform.localScale += new Vector3(scaleChanger, 0, scaleChanger);
        _scoreScaling = 0;
    }
}

