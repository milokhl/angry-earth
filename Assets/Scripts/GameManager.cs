using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private float earthRadius = 5.0f;

    // The number of tiles on earth's surface.
    public int numTiles = 36;

    // Prefab for a generic building type.
    public GameObject buildingPrefab;

    // Each Tile prefab has a BuildingManager script attached.
    private List<BuildingManager> tiles_ = new List<BuildingManager>();
    private BuildingManager activeTile = null;

    public GameObject disasterPrefab;
    
    // Player state.
    // The currently clicked disaster type.
    private DisasterType selectedDisaster = DisasterType.NotSelected;

    // Every time a button in the toolbar is clicked, it calls this method
    // with its DisasterType (see DisasterManager.cs)
    private Dictionary<DisasterType, Type> DisasterTypeToClass = new Dictionary<DisasterType, Type>
    {
        {DisasterType.Thunderstorm, typeof(Thunderstorm)},
        {DisasterType.Fire, typeof(Fire)},
        {DisasterType.Tornado, typeof(Tornado)},
        {DisasterType.Tsunami, typeof(Tsunami)},
        {DisasterType.Meteor, typeof(Meteor)}
    };

    public void DisasterButtonClickHandler(DisasterType type, ButtonController controller)
    {
        selectedDisaster = type;
    }

    // Start is called before the first frame update
    public void Start()
    {
        InitializeTiles();

        Transform toolbar = transform.Find("Toolbar");
    }

    public void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        // The mouse position is in screen pixel coordinates, and we need to
        // center it relative to the world.
        Vector2 centeredPos = mousePos - 0.5f * new Vector2(Screen.width, Screen.height);
        BuildingManager active = GetActiveTile(centeredPos.x, centeredPos.y);

        if (activeTile != null) {
            activeTile.Highlight(false); // Turn off previously active tile.
        }
        activeTile = active;
        activeTile.Highlight(true);

        // if the mouse button is held down, create a disaster in the correct section
        if (Input.GetMouseButtonDown(0))
        {
            int activeI = GetActiveIndex(centeredPos.x, centeredPos.y);

            if (selectedDisaster != DisasterType.NotSelected) {
                PlaceDisaster(activeI);

                Type SelectedDisasterT = DisasterTypeToClass[selectedDisaster];
                Disaster instance = (Disaster)Activator.CreateInstance(SelectedDisasterT);

                bool destroyed = active.Attack(instance);
                if (destroyed) {
                    activeTile.Destroy(instance);
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
            // For now, let's let tiles be randomly either a tree (75% chance) or a settlement (25% chance)
            UnityEngine.Random rand = new UnityEngine.Random();
            GameObject tile = Instantiate(buildingPrefab);
            BuildingManager manager = tile.GetComponent<BuildingManager>();

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
        Destroy(disaster, 1.0f);

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
}
