/*
@Author - Craig
@Description - 
*/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

//inherit MonoBehaviour to use Unity's built-in functions, then implement Entity
public class Player : Entity
{
    public static bool frozen = false;
    public CharacterInfo characterInfo;
    public bool mainCharacter = false;
    
    public override EntityInfo GetEntityInfo()
    {
        return characterInfo;
    }
    // Start is called before the first frame update
    

    protected override void Start()
    {
        base.Start();
        mana = characterInfo.mana;
    }

    void Update()
    {
        if (!GameManager.instance.isBattle && !mainCharacter)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 direction = new Vector2(0, 0);

        if (!GameManager.instance.isBattle && !frozen)
        {
            direction = new Vector2(GameManager.instance.Horizontal() * characterInfo.speed, GameManager.instance.Vertical() * characterInfo.speed);
            GetComponent<Rigidbody2D>().velocity = direction;
        }

        bool moving = GetComponent<Rigidbody2D>().velocity.magnitude > 0.0001f;
        Animator animator = GetComponentInChildren<Animator>();
        if (animator != null)
        {
            animator.SetBool("moving", moving);
        }

        if ((moving && Mathf.Abs(direction.x) >= 0.0001f) || GameManager.instance.isBattle)
        {
            bool right = direction.x > 0 && !GameManager.instance.isBattle;
            float sx = Mathf.Abs(animator.transform.localScale.x);
            if (!right)
            {
                sx = -sx;
            }

            animator.transform.localScale =
                new Vector3(sx, animator.transform.localScale.y, animator.transform.localScale.z);
        }

    }

    //either for line of sight of enemy, or if player gets too close
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<Enemy>() != null
        && !GameManager.instance.isBattle
        && GameManager.instance.currentRoom.roomType != "Hallway")
        {
            GameManager.instance.StartBattle(collider.gameObject.GetComponent<Enemy>());
        }
    }
}