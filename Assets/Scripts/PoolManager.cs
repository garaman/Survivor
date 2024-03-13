using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] GameObject[] prefabs;

    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for(int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }


    public GameObject Get(int index)
    {
        GameObject selectObj = null;

        foreach(GameObject obj in pools[index]) 
        {
            if(!obj.activeSelf)
            {
                selectObj = obj;
                selectObj.SetActive(true);
                break;
            }
        }

        if(selectObj == null) 
        {
            selectObj = Instantiate(prefabs[index], transform);
            pools[index].Add(selectObj);
        }

        return selectObj;
    }

}
