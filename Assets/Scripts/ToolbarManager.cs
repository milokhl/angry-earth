using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarManager : MonoBehaviour
{
    private List<Button> buttons_ = new List<Button>();

    void Start()
    {
        // Get a reference to the GameManager. It must be the parent of this
        // component or this will break!
        GameManager gm = transform.parent.gameObject.GetComponent<GameManager>();

        // Get all buttons that are tagged with 'DisasterButton'.
        int numChildren = transform.childCount;
        for (int i = 0; i < numChildren; ++i) {
            Transform child = transform.GetChild(i);
            if (child.gameObject.tag == "DisasterButton") {
                Button button = child.gameObject.GetComponent<Button>();

                // Set up a click callback on each button. The button will pass its own disaster
                // type into the callback function to notify the GameManager which button was clicked.
                ButtonController controller = button.GetComponent<ButtonController>();
                button.onClick.AddListener(delegate {
                    gm.DisasterButtonClickHandler(controller.disasterType, controller);
                });

                buttons_.Add(button);
            }
        }
    }
    public void UnlockButton(int buttonIndex)
    {
        buttons_[buttonIndex].GetComponent<ButtonController>().Unlock();

    }

}
