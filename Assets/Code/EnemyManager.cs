using UnityEngine;
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public int enemyCount;

    void Awake()
    {
        instance = this;
    }

    public void RegisterEnemy()
    {
        enemyCount++;
    }

    public void EnemyDied()
    {
        enemyCount--;

        Debug.Log("Enemy left: " + enemyCount);

        if (enemyCount <= 0)
        {
            Debug.Log("ALL ENEMIES DEAD – ENTER THE LAKE");
        }
    }
}