using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [Header("’e‚ÌPrefab")]
    [SerializeField] private BulletVisual bulletPrefab;
    [Header("ˆê“x‚É¶¬‚³‚ê‚é’e‚Ì”")]
    [SerializeField] private int poolSize = 30;

    private List<BulletVisual> bullets = new();

    /// <summary>
    /// ‰Šú‰»
    /// </summary>
    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            BulletVisual bullet = Instantiate(bulletPrefab, transform);
            
            bullet.gameObject.SetActive(false);
            bullets.Add(bullet);
        }
    }

    public BulletVisual GetBullet()
    {
        foreach (BulletVisual bullet in bullets)
        {
            if(!bullet.gameObject.activeInHierarchy)
            {
                return bullet;
            }
        }
        return null;
    }
}
