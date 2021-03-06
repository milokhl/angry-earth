using System.Collections;
using System.Collections.Generic;

public enum DisasterType
{
    NotSelected,
    Thunderstorm,
    Fire,
    Tornado,
    Tsunami,
    Meteor
};

// Base class for all disasters.
public class Disaster
{
    public float damage = 1.0f * Building.BASE_HEALTH_UNIT;
    public float cooldownTime = 5.0f;
    public float altitude = 0.0f;
    public string spritePath = "Sprites/Nature/Thunderstorm";
};

public class Thunderstorm : Disaster
{
    public Thunderstorm() : base() {
        damage = Building.BASE_HEALTH_UNIT;
        cooldownTime = 0.5f;
        altitude = 0.5f;
        spritePath = "Sprites/Nature/Thunderstorm";
    }
};
public class Fire : Disaster
{
    public Fire() : base() {
        damage = 2 * Building.BASE_HEALTH_UNIT;
        cooldownTime = 2.0f;
        altitude = 0.3f;
        spritePath = "Sprites/Nature/Fire";
    }
};

public class Tornado : Disaster
{
    public Tornado() : base() {
        damage = 3 * Building.BASE_HEALTH_UNIT;
        cooldownTime = 3.0f;
        altitude = 0.4f;
        spritePath = "Sprites/Nature/Tornado";
    }
};

public class Tsunami : Disaster
{
    public Tsunami() : base() {
        damage = 4f * Building.BASE_HEALTH_UNIT;
        cooldownTime = 6.0f;
        altitude = 0.1f;
        spritePath = "Sprites/Nature/Tsunami";
    }
};

public class Meteor : Disaster
{
    public Meteor() : base() {
        damage = 100.0f * Building.BASE_HEALTH_UNIT;
        cooldownTime = 20.0f;
        altitude = 1.0f;
        spritePath = "Sprites/Nature/Meteor";
    }
};