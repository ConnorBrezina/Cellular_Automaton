using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour {


    private int[,] grid;

    // Recommended Numbers (change Fill% to see how it changes)
    public int columns = 60;
    public int rows = 20;
    [Range(0, 100)]
    public int randomFillPercent = 26;


    //Random map generation or specific (seed)
    public bool isRandom = true;
    public string seed;


    void Start () {
		Generate();
	}
	

	void Update () {
		
	}

    //Basic Init and call function
    void Generate()
    {
        grid = new int[rows, columns];
        Randomizer();

        for (int i = 0; i<5; i++)
        {
            SmoothMap();
        }
    }

    //Assigns a value of 1 or 0 for if its a wall or not... also the outside edges must be walls
    void Randomizer()
    {
        if (isRandom)
        {
            seed = System.DateTime.Now.Ticks.ToString();
        }

        System.Random psRandom = new System.Random(seed.GetHashCode());

        for (int x = 0; x < rows; x++)
        {
            for (int y = 0; y < columns; y++)
            {
                if (x == 0 || x == rows - 1 || y == 0 || y == columns - 1)
                {
                    grid[x, y] = 1;
                }
                else
                {
                    grid[x, y] = psRandom.Next(0, 100) < randomFillPercent ? 1 : 0;
                }
            }
        }
    }

    //Function to make sure there aren't so many single walls and caverns, x < around a square it becomes a wall, x > becomes a cavern.
    void SmoothMap()
    {
        for (int x = 0; x < rows; x ++)
        {
            for (int y = 0; y < columns; y++)
            {
                int neighbourWallTiles = GetWallCount(x, y);

                if (neighbourWallTiles > 4)
                    grid[x, y] = 1;
                else if (neighbourWallTiles < 4)
                    grid[x, y] = 0;
            }  
        }
    }

    //Getting surrounding wall count for SmoothMap()
    int GetWallCount(int gridX, int gridY)
    {
        int wallCount = 0;
        for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
        {
            for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < rows && neighbourY >= 0 && neighbourY < columns)
                {
                    if (neighbourX != gridY || neighbourY != gridY)
                    {
                        wallCount += grid[neighbourX, neighbourY];
                    }
                }
                else
                {
                    wallCount++;
                }
            }
        }
        return wallCount;
    }

    //Draw our grid
    private void OnDrawGizmos()
    {
        if (grid != null)
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    Gizmos.color = (grid[x,y] == 1)?Color.black:Color.white;
                    Vector3 pos = new Vector3(-rows / 2 + x + .5f, 0, -columns / 2 + y + .5f);
                    Gizmos.DrawCube(pos, Vector3.one);
                }
            }
        }
    }


}
