using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MovableSolidParticle : SolidParticle
{
    public override void AllSet(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        base.AllSet(pos, scale, rot, index);

        gameObject.layer = 7;
    }

    public override void EnableHitBox()
    {
        base.EnableHitBox();

        if (!boxCollider)
        {
            boxCollider.usedByEffector = true;
            PlatformEffector2D platformEffector = gameObject.AddComponent<PlatformEffector2D>();
            platformEffector.colliderMask = LayerMask.GetMask("Player");
        }
    }

    public override void DisableHitBox()
    {
        if (boxCollider)
        {
            Destroy(gameObject.GetComponent<PlatformEffector2D>());
            base.DisableHitBox();
        }
    }

    public override void Fall(Particle[,] grid, int width, int height)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        if (y != 0)
            if (grid[x, y - 1] == null || grid[x, y - 1] is LiquidParticle)
            {
                if (grid[x, y - 1] == null)
                {
                    MoveDown(grid);
                }
                else
                {
                    MoveDownSwitch(grid);
                }
                stationary = false;
            }
            else if (!stationary)
            {
                int rand = UnityEngine.Random.Range(0, 100);
                if (rand < 49)
                {
                    if (x != 0 && grid[x - 1, y] == null && grid[x - 1, y - 1] == null)
                    {
                        MoveLeft(grid);
                    }
                    else if ((x != 0) && (grid[x - 1, y] == null && grid[x - 1, y - 1] is LiquidParticle ||
                            grid[x - 1, y] is LiquidParticle && grid[x - 1, y - 1] == null ||
                            grid[x - 1, y] is LiquidParticle && grid[x - 1, y - 1] is LiquidParticle))
                    {
                        MoveLeftSwitch(grid);
                    }
                }
                else if (rand > 51)
                {
                    if (x != width - 1 && grid[x + 1, y] == null && grid[x + 1, y - 1] == null)
                    {
                        MoveRight(grid);
                    }
                    else if ((x != width - 1) && (grid[x + 1, y] == null && grid[x + 1, y - 1] is LiquidParticle ||
                            grid[x + 1, y] is LiquidParticle && grid[x + 1, y - 1] == null ||
                            grid[x + 1, y] is LiquidParticle && grid[x + 1, y - 1] is LiquidParticle))
                    {
                        MoveRightSwitch(grid);
                    }
                }
                else
                {
                    stationary = true;
                }

            }
        if (boxCollider && (this.rigidbody.position != this.pos && boxCollider.enabled))
        {
            this.rigidbody.MovePosition(pos);
        }
    }
}