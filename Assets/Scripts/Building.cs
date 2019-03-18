using System.Collections;
using System.Collections.Generic;

public enum BuildingType
{
    NotSelected,
    Settlement,
    House,
    Factory,
    Skyscraper

}

// The base class for buildings. Displays a blank tile (tree) by default.
// All other buildings should derive from this class and override
// the constructor with their own sprite file and other properties.
public class Building
{
    // This is the health of one settlement, which we measure other
    // buildings and disaster damages relative to.
    public static float BASE_HEALTH_UNIT = 1.0f;
    public float health = 0;
    public float xpGain = 0.0f;

    // The rank determines what a building can upgrade to.
    // A building with a lower rank will always upgrad to a building
    // of a higher rank.
    public int rank = 0;
    public int unlockedYear = 1700;
    public string spritePath = "Sprites/Nature/Tree";
    public string debugName = "Building";
    public Building() {}
};

public class Settlement : Building
{
    public Settlement() : base() {
        spritePath = "Sprites/Human/Settlement";
        xpGain = 1.0f;
        health = 1.0f * BASE_HEALTH_UNIT;
        unlockedYear = 1800;
        rank = 1;
    }
}

public class House : Building
{
    public House() : base() {
        spritePath = "Sprites/Human/House";
        xpGain = 3.0f;
        health = 3.0f * BASE_HEALTH_UNIT;
        unlockedYear = 1850;
        rank = 2;
    }
}

public class Trash : Building
{
    public Trash() : base() {
        spritePath = "Sprites/Human/Trash";
        xpGain = 1.0f;
        health = 1.0f * BASE_HEALTH_UNIT;
        unlockedYear = 1950;
        rank = -1;
    }
}

public class Factory : Building
{
    public Factory() : base() {
        spritePath = "Sprites/Human/Factory";
        xpGain = 5.0f;
        health = 5.0f * BASE_HEALTH_UNIT;
        unlockedYear = 1900;
        rank = 3;
    }
}

public class Skyscraper : Building
{
    public Skyscraper() : base() {
        spritePath = "Sprites/Human/Skyscraper";
        xpGain = 10.0f;
        health = 10.0f * BASE_HEALTH_UNIT;
        unlockedYear = 1980;
        rank = 4;
    }
}