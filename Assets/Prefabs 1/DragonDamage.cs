using UnityEngine;

public class DragonTouchDamage : MonoBehaviour
{
    public float damage = 20f;
    public float damageCooldown = 1f;

    float lastDamageTime;

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (Time.time - lastDamageTime >= damageCooldown)
        {
            PlayerHealth hp = collision.gameObject.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }
}
    