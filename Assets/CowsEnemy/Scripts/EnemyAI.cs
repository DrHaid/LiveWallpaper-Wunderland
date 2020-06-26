using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  public float Speed = 0.5f;
  
  private Transform player;
  private Rigidbody2D rb2D;
  private Vector3 originalScale;

  void Start()
  {
    player = GameObject.FindGameObjectWithTag("Player").transform;
    rb2D = gameObject.transform.GetComponent<Rigidbody2D>();
    originalScale = transform.localScale;
  }

  void Update()
  {
    if(Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < 0.2f)
    {
      return;
    }

    var dir = (player.transform.position.x > gameObject.transform.position.x) ? Speed : -Speed;
    rb2D.velocity = new Vector2(dir, rb2D.velocity.y);

    var newScale = transform.localScale;
    newScale.x = (dir < 0) ? -originalScale.x : originalScale.x;
    transform.localScale = newScale;
  }
}
