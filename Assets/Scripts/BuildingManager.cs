using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
    private bool isHighlighted = false;
    private Behaviour halo = null;
    
    // Hightlight or unhighlight a tile.
    public void Highlight(bool on)
    {
        isHighlighted = on;
    }

    // Start is called before the first frame update
    void Start()
    {
        // For now, the Halo component is what's used for highlighting.
        // We might want to change this later on.
        halo = (Behaviour)GetComponent("Halo");
    }

    // Update is called once per frame
    void Update()
    {
        if (halo != null) {
            halo.enabled = isHighlighted;
        }
    }

    // Access the sprite that is being rendered for this tile.
    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
}
