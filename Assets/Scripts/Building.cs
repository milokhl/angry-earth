using System.Collections;
using System.Collections.Generic;

// The base class for buildings. Displays a blank tile (tree) by default.
// All other buildings should derive from this class and override
// the constructor with their own sprite file and other properties.
public class Building
{
    public float health = 0;
    public string spritePath = "Sprites/Nature/Tree";
    public Building() {}
};

public class Settlement : Building
{
    public Settlement() : base() {
        spritePath = "Sprites/Human/Settlement";
        health = 20.0f;
    }
}

public class House : Building
{
    public House() : base() {
        spritePath = "Sprites/Human/House";
        health = 30.0f;
    }
}

public class Factory : Building
{
    public Factory() : base() {
        spritePath = "Sprites/Human/Factory";
        health = 100.0f;
    }
}

public class Skyscraper : Building
{
    public Skyscraper() : base() {
        spritePath = "Sprites/Human/Skyscraper";
        health = 200.0f;
    }
}