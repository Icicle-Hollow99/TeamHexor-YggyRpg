using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        Debug.Log("PLAYER HIT 💀 HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("PLAYER DIED — RESET SCENE");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}