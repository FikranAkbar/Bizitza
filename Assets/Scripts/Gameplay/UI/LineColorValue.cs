using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LineColorValue
{
    static Color horizontalLine = new Color(1f, 0.68f, 0.26f); // Yellow Orange
    static Color verticalLine = new Color(154f/255f, 205f/255f, 50/255f); // Yellow Green
    static Color upLine = new Color(13f/255f, 152f/255f, 186f/255f); // Blue Green
    static Color downLine = new Color(138f/255f, 43f/255f, 226f/255f); // Blue Violet
    static Color rightLine = new Color(199f/255f, 21f/255f, 133f/255f); // Red Violet
    static Color leftLine = new Color(255f/255f, 83f/255f, 73f/255f); // Red Orange

    public static Color GetColor(string id)
    {
        if (id == "horizontal")
        {
            return horizontalLine;
        }
        else if (id == "vertical")
        {
            return verticalLine;
        }
        else if (id == "up")
        {
            return upLine;
        }
        else if (id == "down")
        {
            return downLine;
        }
        else if (id == "right")
        {
            return rightLine;
        }
        else if (id == "left")
        {
            return leftLine;
        }
        else return Color.white;
    }
 }
