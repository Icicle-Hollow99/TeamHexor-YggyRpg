using UnityEngine;

public class LizardHealth : MonoBehaviour
{
    public int hitsToDie = 2;
    private int hits;

    void OnMouseDown()
    {
        hits++;

        if (hits >= hitsToDie)
        {
            Destroy(gameObject);
        }
    }
}

