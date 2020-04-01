using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils : MonoBehaviour
{
    /// <summary>
    /// Detect if cursor is over a UI
    /// </summary>
    /// <returns></returns>
    public static bool IsCursorOverUserInterface()
    {
        // IsPointerOverGameObject check for left mouse (default)
        if (EventSystem.current.IsPointerOverGameObject())
            return true;

        // IsPointerOverGameObject check for touches
        for (int i = 0; i < Input.touchCount; ++i)
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                return true;

        // OnGUI check
        return GUIUtility.hotControl != 0;
    }

    /// <summary>
    /// Return the axis value
    /// </summary>
    /// <returns></returns>
    public static float GetZoomUniversal()
    {
        if (Input.mousePresent)
            return Utils.GetAxisRawScrollUniversal();
        return 0;
    }

    /// <summary>
    /// Return the axis value within the range -1 : 1
    /// </summary>
    /// <returns></returns>
    public static float GetAxisRawScrollUniversal()
    {
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scroll < 0) return -1;
        if (scroll > 0) return 1;
        return 0;
    }
}
