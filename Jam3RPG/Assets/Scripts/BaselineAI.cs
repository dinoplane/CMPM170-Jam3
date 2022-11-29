using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



public class BaselineAI : MonoBehaviour
{
    [SerializeField]
    private PhaseManager phaseManager;

    private TileManager tileManager;

    private Vector2Int minCoord;
    private Vector2Int maxCoord;

    public GameObject highlightTile;

    private Grid tmap;

    void Awake(){

    }

    // Start is called before the first frame update
    void Start()
    {


    }

    public void EnemyTurnStart(){
        if (tileManager == null){
            tileManager = GetComponent<TileManager>();
            // Get the phase manager 
            minCoord = tileManager.minCoord;
            maxCoord = tileManager.maxCoord;
            tmap = tileManager.tmap;
        }
        // testing getting the units...
        Debug.Log("ENEMY TURN START");

        int a = 0;
        foreach(AttackingClass unit in phaseManager.aiUnits){
            if (a == 1){
                // I really dont want to use casts but ok.
                List<UnitBaseClass> hitList = FindAllInRange((AttackingClass) unit);
                UnitBaseClass target = null;
                List<int> damageList = new List<int>();
                if (hitList.Count > 0) { // targets in the range...
                    foreach(UnitBaseClass u in hitList){
                        if (target != null)
                            break;
                        Debug.Log(u.name);
                        // target Damage
                        int targetDamage = unit.attackDamage - u.armorCurrent;

                        // final health
                        int finalHealth = u.healthCurrent - targetDamage;
                        // Check killable (Edge case: which one do i choose if multiple?)
                        if (finalHealth <= 0){ // Killed
                            target = u;
                        } else { // Check most damage (Edge Case: which to choose if multiple?)
                            damageList.Add(targetDamage);
                        }
                    }

                    if (target == null){ // No target yet...
                        int maxIndex = damageList.IndexOf(damageList.Max());
                        target = hitList[maxIndex]; // I think it is implied that the cult leader will be prioritized anyways...
                    }
                } else { // targets not in range
                    List<int> distsToUnits = new List<int>();
                    foreach(UnitBaseClass p in phaseManager.playerUnits){
                        distsToUnits.Add(unit.GetDistanceFromTile(p.tilePosition));
                    }
                }

                Debug.Log("Target: " + target.name);
                // Find space to move to 
                FindMinMaxCostTile(unit, target.tilePosition);

            }
            a += 1;
        }
    }

    public List<UnitBaseClass> FindAllInRange(AttackingClass unit){
        Debug.Log("Targets of: " + unit.name);
        List<UnitBaseClass> pTargets = new List<UnitBaseClass>();

        foreach(UnitBaseClass target in phaseManager.playerUnits){
            if (unit.CheckTileInMoveAttackRange(target.tilePosition) > 0){
                pTargets.Add(target);
            }
        }

        // Queue<TileInfo> frontier = new Queue<TileInfo>();
        // frontier.Enqueue(new TileInfo(unit.tilePosition, 0));

        // List<Vector2Int> reachedTiles = new List<Vector2Int>();
        // reachedTiles.Add(unit.tilePosition);

       

        // int range = unit.attackRange + unit.moveRange;
        
        // Debug.Log(unit.name + " Range: " + range.ToString());
        
        

        // TileInfo currTile;
        // List<TileInfo> neighbors;
        // UnitBaseClass playerUnit;
        // Debug.Log(tileManager);
        // while (frontier.Count > 0){
        //     currTile = frontier.Dequeue();
        //     neighbors = GetNeighboringTiles(currTile);
        //     foreach(TileInfo tileInfo in neighbors){
        //         if (!reachedTiles.Exists(tile => tile == tileInfo.Tile)  && currTile.Cost + 1 <= range){
        //             frontier.Enqueue(tileInfo);
        //             reachedTiles.Add(tileInfo.Tile);

        //             // Check if the tile has a playerUnit
        //             playerUnit = CheckTileIsEmpty(tileInfo);
        //             if (playerUnit != null && playerUnit.turnOver){
        //                 playerUnits.Add(playerUnit);
        //             }
        //         }
        //     }
        // } 

        // Debug.Log(reachedTiles.Count);


        // }

        return pTargets;
    } 

