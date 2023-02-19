
using UnityEngine;

public class SealAI : MoveTowardsPlayerAI
{
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        bool moving = GetComponent<Rigidbody2D>().velocity.magnitude > 0.0001f;
        GetComponentInChildren<Animator>().SetBool("moving", moving);
    }
}
