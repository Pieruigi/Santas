using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.ar.santas
{
    public class Tags
    {
        public static readonly string Santa = "Santa";
        public static readonly string Floor = "Floor";
        public static readonly string Player = "Player";
        public static readonly string Gift = "Gift";
        public static readonly string Building = "Building";
    }

    public class Layers
    {
        public static readonly string TacticsPlane = "TacticsPlane";
        public static readonly string IgnoreRaycast = "Ignore Raycast";
    }

    public class Constants
    {
        public static readonly int LevelIdStartBuildIndex = 1;
        public static readonly int MainSceneBuildIndex = 0;
    }
}
