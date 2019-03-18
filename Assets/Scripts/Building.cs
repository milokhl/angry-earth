using System.Collections;
using System.Collections.Generic;



// The base class for buildings. Displays a blank tile (tree) by default.
// All other buildings should derive from this class and override
// the constructor with their own sprite file and other properties.
public class Building
{
    // This is the health of one settlement, which we measure other
    // buildings and disaster damages relative to.
    public static float BASE_HEALTH_UNIT = 1.0f;
    public float health = 0;
    public string spritePath = "Sprites/Nature/Tree";
    public Building() {}
};

public class Settlement : Building
{
    public Settlement() : base() {
        spritePath = "Sprites/Human/Settlement";
        health = 1.0f * BASE_HEALTH_UNIT;
    }
}

public class House : Building
{
    public House() : base() {
        spritePath = "Sprites/Human/House";
        health = 3.0f * BASE_HEALTH_UNIT;
    }
}

public class Factory : Building
{
    public Factory() : base() {
        spritePath = "Sprites/Human/Factory";
        health = 5.0f * BASE_HEALTH_UNIT;
    }
}

public class Skyscraper : Building
{
    public Skyscraper() : base() {
        spritePath = "Sprites/Human/Skyscraper";
        health = 10.0f * BASE_HEALTH_UNIT;
    }
}