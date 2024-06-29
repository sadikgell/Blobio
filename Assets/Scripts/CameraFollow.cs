using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Camera _camera;
    private Vector3 _offset;
    
    [SerializeField]
    private Player _player;
    private Transform _playerTransform;

    private float _cameraHeight = 3;

    private float _playerCurrentScore;
    private float _playerPreviousScore;
    
    
    private void Start()
    {
        _camera = Camera.main;
        _playerTransform = _player.transform;
        _offset = new Vector3(0,
                            _playerTransform.position.y + _cameraHeight ,
                              0);
        _playerCurrentScore = _player.currentScore;
        _playerPreviousScore = _playerCurrentScore;
    }
    
    private void FixedUpdate()
    {
        _playerCurrentScore = _player.currentScore;
        
        Vector3 desiredPosition = _playerTransform.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(_camera.transform.position, desiredPosition, 0.125f);
        _camera.transform.position = smoothedPosition;
         
        if (_playerCurrentScore ==_playerPreviousScore) return;
        
        CameraPositionUpdate();
    }
    
    private void CameraPositionUpdate()
    {
        _cameraHeight = _player.currentScore * 3f;
        _playerPreviousScore = _playerCurrentScore;
        _offset.y = _playerTransform.position.y + _cameraHeight;
    }
    
}
