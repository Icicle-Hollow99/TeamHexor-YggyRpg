using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    public float speed = 3f;
    public float health = 50f;
    public float damage = 10f;
    public float attackCooldown = 1f;

    Transform player;
    float lastAttackTime;

    Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        anim = GetComponentInChildren<Animator>(); // IMPORTANT
     EnemyManager.instance.enemyCount++;
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.RegisterEnemy();
        }
    }

    void Update()
    {
        if (!player) return;

        Vector3 dir = (player.position - transform.position).normalized;
        dir.y = 0;

        transform.position += dir * speed * Time.deltaTime;
        transform.LookAt(player);
    }

    void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        // 🔥 PLAY ATTACK ANIMATION
        if (anim)
            anim.SetTrigger("Attack");

        PlayerHealth hp = collision.gameObject.GetComponent<PlayerHealth>();
        if (hp)
        {
            hp.TakeDamage(damage);

            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb)
            {
                Vector3 knockDir = (collision.transform.position - transform.position).normalized;
                knockDir.y = 0.5f;
                rb.AddForce(knockDir * 8f, ForceMode.Impulse);
            }
        }
    }

    public void TakeDamage(float dmg)
{
    health -= dmg;
    if (health <= 0)
    {
        EnemyManager.instance.EnemyDied();
        Destroy(gameObject);
    }
}
}