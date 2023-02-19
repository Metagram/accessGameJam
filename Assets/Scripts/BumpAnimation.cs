
using System.Collections;
using UnityEngine;

public class BumpAnimation
{
    public Entity entity;
    public Entity target;
    private static float SPEED = 20f;
    private Vector2 originalPosition;
    private RigidbodyConstraints2D oldContraints;
    
    public BumpAnimation(Entity entity, Entity target)
    {
        this.entity = entity;
        this.target = target;
        this.originalPosition = entity.transform.position;
    }

    public IEnumerator Start()
    {
        if (entity is Enemy && target is Enemy || entity is Player && target is Player)
        {
            yield break;
        }
        oldContraints = target.GetComponent<Rigidbody2D>().constraints;
        target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        while (true)
        {
            Vector2 direction = (target.transform.position - entity.transform.position).normalized;
            entity.GetComponent<Rigidbody2D>().velocity = direction * SPEED;
            if (Vector2.Distance(target.transform.position, entity.transform.position) < 1.5f)
            {
                break;
            }

            yield return null;
        }
    }
    
    public IEnumerator End()
    {
        if (entity is Enemy && target is Enemy || entity is Player && target is Player)
        {
            yield break;
        }
        while (true)
        {
            Vector2 direction = (originalPosition - (Vector2)entity.transform.position).normalized;
            entity.GetComponent<Rigidbody2D>().velocity = direction * SPEED;
            if (Vector2.Distance(originalPosition, entity.transform.position) < 0.5f)
            {
                entity.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                entity.transform.position = originalPosition;
                break;
            }

            yield return null;
        }
        target.GetComponent<Rigidbody2D>().constraints = oldContraints;
    }

    public IEnumerator PlayBoth()
    {
        yield return Start();
        yield return End();
    } 
}
