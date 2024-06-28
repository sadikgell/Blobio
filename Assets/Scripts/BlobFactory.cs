using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BlobFactory : MonoBehaviour
{
    [SerializeField] 
    private GameObject _blobPrefab;
    
    [SerializeField] 
    private LevelData _levelData;
    
    public List<Blob> blobs = new();
    
    private float _timeSinceLastSpawn = 0f;
    
    private int _currentBlobCount = 0;
    private float _blobSpawnInterval;
    private int _maxBlobCount;
    
    private float _worldSizeX;
    private float _worldSizeZ;
    
    private void Start()
    {
        _worldSizeX = _levelData.WorldSizeX;
        _worldSizeZ = _levelData.WorldSizeZ;
        _blobSpawnInterval = _levelData.BlobSpawnInterval;
        _maxBlobCount = _levelData.MaxBlobCount;
        
    }
    private void Update()
    {
        if (_currentBlobCount >= _maxBlobCount) return;
        
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= _blobSpawnInterval )
        {
            SpawnBlob();
            _timeSinceLastSpawn = 0f;
        }
    }
    private void SpawnBlob()
    {
        Vector3 spawnPosition = new Vector3(Random.Range(-_worldSizeX, _worldSizeX), 0, Random.Range(-_worldSizeZ, _worldSizeZ));
        GameObject blob = Instantiate(_blobPrefab, spawnPosition, Quaternion.identity);
        blobs.Add(blob.GetComponentInChildren<Blob>());
        _currentBlobCount++;
    }
    
    public void RemoveBlob(Blob blob)
    {
        blobs.Remove(blob);
        _currentBlobCount--;
    }
    
    
    
}
