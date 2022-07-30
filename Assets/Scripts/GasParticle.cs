using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GasParticle : Particle
{
    public override void AllSet(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        base.AllSet(pos, scale, rot, index);

        color = new Color(199f / 255f, 220f / 255f, 208f / 255f, 32f / 255f);
    }

    public override void Fall(Particle[,] grid, int width, int height)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        int rand = UnityEngine.Random.Range(1, 101);

        if (y != height - 1 && grid[x, y + 1] == null && rand <= 75)
        {
            MoveUp(grid);
        }
        else if ((x != 0 && grid[x - 1, y] == null || x != width - 1 && grid[x + 1, y] == null) && rand > 75)
        {

            if (rand <= 88)
            {
                if (x != 0 && grid[x - 1, y] == null)
                {
                    {
                        MoveLeft(grid);
                    }
                }
            }
            else
            {
                if (x != width - 1 && grid[x + 1, y] == null)
                {
                    MoveRight(grid);
                }
            }
        }
        if (this.pos != new Vector2(x, y))
        {
            this.gameObject.transform.position = pos;
        }
    }

    protected virtual void MoveUp(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        grid[x, y + 1] = grid[x, y];
        grid[x, y] = null;
        this.pos.y++;
    }
}


