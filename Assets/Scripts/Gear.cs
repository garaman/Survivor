using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        name = "Gear " + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localScale = Vector3.zero;

        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUP(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }


    void ApplyGear()
    {
        switch(type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        float speed = 0f;
        foreach (Weapon weapon in weapons)
        {
            switch(weapon.id)
            {
                case 0:
                    speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                case 1:
                    speed = 0.7f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;

                default:                    
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = GameManager.instance.player.speed * Character.Speed;
        GameManager.instance.player.speed = speed + speed*rate;
    }
}
