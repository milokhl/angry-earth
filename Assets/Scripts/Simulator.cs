using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simulator : MonoBehaviour
{
    public int initialYear = 1800;
    public int initialPopulation = 1000000000;
    public float yearlyGrowthFactor = 1.01f;
    public float secondsPerYear = 1.0f; // 5 minutes to go 1800 -> 2100

    private int population;
    private int currentYear;
    private float globalTimer;
    private float yearTimer;

    private Text populationMeter;

    // Start is called before the first frame update
    void Start()
    {
        population = initialPopulation;
        currentYear = initialYear;

        globalTimer = 0.0f;
        yearTimer = secondsPerYear;

        populationMeter = GameObject.Find("PopulationMeter").GetComponent<Text>();

        OnYearStart();
    }

    void FixedUpdate()
    {
        globalTimer += Time.deltaTime;
        yearTimer -= Time.deltaTime;

        if (yearTimer <= 0) {
            yearTimer = secondsPerYear;
            currentYear += 1;

            // Trigger any actions that occur at the start of the year.
            OnYearStart();
        }
    }

    // Called at the end of every year.
    void OnYearStart()
    {
        population = (int)((float)population * yearlyGrowthFactor);
        Debug.Log("Population: " + population);

        populationMeter.text = "Year: " + currentYear + "\nPopulation: " + population;
    }
}
