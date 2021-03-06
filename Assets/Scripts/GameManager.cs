﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    // Game mode switches.
    bool enableCooldown = true;

    private float earthRadius = 5.0f;

    // The number of tiles on earth's surface.
    public int numTiles = 36;

    private Camera camera;
    private Simulator simulator;

    // Prefab for a generic building type.
    public GameObject buildingPrefab;

    // Each Tile prefab has a BuildingManager script attached.
    private List<BuildingManager> tiles_ = new List<BuildingManager>();
    private BuildingManager activeTile = null;

    public GameObject disasterPrefab;
    public GameObject nukePrefab;
    public GameObject textPrefab;
    
    // Player state.
    // The currently clicked disaster type.
    private DisasterType selectedDisaster = DisasterType.NotSelected;
    private ButtonController selectedButton = null;

    // Every time a button in the toolbar is clicked, it calls this method
    // with its DisasterType (see DisasterManager.cs)
    private Dictionary<DisasterType, Type> DisasterTypeToClass =
        new Dictionary<DisasterType, Type> {
        {DisasterType.Thunderstorm, typeof(Thunderstorm)},
        {DisasterType.Fire, typeof(Fire)},
        {DisasterType.Tornado, typeof(Tornado)},
        {DisasterType.Tsunami, typeof(Tsunami)},
        {DisasterType.Meteor, typeof(Meteor)}
    };

    // slider used for xp
    public GameObject xpSlider;

    private Transform toolbar;

    public void DisasterButtonClickHandler(DisasterType type, ButtonController controller)
    {
        selectedDisaster = type;
        selectedButton = controller;
    }

    // Start is called before the first frame update
    public void Start()
    {
        InitializeTiles();
        toolbar = transform.Find("Toolbar");
        xpSlider = GameObject.Find("XPSlider");
        simulator = GetComponent<Simulator>();
        camera = Camera.main;
    }

    // Get the mouse position in world coordinates.
    public Vector3 MouseInWorld()
    {
        return camera.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera.nearClipPlane));
    }

    // public void PlaceFeedbackText(int i)
    // {
    //     Transform tileTransform = tiles_[i].transform;
    //     Canvas canvas = GameObject.Find("Canvas");
    //     Text feedback = (Text)Instantiate(textPrefab, tileTransform.position, tileTransform.rotation);
    //     feedback.transform.SetParent(canvas.transform, false);
    //     feedback.fontSize = 14;
    //     feedback.text = "asdlfkjsdfkj";
    // }

    public void Update()
    {
        // The mouse position is in screen pixel coordinates, and we need to
        // center it relative to the world.
        Vector3 mouse_in_world = MouseInWorld();
        BuildingManager active = GetActiveTile(mouse_in_world.x, mouse_in_world.y);

        if (activeTile != null) {
            activeTile.Highlight(false); // Turn off previously active tile.
        }
        activeTile = active;
        activeTile.Highlight(true);

        // if the mouse button is held down, create a disaster in the correct section
        if (Input.GetMouseButtonDown(0)) {            
            if (selectedDisaster == DisasterType.Meteor)
            {
                SceneManager.LoadScene("winScene");
            }

            int activeI = GetActiveIndex(mouse_in_world.x, mouse_in_world.y);

            Vector2 mouse_in_world_2d = new Vector2(mouse_in_world.x, mouse_in_world.y);
            float clickRadius = mouse_in_world_2d.magnitude;

            if (selectedDisaster != DisasterType.NotSelected &&
                    clickRadius >= earthRadius &&
                    clickRadius <= 2*earthRadius) {

                PlaceDisaster(activeI);

                Type SelectedDisasterT = DisasterTypeToClass[selectedDisaster];
                Disaster instance = (Disaster)Activator.CreateInstance(SelectedDisasterT);

                // Start the disaster cooldown.
                if (enableCooldown) {
                    selectedButton.DisableForCountdown(instance.cooldownTime);
                    selectedDisaster = DisasterType.NotSelected;
                }

                bool destroyed = active.Attack(instance);

                if (destroyed) {
                    XPSystem system = xpSlider.GetComponent<XPSystem>();
                    float xpLevel = system.EarnXP(active.building.xpGain);

                    simulator.KillPopulation(active.building.populationPct);
                    simulator.KillTechnology(active.building.technologyPct);

                    activeTile.Destroy(instance);

                    ToolbarManager tManager = toolbar.GetComponent<ToolbarManager>();
                    if (xpLevel >= 1.0f) { tManager.UnlockButton(4);
                    } else if (xpLevel >= 0.60f) { tManager.UnlockButton(3);
                    } else if (xpLevel >= 0.25f) { tManager.UnlockButton(2);
                    } else if (xpLevel >= 0.1f) { tManager.UnlockButton(1); } 
                }
            }
        }
    }

    public void SetBuilding(int i, Building instance)
    {
        BuildingManager manager = tiles_[i].GetComponent<BuildingManager>();
        manager.SetBuilding(instance);
    }

    public List<BuildingManager> BuildingManagers()
    {
        return tiles_;
    }

    private void InitializeTiles()
    {
        // Tile angular width in degrees and radians.
        float tileDeg = 360.0f / numTiles;
        float tileRad = tileDeg * Mathf.PI / 180.0f;
        float tileArcLength = tileRad * earthRadius;

        for (int i = 0; i < numTiles; ++i) {
            float tileCenterDeg = (float)i * tileDeg - 90.0f;
            float tileCenterRad = (float)i * tileRad;

            // Instantiate the tile and retrieve its BuildingManager script.
            UnityEngine.Random rand = new UnityEngine.Random();
            GameObject tile = Instantiate(buildingPrefab);
            BuildingManager manager = tile.GetComponent<BuildingManager>();

            float initBuildingProb = 0.25f;
            if (UnityEngine.Random.Range(0.0f, 1.0f) <= initBuildingProb) {
                manager.SetBuilding(new Settlement());
            }

            // We want to scale the tile width so that it takes up all of the available arc length.
            // This allows us to instantiate the tile without knowing about the units / px of the sprite.
            float spriteWidth = manager.GetSprite().bounds.max[0] - manager.GetSprite().bounds.min[0];
            float spriteHeight = manager.GetSprite().bounds.max[1] - manager.GetSprite().bounds.min[1];
            float tileScaleFactor = tileArcLength / spriteWidth; // TODO: this isn't used??

            // Rotate and translate the tile so that it is on the edge of the earth.
            // Quaternion.Euler expects degrees!
            Quaternion tileRot = Quaternion.Euler(0, 0, tileCenterDeg);

            // Move the tile radially outward by earthRadius.
            Vector3 tileNormal = new Vector3(Mathf.Cos(tileCenterRad), Mathf.Sin(tileCenterRad), 0.0f);
            Vector3 tilePos = (earthRadius - 0.1f) * tileNormal;
            tile.transform.position = tilePos;
            tile.transform.rotation = tileRot;

            tiles_.Add(manager);
        }
    }

    // Get the tile that is 'active' given the current angular mouse location.
    BuildingManager GetActiveTile(float x, float y)
    {
        float tileRad = 2.0f * Mathf.PI / numTiles;
        float theta = Mathf.Atan2(y, x);

        // Atan2 maps angles between [-Pi, Pi]. For the math below to work, we
        // want to map these angles from 0 to 2PI.
        if (theta < 0) { theta = 2*Mathf.PI + theta; }

        int below = (int)Mathf.Floor(theta / tileRad);
        int above = (below + 1) % numTiles;

        float belowDelta = (theta - (float)below * tileRad);
        float aboveDelta = (float)above * tileRad - theta;

        // Note: need to handle the wraparound case here where above = 0 and below = 35.
        if (aboveDelta < 0) { aboveDelta = 2.0f * Mathf.PI + aboveDelta; }

        int closest_idx = (belowDelta <= aboveDelta) ? below : above;
        return tiles_[closest_idx];
    }

    //place a disaster in the given i section
    private void PlaceDisaster(int i)
    {
        // Don't place anything if no disaster selected.
        if (selectedDisaster == DisasterType.NotSelected) {
            return;
        }

        // Disaster angular width in degrees and radians.
        float disDeg = 360.0f / numTiles;
        float disRad = disDeg * Mathf.PI / 180.0f;
        float disArcLength = disRad * earthRadius;

        float disCenterDeg = (float)i * disDeg - 90.0f;
        float disCenterRad = (float)i * disRad;

        // Instantiate the disaster and retrieve its DisasterManager script.
        GameObject disaster = Instantiate(disasterPrefab);
        DisasterManager manager = disaster.GetComponent<DisasterManager>();

        Type SelectedDisasterT = DisasterTypeToClass[selectedDisaster];
        manager.disaster = (Disaster)Activator.CreateInstance(SelectedDisasterT); // Give the disaster a type.

        // Countdown to destroy the disaster.
        Destroy(disaster, 2.0f);

        float spriteWidth = manager.GetSprite().bounds.max[0] - manager.GetSprite().bounds.min[0];
        float spriteHeight = manager.GetSprite().bounds.max[1] - manager.GetSprite().bounds.min[1];
        float disScaleFactor = disArcLength / spriteWidth;

        // Rotate, translate, and scale the disaster so that it is above the edge of the earth.
        // Quaternion.Euler expects degrees!
        Quaternion tileRot = Quaternion.Euler(0, 0, disCenterDeg);

        // Move the disaster radially outward by earthRadius.
        Vector3 disNormal = new Vector3(Mathf.Cos(disCenterRad), Mathf.Sin(disCenterRad), 0.0f);

        //May need to change the position and scaling factor depending on the actual
        //size of the sprite
        float altitude = manager.disaster.altitude;

        Vector3 disPos = (earthRadius + altitude * spriteHeight) * disNormal;
        disaster.transform.localScale = Vector3.one * disScaleFactor*2.0f;
        disaster.transform.position = disPos;
        disaster.transform.rotation = tileRot;
    }

    //get the index of the active tile
    private int GetActiveIndex(float x, float y)
    {
        float tileRad = 2.0f * Mathf.PI / numTiles;
        float theta = Mathf.Atan2(y, x);

        // Atan2 maps angles between [-Pi, Pi]. For the math below to work, we
        // want to map these angles from 0 to 2PI.
        if (theta < 0) { theta = 2 * Mathf.PI + theta; }

        int below = (int)Mathf.Floor(theta / tileRad);
        int above = (below + 1) % numTiles;

        float belowDelta = (theta - (float)below * tileRad);
        float aboveDelta = (float)above * tileRad - theta;

        // Note: need to handle the wraparound case here where above = 0 and below = 35.
        if (aboveDelta < 0) { aboveDelta = 2.0f * Mathf.PI + aboveDelta; }

        int closest_idx = (belowDelta <= aboveDelta) ? below : above;
        return closest_idx;
    }

    public void OnGameOver()
    {
        // Tile angular width in degrees and radians.
        float tileDeg = 360.0f / numTiles;
        float tileRad = tileDeg * Mathf.PI / 180.0f;
        float tileArcLength = tileRad * earthRadius;

        for (int i = 0; i < numTiles; ++i) {
            float tileCenterDeg = (float)i * tileDeg - 90.0f;
            float tileCenterRad = (float)i * tileRad;

            if (UnityEngine.Random.Range(0.0f, 1.0f) <= 0.4f) {
                GameObject nuke = Instantiate(nukePrefab);
                Quaternion tileRot = Quaternion.Euler(0, 0, tileCenterDeg);
                Vector3 tileNormal = new Vector3(Mathf.Cos(tileCenterRad), Mathf.Sin(tileCenterRad), 0.0f);
                Vector3 tilePos = (earthRadius - 0.1f) * tileNormal;
                nuke.transform.position = tilePos;
                nuke.transform.rotation = tileRot;
            }
        }
    }
}
