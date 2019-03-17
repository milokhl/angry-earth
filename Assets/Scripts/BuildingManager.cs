using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour {
    public Building building = new Building();
    private bool isHighlighted = false;
    private Behaviour halo = null;

    // Start is called before the first frame update
    void Start()
    {
        // For now, the Halo component is what's used for highlighting.
        // We might want to change this later on.
        halo = (Behaviour)GetComponent("Halo");

        // Each type of building has a path to a sprite.
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(building.spritePath);
    }

    // Update is called once per frame
    void Update()
    {
        if (halo != null) {
            halo.enabled = isHighlighted;
        }
    }

    // Hightlight or unhighlight a tile.
    public void Highlight(bool on)
    {
        isHighlighted = on;
    }

    // Access the sprite that is being rendered for this tile.
    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
    public bool Attack(DisasterType disaster)
    {
        if (disaster != DisasterType.NotSelected)
        {
            float damage = DisasterManager.Info[disaster].damage;
            building.health -= damage;
            if (building.health <= 0)
            {
                return true;
            }
        }
        return false;

    }
}
