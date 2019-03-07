using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
    public string terrainType = "TreeTerrain";

    // Start is called before the first frame update
    void Start()
    {
        // terrainSprite = Resources.Load<Sprite>(terrainType);
        // GetComponent<SpriteRenderer>().sprite = terrainSprite;
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
