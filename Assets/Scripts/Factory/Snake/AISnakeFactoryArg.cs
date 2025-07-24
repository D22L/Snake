using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISnakeFactoryArg
{
    public AISnakeFactoryArg(SnakeView pfb, SnakeSettings settings, Vector3 startPosition, Vector3 normal)
    {
        Pfb = pfb;
        Settings = settings;
        StartPosition = startPosition;
        Normal = normal;
    }

    public SnakeView Pfb { get; private set; }
    public SnakeSettings Settings { get; private set; }
    public Vector3 StartPosition { get; private set; }
    public Vector3 Normal { get; private set; }


}
