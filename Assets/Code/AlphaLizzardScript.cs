using UnityEngine;

public class LizardAlphaBoss : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float stopDistance = 3f;
    public int health = 8;

    private Transform player;

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p == null)
        {
            enabled = false;
            return;
        }
        player = p.transform;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.magnitude < stopDistance) return;

        dir.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 3f * Time.deltaTime);
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    public void TakeHit()
    {
        health--;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    
}
