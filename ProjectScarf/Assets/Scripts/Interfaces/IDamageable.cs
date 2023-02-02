using UnityEngine;

public interface IDamageable
{
    public void OnHit(float damage, Vector2 knockback);
    public void OnHit(float damage);
}
