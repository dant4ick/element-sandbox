using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LiquidParticle : Particle
{
    protected int liquidity;

    public override void AllSet(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        base.AllSet(pos, scale, rot, index);
        gameObject.layer = LayerMask.NameToLayer("Water");
    }

    public override void Fall(Particle[,] grid, int width, int height)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        if (y != 0 && grid[x, y - 1] == null)
        {
            MoveDown(grid);
            stationary = false;
        }
        else if (x != 0 && grid[x - 1, y] == null || x != width - 1 && grid[x + 1, y] == null)
        {
            int rand = UnityEngine.Random.Range(1, 101);
            if (rand <= 50)
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
    protected override void MoveDown(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        int moveValue = 0;

        int rand = UnityEngine.Random.Range(2, this.liquidity + 1);

        for (int distance = 1; distance < rand; distance++)
        {
            if (y - distance >= 0 && grid[x, y - distance] == null)
            {
                moveValue++;
            }
            else
            {
                break;
            }
        }

        grid[x, y - moveValue] = grid[x, y];
        grid[x, y] = null;
        this.pos.y-=moveValue;
    }

    protected override void MoveLeft(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        int moveValue = 0;

        int rand = UnityEngine.Random.Range(2, this.liquidity + 1);

        for (int distance = 1; distance < rand; distance++)
        {
            if (x - distance >= 0 && grid[x - distance, y] == null)
            {
                moveValue++;
            }
            else
            {
                break;
            }
        }
        grid[x - moveValue, y] = grid[x, y];
        grid[x, y] = null;
        this.pos.x -= moveValue;        
    }

    protected override void MoveRight(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        int moveValue = 0;

        int rand = UnityEngine.Random.Range(2, this.liquidity + 1);

        for (int distance = 1; distance < rand; distance++)
        {
            if (x + distance < 356 && grid[x + distance, y] == null)
            {
                moveValue++;
            }
            else
            {
                break;
            }
        }

        grid[x + moveValue, y] = grid[x, y];
        grid[x, y] = null;
        this.pos.x += moveValue;
        
    }

    //protected bool MoveLeftDiagonally(Particle[,] grid)
    //{
    //    int x = Convert.ToInt32(this.pos.x);
    //    int y = Convert.ToInt32(this.pos.y);

    //    int moveValue = 0;

    //    for (int distance = 1; distance < this.liquidity; distance++)
    //    {
    //        if (x - distance >= 0 && y - distance >= 0 && grid[x - distance, y - distance] == null)
    //        {
    //            moveValue++;
    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }
    //    if (moveValue == 0)
    //    {
    //        return false;
    //    }
    //    grid[x - moveValue, y - moveValue] = grid[x, y];
    //    grid[x, y] = null;
    //    this.pos.x -= moveValue;
    //    this.pos.y -= moveValue;
    //    return true;
    //}
}