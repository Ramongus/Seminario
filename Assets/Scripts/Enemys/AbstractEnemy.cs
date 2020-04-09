using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AbstractEnemy : MonoBehaviour
{
	public float maxHp;
	protected float hp;
	public Image healthBar;
	public List<IAttack> attacks;
	public IAttack currentAttack;

	virtual protected void Awake()
	{
		hp = maxHp;
		attacks = new List<IAttack>();
	}

	public abstract void Appear();
	public abstract void Disappear();
	public abstract IAttack ChooseOneAttack();
	public abstract void Attack();
	public abstract void Move();
	public abstract void Die();

	public float GetHP()
	{
		return hp;
	}

	public void SetHp(float health)
	{
		hp = health;
		if (hp < 0)
		{
			hp = 0;
			Die();
			return;
		}
		if(hp > maxHp)
		{
			hp = maxHp;
			return;
		}
		
	}

	protected void UpdateHealthBar()
	{
		healthBar.fillAmount = hp / maxHp;
	}


}
