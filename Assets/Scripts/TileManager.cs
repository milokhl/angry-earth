using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
    public string terrainType = "TreeTerrain";

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    // Access the sprite that is being rendered for this tile.
    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
}
