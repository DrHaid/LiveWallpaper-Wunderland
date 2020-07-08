using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	public bool DebugDrawAttacks;
	public Attack PrimaryAttack;
	public Attack SpecialAttack;

	private void Update()
	{
		PrimaryAttack.UpdateDirection();
		SpecialAttack.UpdateDirection();

		if (!DebugDrawAttacks)
		{
			return;
		}
		Debug.DrawLine(PrimaryAttack.origin.position,
			PrimaryAttack.origin.position + PrimaryAttack.direction * PrimaryAttack.range, 
			Color.green);
		Debug.DrawLine(SpecialAttack.origin.position,
			SpecialAttack.origin.position + SpecialAttack.direction * SpecialAttack.range,
			Color.red);
	}

	private void OnMainAttack()
	{
    if (PlayerAnimation.current.Attack())
    {
			StartCoroutine(PerformAttack(PrimaryAttack));
    }
	}

	private void OnSpecialAttack()
	{
    if (PlayerAnimation.current.SpecialAttack())
    {
			StartCoroutine(PerformAttack(SpecialAttack));
    }
	}

	IEnumerator PerformAttack(Attack attack)
	{
		yield return new WaitForSeconds(attack.delay);
		RaycastHit2D[] hits = Physics2D.CircleCastAll(attack.origin.position, attack.radius, attack.direction, attack.range);
		foreach(var hit in hits)
		{
			if(hit.collider.gameObject.tag != "Enemy")
			{
				continue;
			}
			if (hit.transform.TryGetComponent<EnemyAI>(out EnemyAI enemyAI))
			{
				enemyAI.PerformKnockOut(attack);
			}
		}
	}
}

[Serializable]
public class Attack
{
	public float strength;
	public float range;
	public float radius;
	public float delay;
	[HideInInspector] public Vector3 direction;
	public Transform origin;

	public void UpdateDirection()
	{
		direction = (MouseLook.isFacingRight ? origin.right : -origin.right);
	}
}
