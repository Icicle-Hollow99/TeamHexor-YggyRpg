using UnityEngine;

public class AlphaLizardHealth : MonoBehaviour
{
    public int hitsToDie = 6;
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
