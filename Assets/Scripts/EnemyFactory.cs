using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField] 
    private GameObject _enemyPrefab;
    
    [SerializeField] 
    private LevelData _levelData;
    
    public List<Enemy> enemies = new();
    
    private float _timeSinceLastSpawn = 0f;
    
    private int _currentEnemyCount = 0;
    private float _enemySpawnInterval;
    private int _maxEnemyCount;
    
    private float _worldSizeX;
    private float _worldSizeZ;
    
    private void Start()
    {
        _worldSizeX = _levelData.WorldSizeX;
        _worldSizeZ = _levelData.WorldSizeZ;
        _maxEnemyCount = _levelData.MaxEnemyCount;
    }
    
    private void Update()
    {
        if (_currentEnemyCount >= _maxEnemyCount) return;
        
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= _enemySpawnInterval )
        {
            SpawnEnemy();
            _timeSinceLastSpawn = 0f;
        }
    }
    
    private void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-_worldSizeX, _worldSizeX), 0, Random.Range(-_worldSizeZ, _worldSizeZ));
        GameObject enemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
        enemies.Add(enemy.GetComponentInChildren<Enemy>());
        _currentEnemyCount++;
    }
    
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);
        _currentEnemyCount--;
    }
}
