using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
    public string terrainType = "TreeTerrain";

    private bool isHighlighted = false;
    
    // Hightlight or unhighlight a tile.
    public void Highlight(bool on)
    {
        isHighlighted = on;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Behaviour h = (Behaviour)GetComponent("Halo");
        h.enabled = isHighlighted; 
    }

    // Access the sprite that is being rendered for this tile.
    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
}
