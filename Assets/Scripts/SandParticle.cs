using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SandParticle : MovableSolidParticle
{
    public override void AllSet(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        base.AllSet(pos, scale, rot, index);
        int colorIndex = UnityEngine.Random.Range(0, 2);
        switch (colorIndex)
        {
            case 0:
                color = new Color(249f / 255f, 194f / 255f, 43f / 255f);
                break;
            case 1:
                color = new Color(247f / 255f, 150f / 255f, 23f / 255f);
                break;
        }
    }
}
