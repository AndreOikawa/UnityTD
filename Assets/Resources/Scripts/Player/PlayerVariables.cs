using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    public enum ClickType {
        SHOOT,
        BUILD
    };
    public ClickType CurrentMode = ClickType.SHOOT;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        var prev = CurrentMode;
        if (Input.GetAxis("Mouse ScrollWheel") < 0f) CurrentMode = ClickType.SHOOT;
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f) CurrentMode = ClickType.BUILD;
        if (prev != CurrentMode)
            Debug.Log(CurrentMode);
    }
}
