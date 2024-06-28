using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "LevelData 001", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    [Header("World Settings")]
    [SerializeField] private float _WorldSizeX = 10f;
    [SerializeField] private float _WorldSizeZ = 10f;
        

    [Header("Blob Settings")]
    [SerializeField] private int _InitialBlobCount = 10;
    [SerializeField] private int _MaxBlobCount = 50;
    [SerializeField] private float _BlobSpawnInterval = 1f;
        
    [Header("Obstacle Settings")]
    [SerializeField] private int _MaxObstacleCount = 10;
        
    [Header("Enemy Settings")]
    [SerializeField] private int _MaxEnemyCount = 5;
    
    public float WorldSizeX => _WorldSizeX;
    public float WorldSizeZ => _WorldSizeZ;
    public int InitialBlobCount => _InitialBlobCount;
    public int MaxBlobCount => _MaxBlobCount;
    public int MaxObstacleCount => _MaxObstacleCount;
    public float BlobSpawnInterval => _BlobSpawnInterval;
    public int MaxEnemyCount => _MaxEnemyCount;
}
