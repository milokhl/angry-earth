using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public DisasterType type; // See DisasterManager.cs for supported types.
    public bool isUnlocked = false;

    // Keeps track of the time until this button is available.
    // Any time <= 0 means it is available.
    private float countdown = 0.0f;

    public void Start()
    {
        GetComponent<Button>().interactable = (countdown >= 0.0f && isUnlocked);
    }

    // Disable the button for a specified number of seconds.
    // Once the timer expires, it should be re-enabled.
    public void DisableForCountdown(float seconds)
    {
        countdown = seconds;
        GetComponent<Button>().interactable = false;
    }

    // Immediately set the countdown timer to zero.
    public void StopCountdown()
    {
        countdown = 0.0f;
        GetComponent<Button>().interactable = true;
    }

    // Unlock or lock the disaster (based on player's current level).
    public void Unlock() { isUnlocked = true; }
    public void Lock() { isUnlocked = false; }

    // Decrements the countdown timer and re-enables the button
    // when it hits zero.
    void FixedUpate()
    {
        if (countdown > 0.0f) {
            countdown -= Time.deltaTime;
        } else {
            if (isUnlocked) {
                StopCountdown();
            }
        }
    }
}
