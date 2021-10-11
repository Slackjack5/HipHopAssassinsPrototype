using System;
public class HealthSystem 
{

  private int health;
  private int healthMax;
  public event EventHandler onHealthChanged;
  public event EventHandler OnDead;
  public HealthSystem(int healthMax)
  {
    this.healthMax = healthMax;
    health = healthMax;
  }

  public int GetHealth()
  {
    return health;
  }

  public float GetHealthPercent()
  {
    return (float)health / healthMax;
  }
  public void Damage(int damageAmount)
  {
    health -= damageAmount;
    if (health < 0) health = 0;
    if (onHealthChanged != null) onHealthChanged(this, EventArgs.Empty);

  }

  public void Heal(int healAmount)
  {
    health += healAmount;
    if (health > healthMax) health = healthMax;
    if (onHealthChanged != null) onHealthChanged(this, EventArgs.Empty);
  }

  public void Die()
  {
    if (OnDead != null) OnDead(this, EventArgs.Empty);
  }

  public bool IsDead()
  {
    return health <= 0;
  }
}
