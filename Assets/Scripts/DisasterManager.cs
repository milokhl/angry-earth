using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// These define all the possible disaster types.
// I thought that this might be less prone to
// errors than using a string representation.
public enum DisasterType
{
    NotSelected,
    Thunderstorm,
    Sandstorm,
    Earthquake,
    Tsunami,
    ForestFire
};

// Defines all of the behavior of the disaster.
public struct DisasterInfo
{
    public float damage;            // The damage that this disaster subtracts from health.
    public float cooldownTime;      // Seconds.
    public float altitude;          // For positioning disaster above the earth surface.
    public string spriteFile;       // Filename (not including directory).
};

public class DisasterManager : MonoBehaviour
{
    private string spritePath = "Sprites/Placeholders/";

    public static Dictionary<DisasterType, DisasterInfo> Info = new Dictionary<DisasterType, DisasterInfo>
    {
        {DisasterType.Thunderstorm,
            new DisasterInfo {
                damage = 10.0f,
                cooldownTime=10.0f,
                altitude=1.0f,
                spriteFile="TransparentLightning"}
            },
        {DisasterType.Sandstorm,
            new DisasterInfo {
                damage = 10.0f,
                cooldownTime=10.0f,
                altitude=0.3f,
                spriteFile="TransparentSandstorm"}
            },
        {DisasterType.Earthquake,
            new DisasterInfo {
                damage = 10.0f,
                cooldownTime=10.0f,
                altitude=1.0f,
                spriteFile="TransparentLightning"}
            },
        {DisasterType.Tsunami,
            new DisasterInfo {
                damage = 10.0f,
                cooldownTime=10.0f,
                altitude=0.0f,
                spriteFile="TransparentLightning"}
            },
        {DisasterType.ForestFire,
            new DisasterInfo {
                damage = 10.0f,
                cooldownTime=10.0f,
                altitude=0.05f,
                spriteFile="TransparentFire"}
            },
    };
    
    // The type of this disaster instance.
    public DisasterType instanceType;

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public void SetType(DisasterType type)
    {
        instanceType = type;
        DisasterInfo info = Info[instanceType];
        string file = spritePath + info.spriteFile;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(file);
    }
}
