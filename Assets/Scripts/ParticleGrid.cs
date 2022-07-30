using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class ParticleGrid : MonoBehaviour
{
    [SerializeField] private GameObject player;

    int batchIndexNum = 0;

    public int brushSize = 1;

    public int instances;
    public ComputeShader compute;
    public Mesh objMesh;
    public Material matd;
    public MaterialPropertyBlock mpb;
    public Bounds bd = new Bounds(new Vector2(SCREEN_WIDTH/2, SCREEN_HEIGHT/2) , new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));

    private ComputeBuffer meshPropertiesBuffer;
    private ComputeBuffer argsBuffer;

    MeshProperties[] meshProperties = new MeshProperties[100000];

    Particle[,] grid;

    private static int SCREEN_HEIGHT;
    private static int SCREEN_WIDTH;

    //public static int GetHeight
    //{
    //    get
    //    {
    //        return SCREEN_HEIGHT;
    //    }
    //}
    //public static int GetWidth
    //{
    //    get
    //    {
    //        return SCREEN_WIDTH;
    //    }
    //}

    private struct MeshProperties
    {
        public Matrix4x4 mat;
        public Vector4 color;

        public static int Size()
        {
            return
                sizeof(float) * 4 * 4 + // matrix;
                sizeof(float) * 4;      // color;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Camera cam = Camera.main;

        SCREEN_HEIGHT = Convert.ToInt32(cam.orthographicSize) * 2;
        SCREEN_WIDTH = Convert.ToInt32(SCREEN_HEIGHT * cam.aspect);

        grid = new Particle[SCREEN_WIDTH, SCREEN_HEIGHT];

        meshPropertiesBuffer = new ComputeBuffer(instances, MeshProperties.Size());

        Vector2 camCorner = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        cam.transform.position = new Vector3(-0.5f, -0.5f, -10f) - new Vector3(camCorner.x, camCorner.y, 0);

        mpb = new MaterialPropertyBlock();

        uint[] args = new uint[5] { 0, 0, 0, 0, 0 };
        args[0] = (uint)objMesh.GetIndexCount(0);
        args[1] = (uint)instances;
        //args[2] = (uint)objMesh.GetIndexStart(0);
        //args[3] = (uint)objMesh.GetBaseVertex(0);
        argsBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        argsBuffer.SetData(args);

        for (int i = 0; i < SCREEN_HEIGHT; i += SCREEN_HEIGHT - 1)
            for (int j = 0; j < SCREEN_WIDTH; j++)
                PlaceParticle(j, i, "Stone");                       //бедрок в будующем

        for (int i = 0; i < SCREEN_WIDTH; i += SCREEN_WIDTH - 1)
            for (int j = 0; j < SCREEN_HEIGHT; j++)
                PlaceParticle(i, j, "Stone");

    }

    private void AddObj(int x, int y, Particle data)
    {
        Debug.Log(data);

        grid[x, y] = data;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Convert.ToInt32(Math.Round(mousePos.x));
            int y = Convert.ToInt32(Math.Round(mousePos.y));

            for (int i = 0; i < brushSize; i++)
            {
                for (int j = 0; j < brushSize; j++)
                {
                    PlaceParticle(x + i, y + j, "Water");
                }
            }
        }
        else if (Input.GetMouseButton(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Convert.ToInt32(Math.Round(mousePos.x));
            int y = Convert.ToInt32(Math.Round(mousePos.y));

            for (int i = 0; i < brushSize; i++)
            {
                for (int j = 0; j < brushSize; j++)
                {
                    PlaceParticle(x + i, y + j, "Sand");
                }
            }
        }

        RenderGrid();

        EnableHitBoxes();

        Gravity();
    }

    private void RenderGrid()
    {
        meshPropertiesBuffer.Release();
        meshPropertiesBuffer = new ComputeBuffer(instances, MeshProperties.Size());
        meshPropertiesBuffer.SetData(meshProperties);
        matd.SetBuffer("_Properties", meshPropertiesBuffer);

        Graphics.DrawMeshInstancedIndirect(objMesh, 0, matd, bd, argsBuffer);
    }

    void PlaceParticle(int x, int y, string type)          
    {
        if (!(grid[x, y] is object))
        {
            GameObject go = new GameObject();
            switch (type)
            {
                case "Sand":
                    go.AddComponent<SandParticle>();
                    SandParticle sand = go.GetComponent<SandParticle>();
                    sand.AllSet(new Vector2(x, y), new Vector3(1, 1, 1), Quaternion.identity, batchIndexNum);

                    meshProperties[batchIndexNum].mat = sand.matrix;
                    meshProperties[batchIndexNum].color = sand.GetColor;

                    AddObj(x, y, sand);

                    batchIndexNum++;

                    break;
                case "Stone":
                    go.AddComponent<StoneParticle>();
                    StoneParticle stone = go.GetComponent<StoneParticle>();
                    stone.AllSet(new Vector2(x, y), new Vector3(1, 1, 1), Quaternion.identity, batchIndexNum);

                    meshProperties[batchIndexNum].mat = stone.matrix;
                    meshProperties[batchIndexNum].color = stone.GetColor;

                    AddObj(x, y, stone);

                    batchIndexNum++;

                    break;
                case "Water":
                    go.AddComponent<WaterParticle>();
                    WaterParticle water = go.GetComponent<WaterParticle>();
                    water.AllSet(new Vector2(x, y), new Vector3(1, 1, 1), Quaternion.identity, batchIndexNum);

                    meshProperties[batchIndexNum].mat = water.matrix;
                    meshProperties[batchIndexNum].color = water.GetColor;

                    AddObj(x, y, water);

                    batchIndexNum++;

                    break;
                case "Gas":
                    go.AddComponent<GasParticle>();
                    GasParticle gas = go.GetComponent<GasParticle>();
                    gas.AllSet(new Vector2(x, y), new Vector3(1, 1, 1), Quaternion.identity, batchIndexNum);

                    meshProperties[batchIndexNum].mat = gas.matrix;
                    meshProperties[batchIndexNum].color = gas.GetColor;

                    AddObj(x, y, gas);

                    batchIndexNum++;

                    break;
            }
        }
    }

    void EnableHitBoxes()
    {
        for (int i = -10; i < 10; i++)
            for (int j = -10; j < 10; j++)
                if (j + Convert.ToInt32(player.transform.position.y) >= 0 && (grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)] && grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)] is object))
                {
                    grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)].EnableHitBox();
                }

        for (int i = -11; i < 11; i += 21)
            for (int j = -11; j < 11; j++)
                if (j + Convert.ToInt32(player.transform.position.y) >= 0 && (grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)] && grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)] is object))
                {
                    grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)].DisableHitBox();
                }

        for (int i = -11; i < 11; i++)
            for (int j = -11; j < 11; j += 21)
                if (j + Convert.ToInt32(player.transform.position.y) >= 0 && (grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)] && grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)] is object))
                {
                    grid[i + Convert.ToInt32(player.transform.position.x), j + Convert.ToInt32(player.transform.position.y)].DisableHitBox();
                }
    }

    void Gravity()
    {
        Vector2 check;
        foreach (Particle particle in grid)
        {
            switch (particle)
            {
                case MovableSolidParticle _:
                    check = particle.pos;

                    particle.Fall(grid, SCREEN_WIDTH, SCREEN_HEIGHT);

                    if (check != particle.pos)
                        meshProperties[particle.index].mat = particle.matrix;

                    break;
                case LiquidParticle _:
                    check = particle.pos;

                    particle.Fall(grid, SCREEN_WIDTH, SCREEN_HEIGHT);

                    meshProperties[particle.index].mat = particle.matrix;

                    break;
                case GasParticle _:
                    check = particle.pos;

                    particle.Fall(grid, SCREEN_WIDTH, SCREEN_HEIGHT);

                    if (check != particle.pos)
                        meshProperties[particle.index].mat = particle.matrix;
                    break;
                default:
                    
                    break;
            }
        }
    }
}
