using Unity.Hierarchy;
using UnityEngine;

public class BulletVisual : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float lifeTime;

    /// <summary>
    /// ‰Šú‰»
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="time"></param>
    public void Init(Vector3 dir, float moveSpeed, float time)
    {
        direction = dir;
        speed = moveSpeed;
        lifeTime = time;

        gameObject.SetActive(true);
    }

    /// <summary>
    /// ’e‚Ì—LŒøŠÔ
    /// </summary>
    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0f)
        {
            gameObject.SetActive(false);
        }
    }
}
