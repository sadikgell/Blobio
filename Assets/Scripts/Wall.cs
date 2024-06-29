using System.Security.Cryptography;
using UnityEngine;


public class Wall : MonoBehaviour
{
    private EnemyFactory _enemyFactory;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInChildren<Player>().GameOver();
        }
        else if (other.CompareTag("Enemy"))
        {
            _enemyFactory.RemoveEnemy(other.GetComponentInChildren<Enemy>());
            Destroy(other.gameObject);
        }
        
    }
}
