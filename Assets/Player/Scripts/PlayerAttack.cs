using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	[SerializeField] bool debugDrawAttacks;
	[SerializeField] private Attack primaryAttack;
	[SerializeField] private Attack specialAttack;

	private void Update()
	{
		primaryAttack.UpdateDirection();
		specialAttack.UpdateDirection();

		if (!debugDrawAttacks)
		{
			return;
		}
		Debug.DrawLine(primaryAttack.origin.position,
			primaryAttack.origin.position + primaryAttack.direction * primaryAttack.range, 
			Color.green);
		Debug.DrawLine(specialAttack.origin.position,
			specialAttack.origin.position + specialAttack.direction * specialAttack.range,
			Color.red);
	}

	private void OnMainAttack()
	{
    if (PlayerAnimation.current.Attack())
    {
			StartCoroutine(PerformAttack(primaryAttack));
    }
	}

	private void OnSpecialAttack()
	{
    if (PlayerAnimation.current.SpecialAttack())
    {
			StartCoroutine(PerformAttack(specialAttack));
    }
	}

	IEnumerator PerformAttack(Attack attack)
	{
		yield return new WaitForSeconds(attack.delay);
		RaycastHit2D[] hits = Physics2D.CircleCastAll(attack.origin.position, attack.radius, attack.direction, attack.range);
		foreach(var hit in hits)
		{
			if(hit.collider.gameObject == gameObject)
			{
				continue;
			}
			HandleHit(hit, attack);
		}
	}

	void HandleHit(RaycastHit2D hit, Attack attack)
	{
		if (hit.transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2D))
		{
			rb2D.AddForce(attack.direction * attack.strength, ForceMode2D.Impulse);
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
