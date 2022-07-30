using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class WaterParticle : LiquidParticle
{
    public override void AllSet(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        base.AllSet(pos, scale, rot, index);
        color = new Color(77f / 255f, 155f / 255f, 230f / 255f, 51f / 255f);
        liquidity = 4;
    }

}
