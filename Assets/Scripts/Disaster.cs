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
    public float damage = 10.0f;
    public float cooldownTime = 10.0f;
    public float altitude = 0.0f;
    public string spritePath = "Sprites/Nature/Thunderstorm";
};

public class Thunderstorm : Disaster
{
    public Thunderstorm() : base() {
        damage = 10.0f;
        cooldownTime = 10.0f;
        altitude = 0.5f;
        spritePath = "Sprites/Nature/Thunderstorm";
    }
};
public class Fire : Disaster
{
    public Fire() : base() {
        damage = 15.0f;
        cooldownTime = 10.0f;
        altitude = 0.1f;
        spritePath = "Sprites/Nature/Fire";
    }
};

public class Tornado : Disaster
{
    public Tornado() : base() {
        damage = 20.0f;
        cooldownTime = 5.0f;
        altitude = 0.4f;
        spritePath = "Sprites/Nature/Tornado";
    }
};

public class Tsunami : Disaster
{
    public Tsunami() : base() {
        damage = 40.0f;
        cooldownTime = 15.0f;
        altitude = 0.0f;
        spritePath = "Sprites/Nature/Tsunami";
    }
};

public class Meteor : Disaster
{
    public Meteor() : base() {
        damage = 1000.0f;
        cooldownTime = 20.0f;
        altitude = 5.0f;
        spritePath = "Sprites/Nature/Meteor";
    }
};