using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    float timer;
    Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if(timer > speed)
                {
                    timer = 0;
                    Fire();
                }
                break;
        }
    }

    public void Init()
    { 
        switch(id)
        {
            case 0:
                speed = 150;
                WeaponCirecle();
                break;
            default:
                speed = 0.3f;
                Fire();
                break;
        }
    }

    public void LevelUP(float damage, int count)
    {
        this.damage = damage;
        this.count += count;

        if(id == 0)
        {
            WeaponCirecle();
        }
    }

    void WeaponCirecle()
    {
        for(int i = 0; i < count; i++)
        {
            Transform bulletPos = null;
            if ( i < transform.childCount )
            {
                bulletPos = transform.GetChild(i);
            }
            else
            {
                bulletPos = GameManager.instance.pool.Get(prefabId).transform;
            }            
            bulletPos.parent = transform;

            bulletPos.localPosition = Vector3.zero;
            bulletPos.localRotation = Quaternion.identity;

            Vector3 rotateVec = Vector3.forward * 360 * i / count;
            bulletPos.Rotate(rotateVec);
            bulletPos.Translate(bulletPos.up * 1.5f, Space.World);

            bulletPos.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
        }
    }

    void Fire()
    {
        if (player.scanner.nearestTarget == null) { return; }

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bulletPos = GameManager.instance.pool.Get(prefabId).transform;
        bulletPos.parent = transform;
        bulletPos.position = transform.position;
        bulletPos.rotation = Quaternion.FromToRotation(Vector3.up, dir);

        bulletPos.GetComponent<Bullet>().Init(damage, count, dir);
    }
}
