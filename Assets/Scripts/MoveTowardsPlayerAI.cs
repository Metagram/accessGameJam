using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayerAI : MonoBehaviour
{
    protected virtual void FixedUpdate()
    {
        Enemy enemy = GetComponent<Enemy>();
        float speed = enemy.enemyInfo.speed;
        Vector3 playerPos = GameManager.instance.mainPlayer.transform.position;
        float dist = Vector3.Distance(playerPos, transform.position);
        Vector2 vel = Vector2.zero;
        if (!GameManager.instance.isBattle)
        {
            if (dist > enemy.enemyInfo.sightRange)
            {
                vel = Vector2.zero;
            }
            else
            {
                vel = (playerPos - transform.position).normalized * speed;
            }
            GetComponent<Rigidbody2D>().velocity = vel;
        }

        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            Transform t = animator.transform;
            float sx = t.localScale.x;
            if (vel.x > 0 || GameManager.instance.isBattle)
            {
                sx = Mathf.Abs(sx);
            }
            else
            {
                sx = -Mathf.Abs(sx);
            }
            t.localScale = new Vector3(sx, t.localScale.y, t.localScale.z);
        }
    }
}
