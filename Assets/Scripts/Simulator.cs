using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Simulator : MonoBehaviour
{
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

    private static System.Random rnd = new System.Random();

    // Store the buildings that have been unlocked.
    private List<Building> buildingTypes = new List<Building>() {
        new Settlement(),
        new Farm(),
        new House(),
        new Trash(),
        new Factory(),
        new Skyscraper(),
    };

    private List<bool> buildingUnlockedMask = new List<bool>() {
        false,
        false,
        false,
        false,
        false,
        false
    };

    private Dictionary<Type, Building> buildingUpgradeMap = new Dictionary<Type, Building> {
        {typeof(Farm), new Settlement()},
        {typeof(Settlement), new House()},
        {typeof(House), new Factory()},
        {typeof(Factory), new Skyscraper()}
    };

    private List<int> upgradeIndex = new List<int>() {
        1, // 0 --> 1
        2, // 1 --> 2
        4, // 2 --> 4
        -1, // Trash doesn't upgrade
        5, // Factory --> Skyscraper
        -1 // Skyscraper doesn't upgrade
    };

    // Visual indicators.
    private Text populationMeter;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GetComponent<GameManager>();

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

    private int SampleBuildingUniform()
    {
        List<int> available = new List<int>();
        for (int i = 0; i < buildingTypes.Count; ++i) {
            if (buildingUnlockedMask[i]) { available.Add(i); }
        }
        return rnd.Next(available.Count);
    }

    // Randomly decide whether to initialize a building on an empty tile.
    // Assumes that the "mid" building is empty.
    private void BuildStochastic(int i, Building mid, Building left, Building right)
    {
        bool mEmpty = (mid.GetType() == typeof(Building));
        bool lEmpty = (left.GetType() == typeof(Building));
        bool rEmpty = (right.GetType() == typeof(Building));

        // CASE 1: Empty square, decide whether to init.
        // We decide whether to initialize a building with some probability,
        // then uniformly choose from the available buildings.
        if (mEmpty) {
            float initProbability = (lEmpty && rEmpty) ? 0.003f : 0.006f;
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= initProbability) {
                int randomIdx = SampleBuildingUniform();
                gameManager.SetBuilding(i, buildingTypes[randomIdx]);
            }
        
        // CASE 2: Square has a building, upgrade it with some probability.
        } else {
            float upgradeProbability = (lEmpty && rEmpty) ? 0.005f : 0.01f;
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= upgradeProbability) {
                if (buildingUpgradeMap.ContainsKey(mid.GetType())) {
                    gameManager.SetBuilding(i, buildingUpgradeMap[mid.GetType()]);
                }
            }
        }
    }

    private int WrapAround(int i, int max) {
        if (i < 0) {
            i = (36 + i);
        } else if (i >= max) {
            i = i % max;
        }
        return i;
    }

    // Called at the end of every year.
    private void OnYearStart()
    {
        // Update the population.
        population = (long)((float)population * popGrowthFactor);

        // Update the technology level (don't do this at year 0).
        if (currentYear > initialYear) {
            techLvl += techGrowthFactor * population;
        }

        // Determine which buildings are unlocked.
        for (int i = 0; i < buildingTypes.Count; ++i) {
            Building btype = buildingTypes[i];
            if (currentYear >= btype.unlockedYear) {
                buildingUnlockedMask[i] = true;
            }
        }

        // Randomly upgrade/init tiles.
        List<BuildingManager> managers = gameManager.BuildingManagers();
        for (int i = 0; i < managers.Count; ++i) {
            int l_index = WrapAround(i - 1, 36);
            int r_index = WrapAround(i + 1, 36);

            Building mid = managers[i].building;
            Building left = managers[l_index].building;
            Building right = managers[r_index].building;

            BuildStochastic(i, mid, left, right);
        }

        double populationReadable = Math.Round((double)population / 1e9, 3);

        populationMeter.text = "Year: " + currentYear +
                        "\nPopulation: " + populationReadable + " billion" +
                        "\nTechnology: " + techLvl;

        if (techLvl >= gameoverTechLvl) {
            OnGameOver();
        }
    }

    // Kill of a percentage of the population.
    public void KillPopulation(float percent)
    {
        population -= (long)(percent * (double)population);
    }

    public void KillTechnology(float percent)
    {
        techLvl -= percent;
        techLvl = Mathf.Max(0, techLvl);
    }

    private void GotoLoseScreen()
    {
        SceneManager.LoadScene("loseScene");
    }

    private void OnGameOver()
    {
        Debug.Log("GAMEOVER! Humans reached max technology level.");
        gameManager.OnGameOver();
        Invoke("GotoLoseScreen", 5.0f);
    }
}
