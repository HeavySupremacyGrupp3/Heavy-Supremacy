using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Furniture : Item
{
    public GameObject FurnitureToInstantiate;
    public string GameObjectToDestroy;
    public string GameObjectToReplace;
    private Vector3 pos;

    public void UpdateFurnitures()
    {
        FindPosition();
        AddGameObject(FurnitureToInstantiate);
        RemoveGameObject(GameObjectToDestroy);
        ReplaceSprite(GameObjectToReplace);
    }

    void FindPosition()
    {
        pos = Camera.main.ScreenToWorldPoint(transform.position);
    }

    void AddGameObject(GameObject go)
    {
        if (go != null)
        {
            GameObject goTemp = Instantiate(go, pos, Quaternion.Euler(pos), GameObject.Find("buttonCanvas").transform) as GameObject;
            goTemp.name = go.name;
        }
    }

    void RemoveGameObject(string name)
    {
        if (name != null)
            Destroy(GameObject.Find(name));
    }

    void ReplaceSprite(string name)
    {
        if (name != null)
        {
            GameObject.Find(name).GetComponentInChildren<Image>().sprite = GetComponent<Image>().sprite;
            GameObject.Find(name).transform.position = pos;
            GameObject.Find(name).transform.rotation = Quaternion.Euler(pos);
        }
    }
}
