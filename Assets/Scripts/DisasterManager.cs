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

public class DisasterManager : MonoBehaviour
{
    // Map each DisasterType to a damage amount.
    public Dictionary<DisasterType, float> Damages = new Dictionary<DisasterType, float> {
        {DisasterType.Thunderstorm, 10.0f},
        {DisasterType.Sandstorm, 5.0f},
        {DisasterType.Earthquake, 12.0f},
        {DisasterType.Tsunami, 6.0f},
        {DisasterType.ForestFire, 7.0f}
    };

    // Map each DisasterType to a cooldown time.
    public Dictionary<DisasterType, float> CooldownTimes = new Dictionary<DisasterType, float> {
        {DisasterType.Thunderstorm, 10.0f},
        {DisasterType.Sandstorm, 5.0f},
        {DisasterType.Earthquake, 12.0f},
        {DisasterType.Tsunami, 6.0f},
        {DisasterType.ForestFire, 7.0f}
    };

    // The type of this disaster instance.
    public DisasterType type;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
}
