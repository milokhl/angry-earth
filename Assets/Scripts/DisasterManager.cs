using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasterManager : MonoBehaviour
{
    public Disaster disaster = new Thunderstorm(); // Stores this particular disaster instance.

    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(disaster.spritePath);
    }

    public Sprite GetSprite()
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.sortingOrder = 1;
        return spr.sprite;
    }
}
