using UnityEngine;

public interface IDamagable
{
    public int Health { get; set; }
    public LayerMask GetHitBy  { get; set; }

    public virtual void Damage(int damage, LayerMask attacker)
    {
        if ((GetHitBy.value & attacker.value) != 0) { return; }
        Health -= damage;

        if(Health <= 0)
        {
            Die();
        }
    }

    public abstract void Die();
}
