using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Tilemaps.TilemapRenderer;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    [SerializeField] Rigidbody2D target;
    [SerializeField] RuntimeAnimatorController[] animcon;
    

    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    SpriteRenderer spriter;
    Animator anim;
    WaitForFixedUpdate wait;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();     
        wait = new WaitForFixedUpdate();
    }

    private void FixedUpdate()
    {
        if(!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit")) { return; }
        
        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;

    }

    private void LateUpdate()
    {
        if(!isLive)
        {
            spriter.flipX = target.position.x < transform.position.x ? true : false;
        }        
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        Active(true, 2);
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animcon[data.spriteType]; // 외형 변경.
        speed = data.speed; // 속도 수정.
        maxHealth = data.health; // 체력 수정.
        health = data.health;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet") || !isLive) { return; }

        health -= collision.GetComponent<Bullet>().damage;
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");
            
        }
        else
        {
            Active(false, 1);
            GameManager.instance.Kill++;
            GameManager.instance.GetExp();
        }
    }

    IEnumerator KnockBack()
    {
        yield return wait;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3,ForceMode2D.Impulse);
    }

    void Dead()
    {        
        gameObject.SetActive(false);
    }

    void Active(bool state, int sortOrder)
    {
        isLive = state;
        coll.enabled = state;
        rigid.simulated = state;
        spriter.sortingOrder = sortOrder;
        anim.SetBool("Dead", !state);
    }
}
