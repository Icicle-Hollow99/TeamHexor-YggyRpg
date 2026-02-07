using UnityEngine;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    public Camera cam;
    public Transform muzzle;
    public float range = 100f;
    public float fireRate = 0.2f;
    public float damage = 25f; // 👈 added (that’s it)

    float nextFireTime;

    void OnAttack()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireRate;
        Shoot();
    }

    void Shoot()
    {
        Debug.Log("💥 SHOOT");

        int mask = ~LayerMask.GetMask("Player");

        Ray camRay = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 aimPoint;

        if (Physics.Raycast(camRay, out RaycastHit hit, range, mask))
        {
            aimPoint = hit.point;
            Debug.Log("🎯 HIT " + hit.transform.name);

            //🔥 THIS IS THE ONLY LOGIC ADD
            EnemySimple enemy = hit.transform.GetComponent<EnemySimple>();
            if (enemy != null)
        {
             enemy.TakeDamage(damage);
            }
        }
        else
        {
            aimPoint = camRay.origin + camRay.direction * range;
        }

        Vector3 dir = (aimPoint - muzzle.position).normalized;

        Debug.DrawRay(muzzle.position, dir * range, Color.green, 1f);
        DrawTracer(muzzle.position, muzzle.position + dir * range);
    }

    void DrawTracer(Vector3 start, Vector3 end)
    {
        GameObject tracer = new GameObject("Tracer");
        LineRenderer lr = tracer.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = 0.03f;
        lr.endWidth = 0.03f;
        lr.positionCount = 2;   

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Destroy(tracer, 0.05f);
    }
}