    private List<TileInfo> GetNeighboringTiles(TileInfo tileInfo){
        List<TileInfo> ret = new List<TileInfo>();

        // Fetch all physically available tiles
        if (tileInfo.Tile.x + 1 <= maxCoord.x)
            ret.Add(new TileInfo(new Vector2Int(tileInfo.Tile.x + 1, tileInfo.Tile.y     ), tileInfo.Cost + 1));
        if (tileInfo.Tile.x - 1 >= minCoord.x)
            ret.Add(new TileInfo(new Vector2Int(tileInfo.Tile.x - 1, tileInfo.Tile.y     ), tileInfo.Cost + 1));
        if (tileInfo.Tile.y + 1 <= maxCoord.y)
            ret.Add(new TileInfo(new Vector2Int(tileInfo.Tile.x,     tileInfo.Tile.y + 1 ), tileInfo.Cost + 1));
        if (tileInfo.Tile.y - 1 >= minCoord.y)
            ret.Add(new TileInfo(new Vector2Int(tileInfo.Tile.x,     tileInfo.Tile.y - 1 ), tileInfo.Cost + 1));
        
        return ret;
    }

    public UnitBaseClass CheckTileIsOccupied(TileInfo tileInfo){
        Vector2Int tilePos = tileInfo.Tile;
        Vector3 tileLocation = tmap.GetCellCenterLocal(new Vector3Int(tilePos.x, tilePos.y, 0));

        RaycastHit hit;
        bool hasSelectedUnit = Physics.Raycast(Camera.main.transform.position,tileLocation - Camera.main.transform.position, out hit);
        if (hasSelectedUnit){
            return hit.collider.gameObject.GetComponent<UnitBaseClass>();
        }

        return null;
    }

    public void FindTarget(){

    }

    public void FindMinMaxCostTile(AttackingClass unit, Vector2Int dst){
        int currCost = -1;
        currCost = unit.GetDistanceFromTile(dst);

        Queue<TileInfo> frontier = new Queue<TileInfo>();
        frontier.Enqueue(new TileInfo(dst, currCost));

        List<Vector2Int> reachedTiles = new List<Vector2Int>();
        reachedTiles.Add(dst);

        int range = unit.attackRange + unit.moveRange;
        
        Debug.Log(unit.name + " Range: " + range.ToString());
    
        TileInfo currTile;
        List<TileInfo> neighbors;
        List<Vector2Int> availSpaces = new List<Vector2Int>();

        TileInfo currMax;

        // We assume the destination isn't in move range
        while (frontier.Count > 0){
            currTile = frontier.Dequeue();
            neighbors = GetNeighboringTiles(currTile);
            foreach(TileInfo tileInfo in neighbors){
                if (!reachedTiles.Exists(tile => tile == tileInfo.Tile) && unit.CheckTileInMoveAttackRange(tileInfo.Tile) > 0){ // We search onlly if we need to...
                    if (unit.CheckTileInMoveRange(tileInfo.Tile) > 0 && CheckTileIsOccupied(tileInfo) == null){ // If we can move to this tile...
                        // Check if it is the max distance away from tile?
                        //if (unit.GetDistanceFromTile(tileInfo.Tile) == range){
                            availSpaces.Add(tileInfo.Tile);
                        //}
                    } else if (unit.CheckTileInMoveAttackRange(tileInfo.Tile) > 0 &&
                            unit.CheckTileInMoveRange(tileInfo.Tile) < 0 && 
                            GetDistanceBetweenTiles(tileInfo.Tile, dst) <= unit.attackRange){
                        frontier.Enqueue(tileInfo);
                        reachedTiles.Add(tileInfo.Tile);
                    }
                } 
            }
        } 

        foreach(Vector2Int tilePos in reachedTiles) { // this is in tile coordinates...
            Vector3 position = tmap.GetCellCenterLocal(new Vector3Int(tilePos.x, tilePos.y, 0));
            position.z = 0;
            GameObject tile = Instantiate(highlightTile, position, Quaternion.identity);
            tile.GetComponent<SpriteRenderer>().color = Color.red;
        }

        foreach(Vector2Int tilePos in availSpaces) { // this is in tile coordinates...
            Vector3 position = tmap.GetCellCenterLocal(new Vector3Int(tilePos.x, tilePos.y, 0));
            position.z = 0;
            GameObject tile = Instantiate(highlightTile, position, Quaternion.identity);
            tile.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }

    public int GetDistanceBetweenTiles(Vector2Int a, Vector2Int b){
        Vector2Int result = a - b;
        return Mathf.Abs(result.x) + Mathf.Abs(result.y); // cost
    }

    public void CommandUnit(){

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
