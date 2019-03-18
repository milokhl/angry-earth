using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Simulator : MonoBehaviour
{
    private const float e = 2.7182818f;

    // Initial state.
    public int initialYear = 1800;
    public long initialPopulation = 1000000000;
    public float popGrowthFactor = 1.011f;
    public float secondsPerYear = 0.1f; // 5 minutes to go 1800 -> 2100
    public float initialTechLvl = 0.0f;

    // Current state.
    private long population;
    private int currentYear;
    private float globalTimer;
    private float yearTimer;
    private float techLvl;
    private float gameoverTechLvl = 100.0f;
    private int targetGameoverYear = 2100;
    private float techGrowthFactor;

    // Visual indicators.
    private Text populationMeter;

    // Start is called before the first frame update
    void Start()
    {
        population = initialPopulation;
        currentYear = initialYear;

        globalTimer = 0.0f;
        yearTimer = secondsPerYear;

        techLvl = initialTechLvl;

        // Set the techGrowthFactor so that an undisturbed human civilization would reach
        // gameoverTechLvl by targetGameoverYear. This equation assumes that the techLvl
        // increments at a rate proportional to the human population (techGrowthFactor).
        float numYrs = (float)(targetGameoverYear - initialYear);
        techGrowthFactor = gameoverTechLvl * (popGrowthFactor - 1) /
                           (initialPopulation * popGrowthFactor * (Mathf.Pow(popGrowthFactor, numYrs) - 1));

        populationMeter = GameObject.Find("PopulationMeter").GetComponent<Text>();

        OnYearStart();
    }

    void Update()
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
        // Update the population.
        population = (long)((float)population * popGrowthFactor);

        // Update the technology level (don't do this at year 0).
        if (currentYear > initialYear) {
            techLvl += techGrowthFactor * population;
        }

        populationMeter.text = "Year: " + currentYear +
                        "\nPopulation: " + population +
                        "\nTechnology: " + techLvl;

        if (techLvl >= gameoverTechLvl) {
            OnGameOver();
        }
    }

    void OnGameOver()
    {
        Debug.Log("GAMEOVER! Humans reached max technology level.");
    }
}
