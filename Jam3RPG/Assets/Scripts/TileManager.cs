

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperTiled2Unity;


public struct TileInfo{
    public Vector2Int Tile;
    public int Cost;

    public TileInfo(Vector2Int t, int c){
        Tile = t;
        Cost = c;
    }
};

public class TileManager : MonoBehaviour
{
    private List<Vector2Int> reachedTiles; // In tile coordinates
    private List<GameObject> createdTiles; // In tile coordinates
    
    private GameObject tileContainer;
    
    [SerializeField]
    private GameObject highlightTile;

    private Grid tmap;

    [SerializeField]
    private Vector2Int minCoord;
    [SerializeField]
    private Vector2Int maxCoord;
    
    // Start is called before the first frame update
    void Start()
    {
        reachedTiles = new List<Vector2Int>();
        createdTiles = new List<GameObject>();
        tmap = GameObject.Find("Grid").GetComponent<Grid>();

        SuperMap m = FindObjectOfType<SuperMap>();
        minCoord = new Vector2Int(-m.m_Width/2, -m.m_Height/2);
        maxCoord = new Vector2Int(m.m_Width/2 - 1, m.m_Height/2 - 1);
        
        tileContainer = gameObject.transform.Find("RangeContainer").gameObject;
        //CreateRangeTiles(new Vector2Int(0,0), 5);
    }

    public void CreateRangeTiles(Vector2Int tilePosition, int range, Color c){
        // You can do a collision check... for obstacles... teehee
        // https://www.redblobgames.com/pathfinding/a-star/introduction.html i love cse 101...
        Queue<TileInfo> frontier = new Queue<TileInfo>();
        frontier.Enqueue(new TileInfo(tilePosition, 0));

        reachedTiles.Add(tilePosition);


        TileInfo currTile;
        List<TileInfo> neighbors;
        while (frontier.Count > 0){
            currTile = frontier.Dequeue();
            neighbors = GetNeighboringTiles(currTile);
            foreach(TileInfo tileTuple in neighbors){
                if (!reachedTiles.Exists(tile => tile == tileTuple.Tile)  && currTile.Cost + 1 <= range){
                    frontier.Enqueue(tileTuple);
                    reachedTiles.Add(tileTuple.Tile);
                }
            }
        }

        foreach(Vector2Int tilePos in reachedTiles) { // this is in tile coordinates...
            Vector3 position = tmap.GetCellCenterLocal(new Vector3Int(tilePos.x, tilePos.y, 0));
            position.z = 0;
            GameObject tile = Instantiate(highlightTile, position, Quaternion.identity, tileContainer.transform);
            tile.GetComponent<SpriteRenderer>().color = c;
            createdTiles.Add(tile);
        }
    }

    private List<TileInfo> GetNeighboringTiles(TileInfo tileTuple){
        List<TileInfo> ret = new List<TileInfo>();
        if (tileTuple.Tile.x + 1 <= maxCoord.x)
            ret.Add(new TileInfo(new Vector2Int(tileTuple.Tile.x + 1, tileTuple.Tile.y     ), tileTuple.Cost + 1));
        if (tileTuple.Tile.x - 1 >= minCoord.x)
            ret.Add(new TileInfo(new Vector2Int(tileTuple.Tile.x - 1, tileTuple.Tile.y     ), tileTuple.Cost + 1));
        if (tileTuple.Tile.y + 1 <= maxCoord.y)
            ret.Add(new TileInfo(new Vector2Int(tileTuple.Tile.x,     tileTuple.Tile.y + 1 ), tileTuple.Cost + 1));
        if (tileTuple.Tile.y - 1 >= minCoord.y)
            ret.Add(new TileInfo(new Vector2Int(tileTuple.Tile.x,     tileTuple.Tile.y - 1 ), tileTuple.Cost + 1));
        return ret;
    }

    public void RemoveRangeTiles(){
        foreach(GameObject tile in createdTiles){
            Object.Destroy(tile);
        }
        createdTiles.Clear();
        reachedTiles.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
