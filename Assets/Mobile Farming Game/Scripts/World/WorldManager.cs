using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using System;

public class WorldManager : MonoBehaviour
{
    private enum chunkShape {None,TopRight,BotRight,BotLeft,TopLeft,Top,Right,Bottom,Left,Four}
    [Header("Elements")]
    [SerializeField] private Transform world;

    [Header("Settings")]
    private WorldData worldData;
    private string dataPath;
    private bool shouldSave;
    private Chunk[,] grid;
    [SerializeField] private int gridSize;
    [SerializeField] private int gridScale;

    [Header("Chunk Mesh")]
    [SerializeField] private Mesh[] chunkShapes;
    private void Awake()
    {
        Chunk.onChunkUnlocked += chunkUnlockedCallback;
        Chunk.onPriceChange += priceChangeCallback;
    }
    private void Start()
    {
        dataPath = Application.persistentDataPath + "/world.data";
        loadWorld();
        Initialize();
        InvokeRepeating("trySaveGame", 1, 1);
    }
    private void OnDestroy()
    {
        Chunk.onChunkUnlocked -= chunkUnlockedCallback;
        Chunk.onPriceChange -= priceChangeCallback;
    }
    private void chunkUnlockedCallback()
    {
        updateChunkWalls();
        updateGridRenderers();
        saveWorld();
    }
    private void trySaveGame()
    {
        if(shouldSave)
        {
            saveWorld();
            shouldSave = false;
        }
    }
    private void priceChangeCallback()
    {
        shouldSave = true;
    }
    private void Initialize()
    {
        for(int i=0;i<world.childCount;i++)
            world.GetChild(i).GetComponent<Chunk>().Initialze(worldData.chunkPrices[i]);

        InitializeGrid();
        updateChunkWalls();
        updateGridRenderers();
    }
    private void InitializeGrid()
    {
        grid = new Chunk[gridSize,gridSize];
        for(int i=0;i<world.childCount;i++)
        {
            Chunk chunk = world.GetChild(i).GetComponent<Chunk>();
            Vector2Int chunkGridPosition = new Vector2Int
                ((int)chunk.transform.position.x / gridScale, (int)chunk.transform.position.z / gridScale);
            chunkGridPosition += new Vector2Int(gridSize/2, gridSize/2);
            grid[chunkGridPosition.x, chunkGridPosition.y] = chunk;
        }
    }
    private void updateChunkWalls()
    {
        for (int x = 0;x < grid.GetLength(0);x++)
        {
            for (int y = 0;y < grid.GetLength(1);y++)                                                                                                  
            {
                Chunk chunk = grid[x,y];
                if (chunk == null)
                    continue;

                Chunk frontChunk = isValidGridPosition(x, y + 1) ? grid[x, y + 1] : null;
                Chunk rightChunk = isValidGridPosition(x + 1, y) ? grid[x + 1, y] : null;
                Chunk backChunk = isValidGridPosition(x, y - 1) ? grid[x, y - 1] : null;
                Chunk leftChunk = isValidGridPosition(x - 1, y) ? grid[x - 1, y] : null;

                int configuration = 0;
                if (frontChunk != null && frontChunk.isUnlocked() == true)
                    configuration += 1;

                if (rightChunk != null && rightChunk.isUnlocked() == true)
                    configuration += 2;

                if (backChunk != null && backChunk.isUnlocked() == true)
                    configuration += 4;

                if (leftChunk != null && leftChunk.isUnlocked() == true)
                    configuration += 8;

                chunk.updateWalls(configuration);

                setChunkRenderer(chunk,configuration);
            }
        }
    }

