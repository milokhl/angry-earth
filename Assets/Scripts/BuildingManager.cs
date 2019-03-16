﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The base class for buildings. Displays a blank tile (tree) by default.
// All other buildings should derive from this class and override
// the constructor with their own sprite file and other properties.
public class Building
{
    public float health = 0;
    public string spritePath = "Sprites/Placeholders/TransparentTree";
    public Building() {}
};

public class Settlement : Building
{
    public Settlement() : base() {
        spritePath = "Sprites/Placeholders/TransparentSettlement";
        health = 50;
    }
}

public class Factory : Building
{
    public Factory() : base() {
        spritePath = "Sprites/Placeholders/Factory";
        health = 100;
    }
}

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

        // Each type of building has a path to a sprite.
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(building.spritePath);
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
        return GetComponent<SpriteRenderer>().sprite;
    }
}
