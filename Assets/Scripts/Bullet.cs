using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Pool pool;

    public int damage;

    void OnTriggerEnter(Collider other)
    {
        other
            .GetComponents<IHitbox>()
            ?
            .Map(hitbox => hitbox.OnHit(this));

        // pool.Despawn(this);

        GameObject.Destroy(gameObject);
    }
}
