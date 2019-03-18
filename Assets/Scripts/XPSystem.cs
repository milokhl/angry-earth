using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XPSystem : MonoBehaviour
{
    public float startingXP = 0.0f;
    public float maxXP = 100;                           
    public float currentXP;                                   
    public Slider XPSlider;

    // Start is called before the first frame update
    void Start()
    {
        XPSlider.value = startingXP;
    }

    // Update is called once per frame
    void Update()
    {
        XPSlider.value = currentXP / maxXP;
    }

    public void EarnXP(float xpEarned)
    {
        currentXP += xpEarned;
        //XPSlider.value = currentXP / maxXP;
    }
}
