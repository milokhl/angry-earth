using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private float earthRadius = 5.0f;
    public int numTiles = 36;
    public GameObject tilePrefab;
    private List<TileManager> tiles_ = new List<TileManager>();

    // Start is called before the first frame update
    void Start()
    {
        // Tile angular width in degrees and radians.
        float tileDeg = 360.0f / numTiles;
        float tileRad = tileDeg * Mathf.PI / 180.0f;
        float tileArcLength = tileRad * earthRadius;

        for (int i = 0; i < numTiles; ++i) {
            float tileCenterDeg = (float)i * tileDeg - 90.0f;
            float tileCenterRad = (float)i * tileRad;

            GameObject tile = Instantiate(tilePrefab);
            TileManager manager = tile.GetComponent<TileManager>();

            // We want to scale the tile width so that it takes up all of the available arc length.
            float spriteWidth = manager.GetSprite().bounds.max[0] - manager.GetSprite().bounds.min[0];
            float spriteHeight = manager.GetSprite().bounds.max[1] - manager.GetSprite().bounds.min[1];
            float tileScaleFactor = tileArcLength / spriteWidth;

            // Rotate and translate the tile so that it is on the edge of the earth.
            // Quaternion.Euler expects degrees!
            Quaternion tileRot = Quaternion.Euler(0, 0, tileCenterDeg);

            // Move the tile radially outward by earthRadius.
            Vector3 tileNormal = new Vector3(Mathf.Cos(tileCenterRad), Mathf.Sin(tileCenterRad), 0.0f);
            Vector3 tilePos = (earthRadius + 0.5f *spriteHeight) * tileNormal;
            tile.transform.position = tilePos;
            tile.transform.rotation = tileRot;

            // GameObject tile = Instantiate(tilePrefab, tilePos, tileRot);
            tiles_.Add(manager);
        }

    }

    
}