    private void setChunkRenderer(Chunk chunk, int configuration)
    {
        switch(configuration)
        {
            case 0: chunk.setRenderer(chunkShapes[(int)chunkShape.Four]);
                break;
            case 1: chunk.setRenderer(chunkShapes[(int)chunkShape.Bottom]);
                break;
            case 2:
                chunk.setRenderer(chunkShapes[(int)chunkShape.Left]);
                break;
            case 3:
                chunk.setRenderer(chunkShapes[(int)chunkShape.BotLeft]);
                break;
            case 4:
                chunk.setRenderer(chunkShapes[(int)chunkShape.Top]);
                break;
            case 5:
                chunk.setRenderer(chunkShapes[(int)chunkShape.None]);
                break;
            case 6:
                chunk.setRenderer(chunkShapes[(int)chunkShape.TopLeft]);
                break;
            case 7:
                chunk.setRenderer(chunkShapes[(int)chunkShape.None]);
                break;
            case 8:
                chunk.setRenderer(chunkShapes[(int)chunkShape.Right]);
                break;
            case 9:
                chunk.setRenderer(chunkShapes[(int)chunkShape.BotRight]);
                break;
            case 10:
                chunk.setRenderer(chunkShapes[(int)chunkShape.None]);
                break;
            case 11:
                chunk.setRenderer(chunkShapes[(int)chunkShape.None]);
                break;
            case 12:
                chunk.setRenderer(chunkShapes[(int)chunkShape.TopRight]);
                break;
            case 13:
                chunk.setRenderer(chunkShapes[(int)chunkShape.None]);
                break;
            case 14:
                chunk.setRenderer(chunkShapes[(int)chunkShape.None]);
                break;
            case 15:
                chunk.setRenderer(chunkShapes[(int)chunkShape.None]);
                break;
        }    
        
    }

    private void updateGridRenderers()
    {
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                Chunk chunk = grid[x, y];
                if (chunk == null)
                    continue;

                if (chunk.isUnlocked() == true)
                    continue;

                Chunk frontChunk = isValidGridPosition(x,y + 1) ? grid[x, y+ 1] : null;
                Chunk rightChunk = isValidGridPosition(x + 1,y) ? grid[x + 1, y] : null;
                Chunk backChunk = isValidGridPosition(x,y - 1) ? grid[x, y - 1] : null;
                Chunk leftChunk = isValidGridPosition(x - 1,y ) ? grid[x - 1, y] : null;
                if (frontChunk != null && frontChunk.isUnlocked() == true)
                    chunk.displayLockedElements();
                else if (rightChunk != null && rightChunk.isUnlocked() == true)
                    chunk.displayLockedElements();
                else if (backChunk != null && backChunk.isUnlocked() == true)
                    chunk.displayLockedElements();
                else if (leftChunk != null && leftChunk.isUnlocked() == true) 
                    chunk.displayLockedElements();
            }
        }
    }
    private bool isValidGridPosition(int x,int y)
    {
        if(x < 0 || y < 0 || x >= gridSize || y >= gridSize)
            return false;
        return true;
    }
    private void loadWorld()
    {
        string data = "";
        if (!File.Exists(dataPath))
        {
            FileStream fs = new FileStream(dataPath, FileMode.Create);
            worldData = new WorldData();
            for (int i = 0; i < world.childCount; i++)
                worldData.chunkPrices.Add(world.GetChild(i).GetComponent<Chunk>().getInitialPrice());
            string worldDataString = JsonUtility.ToJson(worldData, true);
            byte[] worldDataByte = Encoding.UTF8.GetBytes(worldDataString);
            fs.Write(worldDataByte);
            fs.Close();
        }
        else
        {
            data = File.ReadAllText(dataPath);
            worldData = JsonUtility.FromJson<WorldData>(data);

            if (worldData.chunkPrices.Count < world.childCount)
                updateData();
        }
    }
    private void updateData()
    {
        int missingData = world.childCount - worldData.chunkPrices.Count;
        for(int i = 0;i<missingData;i++)
        {
            int index = world.childCount - missingData + i;
            int missingIndex = world.GetChild(index).GetComponent<Chunk>().getInitialPrice();
            worldData.chunkPrices.Add(missingIndex);
        }
    }
    private void saveWorld()
    {
        if (worldData.chunkPrices.Count != world.childCount)
            worldData = new WorldData();

        for(int i=0;i<world.childCount;i++)
        {
            int chunkCurrentPrice = world.GetChild(i).GetComponent<Chunk>().getCurrentPrice();
            if (worldData.chunkPrices.Count > i)
                worldData.chunkPrices[i] = chunkCurrentPrice;
            else worldData.chunkPrices.Add(chunkCurrentPrice);
        }

        string data = JsonUtility.ToJson(worldData, true);
        File.WriteAllText(dataPath, data);
    }
}
