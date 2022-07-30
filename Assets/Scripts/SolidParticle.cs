using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidParticle : Particle
{
    public override void AllSet(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        base.AllSet(pos, scale, rot, index);
    }
}