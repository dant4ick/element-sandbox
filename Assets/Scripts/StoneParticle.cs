using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneParticle : ImmovableSolidParticle
{
    public override void AllSet(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        base.AllSet(pos, scale, rot, index);
        int colorIndex = UnityEngine.Random.Range(0, 5);
        switch (colorIndex)
        {
            case 0:
                color = new Color(127f / 255f, 112f / 255f, 138f / 255f);
                break;
            case int rand when (rand > 0):
                color = new Color(98f / 255f, 85f / 255f, 101f / 255f);
                break;
        }
    }
}
