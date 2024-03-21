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
        player = GameManager.instance.player;
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

    public void Init(ItemData data)
    { 
        name = "Weapon "+data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for(int i = 0; i < GameManager.instance.pool.prefabs.Length; i++)
        {
            if(data.projectile == GameManager.instance.pool.prefabs[i])
            {
                prefabId = i;
                break;
            }
        }

        switch(id)
        {
            case 0:
                speed = 150;
                WeaponCirecle();
                break;
            default:
                speed = 0.7f;
                Fire();
                break;
        }

        // Hand set
        Hand hand = player.hands[(int)data.itemType];
        hand.spriter.sprite = data.hand;
        hand.gameObject.SetActive(true);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUP(float damage, int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        if(id == 0)
        {
            WeaponCirecle();
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
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
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Melee);
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
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
