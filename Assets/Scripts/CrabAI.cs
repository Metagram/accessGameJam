
using UnityEngine;

public class CrabAI : MonoBehaviour
{
    private static int THRESHOLD = 15;
    private float seenPlayerTime = 0;
    protected virtual void FixedUpdate()
    {
        Enemy enemy = GetComponent<Enemy>();
        float speed = enemy.enemyInfo.speed;
        Vector3 playerPos = GameManager.instance.mainPlayer.transform.position;
        float dist = Vector3.Distance(playerPos, transform.position);
        Vector2 vel = Vector2.zero;
        if (dist <= enemy.enemyInfo.sightRange)
        {
            seenPlayerTime++;
        }
        else
        {
            seenPlayerTime = 0;
        }

        if (!GameManager.instance.isBattle)
        {
            if (dist > enemy.enemyInfo.sightRange || seenPlayerTime < THRESHOLD)
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
        
        bool moving = vel.magnitude > 0.0001f;
        animator.SetBool("moving", moving);
    }
}