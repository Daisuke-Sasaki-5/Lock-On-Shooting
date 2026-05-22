using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy‚ÌÅ‘åHP(‰¼)")]
    [SerializeField] private int maxHitCount = 3;

    private int currentHitCount;

    public void TakeHit()
    {
        currentHitCount++;

        Debug.Log("Hit Count : " + currentHitCount);

        if(currentHitCount >= maxHitCount)
        {
            Destroy(gameObject);
        }
    }
}
