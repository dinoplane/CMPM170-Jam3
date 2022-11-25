using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private List<Vector2Int> reachedTiles; // In tile coordinates
    
    // Start is called before the first frame update
    void Start()
    {
        reachedTiles = new List<Vector2Int>();
    }

    public void CreateRangeTiles(Vector2Int tilePosition, int range){
        // You can do a collision check... for obstacles... teehee
        // https://www.redblobgames.com/pathfinding/a-star/introduction.html i love cse 101...
        Queue<(Vector2Int Tile, int Cost)> frontier = new Queue<(Vector2Int Tile, int Cost)>();
        frontier.Enqueue((tilePosition, 0));

        reachedTiles.Add(tilePosition);


        (Vector2Int Tile, int Cost) currTile;
        List<(Vector2Int Tile, int Cost)> neighbors;
        while (frontier.Count > 0){
            currTile = frontier.Dequeue();
            neighbors = GetNeighboringTiles(currTile);
            foreach((Vector2Int Tile, int Cost) tileTuple in neighbors){
                if (!reachedTiles.Exists(tile => tile == tileTuple.Tile)){
                    frontier.Enqueue(tileTuple);
                    reachedTiles.Add(tileTuple.Tile);
                }
            }
        }


    }

    private List<(Vector2Int Tile, int Cost)> GetNeighboringTiles((Vector2Int Tile, int Cost) tileTuple){
        List<(Vector2Int Tile, int Cost)> ret = new List<(Vector2Int Tile, int Cost)>();
        ret.Add((new Vector2Int(tileTuple.Tile.x + 1, tileTuple.Tile.y     ), tileTuple.Cost + 1   ));
        ret.Add((new Vector2Int(tileTuple.Tile.x - 1, tileTuple.Tile.y     ), tileTuple.Cost + 1   ));
        ret.Add((new Vector2Int(tileTuple.Tile.x,     tileTuple.Tile.y + 1 ), tileTuple.Cost + 1));
        ret.Add((new Vector2Int(tileTuple.Tile.x,     tileTuple.Tile.y - 1 ), tileTuple.Cost + 1));
        return ret;
    }

    public void RemoveRangeTiles(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
