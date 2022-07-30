using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Particle : MonoBehaviour
{
    public Vector2 pos;
    public Vector3 scale;
    public Quaternion rot;
    public Material objMat;

    public BoxCollider2D boxCollider;
    public new Rigidbody2D rigidbody;

    protected Color color;

    protected int health;

    public int index;

    public bool stationary = false;

    public Matrix4x4 matrix
    {
        get
        {
            return Matrix4x4.TRS(pos, rot, scale);
        }
    }

    public Color GetColor
    {
        get { return color; }
    }

    public Particle() { }

    public Particle(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        this.pos = pos;
        this.scale = scale;
        this.rot = rot;
        this.index = index;
    }

    public virtual void AllSet(Vector2 pos, Vector3 scale, Quaternion rot, int index)
    {
        this.pos = pos;
        this.scale = scale;
        this.rot = rot;
        this.index = index;

        this.gameObject.transform.position = pos;
    }

    public virtual void Fall(Particle[,] grid, int width, int height) { }

    protected virtual void MoveDown(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        grid[x, y - 1] = grid[x, y];
        grid[x, y] = null;
        this.pos.y--;
    }

    protected virtual void MoveLeft(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        grid[x - 1, y] = grid[x, y];
        grid[x, y] = null;
        this.pos.x--;
    }

    protected virtual void MoveRight(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        grid[x + 1, y] = grid[x, y];
        grid[x, y] = null;
        this.pos.x++;
    }

    protected void MoveDownSwitch(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        Particle bottomParticle = grid[x, y - 1];

        grid[x, y - 1] = grid[x, y];
        grid[x, y] = bottomParticle;

        this.pos.y--;
        bottomParticle.pos.y++;
    }

    protected void MoveLeftSwitch(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        Particle leftParticle = grid[x - 1, y];

        grid[x - 1, y] = grid[x, y];
        grid[x, y] = leftParticle;

        if (this != null)
            this.pos.x--;
        if (leftParticle != null)
            leftParticle.pos.x++;
    }

    protected void MoveRightSwitch(Particle[,] grid)
    {
        int x = Convert.ToInt32(this.pos.x);
        int y = Convert.ToInt32(this.pos.y);

        Particle rightParticle = grid[x + 1, y];

        grid[x + 1, y] = grid[x, y];
        grid[x, y] = rightParticle;

        if (this != null)
            this.pos.x++;
        if (rightParticle != null)
            rightParticle.pos.x--;
    }

    public virtual void EnableHitBox()
    {
        if (boxCollider) return;
        this.boxCollider = gameObject.AddComponent<BoxCollider2D>();

        this.rigidbody = gameObject.AddComponent<Rigidbody2D>();
        this.rigidbody.bodyType = RigidbodyType2D.Kinematic;
        this.rigidbody.MovePosition(pos);
    }

    public virtual void DisableHitBox()
    {
        if (!boxCollider) return;
        Destroy(this.rigidbody);
        Destroy(this.boxCollider);
    }
}