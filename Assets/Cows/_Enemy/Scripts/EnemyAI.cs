using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  public float Speed = 0.5f;

  private bool isKnockedOut;
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
    if (isKnockedOut)
    {
      return;
    }

    if(Mathf.Abs(player.transform.position.x - gameObject.transform.position.x) < 0.2f)
    {
      ProgressGame.instance.RemoveProgress();
      return;
    }

    var dir = (player.transform.position.x > gameObject.transform.position.x) ? Speed : -Speed;
    rb2D.velocity = new Vector2(dir, rb2D.velocity.y);

    var newScale = transform.localScale;
    newScale.x = (dir < 0) ? -originalScale.x : originalScale.x;
    transform.localScale = newScale;
  }

  public void PerformKnockOut(Attack attack)
  {
    if (transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2D))
    {
      isKnockedOut = true;
      rb2D.constraints = RigidbodyConstraints2D.None;
      rb2D.AddForce(attack.direction * attack.strength, ForceMode2D.Impulse);
      rb2D.AddTorque(-attack.direction.x * attack.strength, ForceMode2D.Impulse);
      Destroy(gameObject.transform.GetComponent<CapsuleCollider2D>());
      Invoke("DeleteCowEnemy", 0.5f);
    }
  }

  private void DeleteCowEnemy()
  {
    GameObject explosion = Instantiate(Resources.Load("Prefabs/Explosion", typeof(GameObject)),
       gameObject.transform.position,
       Quaternion.identity) as GameObject;
    ProgressGame.instance.AddProgress();
    EnemySpawner.instance.DeleteEnemy(gameObject, explosion.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length * 0.10f);
  }
}
