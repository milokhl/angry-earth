﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingManager : MonoBehaviour {
    public Building building = new Building();
    private bool isHighlighted = false;
    private Behaviour halo = null;
    
    // Start is called before the first frame update
    void Start()
    {
        // For now, the Halo component is what's used for highlighting.
        // We might want to change this later on.
        halo = (Behaviour)GetComponent("Halo");
    }

    // Update is called once per frame
    void Update()
    {
        if (halo != null) {
            halo.enabled = isHighlighted;
        }
    }

    // Hightlight or unhighlight a tile.
    public void Highlight(bool on)
    {
        isHighlighted = on;
    }

    // Access the sprite that is being rendered for this tile.
    public Sprite GetSprite()
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        spr.sortingOrder = -1;
        return spr.sprite;
    }
    
    public bool Attack(Disaster disaster)
    {
        building.health -= disaster.damage;
        return (building.health <= 0);
    }

    public void SetBuilding(Building instance)
    {
        building = instance;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(instance.spritePath);
    }

    // Resets a building to a tree (empty tile).
    // Pass in the disaster type in case we want to support different animations later.
    public void Destroy(Disaster disaster)
    {
        SetBuilding(new Building());
    }
}
