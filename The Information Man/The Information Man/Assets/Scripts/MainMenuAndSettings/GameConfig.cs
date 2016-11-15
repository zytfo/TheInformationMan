using UnityEngine;
using System.Collections;

public static class GameConfig
{
    public static string language = "British English";
    public static string lighting = "Average";
    public static string superSampling = "On";
    public static string motionBlur = "On";
    public static string blood = "Off";
    public static string mode3D = "Off";
    public static string smoothing = "On";
    public static string textures = "Ultra";
    public static string polygones = "Average";
    public static string effects = "Average";
    public static string postprocessing = "Average";
    public static string grid = "Average";
    public static string environment = "Average";
    public static string grass = "Average";
    public static string FXAA = "On";
    public static string MSAA = "On";
    public static string antialiasing = "0";
    public static string alphaBlending = "On";
    public static int resolutionWidth = Screen.width; //Screen.currentResolution.width;
    public static int resolutionHeight = Screen.height;
    public static string fullscreen = Screen.fullScreen ? "On" : "Off";

    public static bool[] resolutions = { false, false, false, false, false };
}
    