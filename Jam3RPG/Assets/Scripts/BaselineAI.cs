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
    public GameObject aiCursor;

    private Grid tmap;

    void Awake(){

    }

    // Start is called before the first frame update
    void Start()
    {
        aiCursor =  GameObject.Find("AICursor");
        aiCursor.SetActive(false);
    }

    /*Coroutine is started when the AI phase starts*/
    public IEnumerator EnemyTurnStart(){
        if (tileManager == null){
            tileManager = GetComponent<TileManager>();
            // Get the phase manager 
            minCoord = tileManager.minCoord;
            maxCoord = tileManager.maxCoord;
            tmap = tileManager.tmap;

            aiCursor.SetActive(true);
        }
        // testing getting the units...
        Debug.Log("ENEMY TURN START");

        int a = 0;
        foreach(AttackingClass unit in phaseManager.aiUnits){ // For every Ai unit
            if (a < 100){
                checkAggro(unit);
                if(unit.isAggro == true){
                    UnitBaseClass target = null; 
                    Vector2Int destTile = minCoord - Vector2Int.one;
                    List<UnitBaseClass> blackList = new List<UnitBaseClass>();

                    while (!checkValidTile(destTile)){
                        Debug.Log("hello");
                        target = FindTargetInRange(unit, blackList); /* Find highest priority target not on the blacklist*/
                        if (target == null)
                            break;

                        // Find a tile such that you can attack but also be furthest away from
                        /*But if that tile is occupied / not valid, the target is added to the blacklist
                         Will always try to return spaces that are valid however - checking for best first and then checking
                        for 2nd best if 1st is not valid*/
                        destTile = FindMinMaxCostTile(unit, target.tilePosition); 
                        if (!checkValidTile(destTile))
                            blackList.Add(target);
                            /*If the best target cannot be reached, they are added to blacklist and we try the next best target instead*/
                    }

                    if (target == null){ // if no targets in range 
                        target = unit;
                        UnitBaseClass closestTarget = FindClosestUnit(unit);
                        destTile = FindMovementTile(unit, closestTarget);

                        Debug.Log("Closest Unit: "+ closestTarget.tilePosition);
                    }
            
                // lemme get a cursor...
                /*So getting a target is all done in one tick*/
                    yield return StartCoroutine(StartUnitCommandSequence(unit, target, destTile));
                }
                unit.FinishTurn();
            }
            a += 1;
        }
    }

    /*Basically implements the squad behavior*/
    public void checkAggro(AttackingClass unit){
        foreach(UnitBaseClass target in phaseManager.playerUnits){
            if (unit.CheckTileInRange(target.tilePosition, unit.aggroRange) >= 0){
                unit.isAggro = true;
            }
        }
    }

    public UnitBaseClass FindClosestUnit(AttackingClass unit){
        UnitBaseClass closestUnit = null;
        int closest = 1000;

        foreach(UnitBaseClass target in phaseManager.playerUnits){
            int currCloseness = unit.GetDistanceFromTile(target.tilePosition);
            if (currCloseness < closest){
                closest = currCloseness;
                closestUnit = target;
            }
        }
        return closestUnit;
    }

    public List<UnitBaseClass> FindAllInRange(AttackingClass unit){
        Debug.Log("Targets of: " + unit.name);
        List<UnitBaseClass> pTargets = new List<UnitBaseClass>();

        foreach(UnitBaseClass target in phaseManager.playerUnits){
            if (unit.CheckTileInMoveAttackRange(target.tilePosition) >= 0){
                
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

    public UnitBaseClass FindTargetInRange(AttackingClass unit, List<UnitBaseClass> blackList=null){ // finds the target of the enemy unit
        List<UnitBaseClass> hitList = FindAllInRange(unit);    
        if (blackList != null){
            hitList.RemoveAll(hit => blackList.Exists(b => hit == b));
        }

        UnitBaseClass target = null;

        List<int> healthList = new List<int>();
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
                    healthList.Add(finalHealth);
                } else { // Check most damage (Edge Case: which to choose if multiple?)
                    damageList.Add(targetDamage);
                }
            }
            // Remove all occurrances in the blacklist...


            if (healthList.Count > 0){
                int maxIndex = healthList.IndexOf(healthList.Max());
                target = hitList[maxIndex]; // I think it is implied that the cult leader will be prioritized anyways...
            }
            if (damageList.Count > 0){ // No target yet...
                int maxIndex = damageList.IndexOf(damageList.Max());
                target = hitList[maxIndex]; // I think it is implied that the cult leader will be prioritized anyways...
            }
        }
        return target;
    }

    // Jank
    // Start bfs from closest unit
    // Spread to neighbors but only think about adding a place to move to if tile inside of unit move range
    public Vector2Int FindMovementTile(AttackingClass unit, UnitBaseClass closestTarget){
        Vector2Int dst = closestTarget.tilePosition;

        Queue<TileInfo> frontier = new Queue<TileInfo>();
        List<Vector2Int> reachedTiles = new List<Vector2Int>();    
        TileInfo currTile;
        List<TileInfo> neighbors;
        List<Vector2Int> availSpaces = new List<Vector2Int>();
        bool notFound = true;

        frontier.Enqueue(new TileInfo(dst, 0));
        reachedTiles.Add(dst);

        int bestCost = 1000;

        while(notFound){
            //Get Neighbors
            if(frontier.Count == 0){
                break;
            }
            currTile = frontier.Dequeue();
            neighbors = GetNeighboringTiles(currTile);

            foreach(TileInfo tileInfo in neighbors){
                // Debug.Log("TILE INFO:  " + tileInfo.Tile);

                if (tileInfo.Tile == unit.tilePosition){  // Located
                    notFound = false;
                    break;
                }

                if (!reachedTiles.Exists(tile => tile == tileInfo.Tile)){
                    if(unit.CheckTileInMoveRange(tileInfo.Tile) >= 0){
                        if(GetDistanceBetweenTiles(tileInfo.Tile, dst) <= bestCost && (CheckTileIsOccupied(tileInfo) == null || tileInfo.Tile == unit.tilePosition)){
                            availSpaces.Add(tileInfo.Tile); // a candidate is found
                            if (tileInfo.Cost < bestCost){ // update the current best if found
                                bestCost = tileInfo.Cost; 
                                // remove all tiles with a greater distance from the target
                                availSpaces.RemoveAll(tile => GetDistanceBetweenTiles(tile, dst) > bestCost);
                                Debug.Log("TILE INFO:  " + tileInfo.Tile);
                            }
                        }
                    }
                    else{
                        frontier.Enqueue(tileInfo);
                        reachedTiles.Add(tileInfo.Tile);
                    }
                }
            }

        }

        // check if availspaces is not full..
        if (availSpaces.Count == 0){
            return maxCoord + new Vector2Int(1, 1);
        } else {
            // Return a random tile location... // YOu can make this smarter if you have time...
            int index = Random.Range(0, availSpaces.Count);
            return availSpaces[index];
        }
    }

    public Vector2Int FindMinMaxCostTile(AttackingClass unit, Vector2Int dst){ // assumes that the target is within movement + attack range
        

        Queue<TileInfo> frontier = new Queue<TileInfo>();
        List<Vector2Int> reachedTiles = new List<Vector2Int>();    
        TileInfo currTile;
        List<TileInfo> neighbors;
        List<Vector2Int> availSpaces = new List<Vector2Int>(); // potential options...

        // When the enemy unit is in the movement + attack (m+a) range, but not in movement range...
        if (unit.CheckTileInMoveRange(dst) < 0 && unit.CheckTileInMoveAttackRange(dst) >= 0){ 

            // We start a BFS from the target
            frontier.Enqueue(new TileInfo(dst, 0));
            reachedTiles.Add(dst);

            while (frontier.Count > 0){ // While we have tiles to explore
                currTile = frontier.Dequeue();
                neighbors = GetNeighboringTiles(currTile); // Get the surrounding tiles...

                foreach(TileInfo tileInfo in neighbors){

                    if (tileInfo.Tile == unit.tilePosition){
                        Debug.Log(!reachedTiles.Exists(tile => tile == tileInfo.Tile));
                    }
                    if (!reachedTiles.Exists(tile => tile == tileInfo.Tile) && // If we havent visited this tile
                        unit.CheckTileInMoveAttackRange(tileInfo.Tile) >= 0){ // But they are in our m+a range

                        if (unit.CheckTileInMoveRange(tileInfo.Tile) >= 0 && // if they are in movement range
                            tileInfo.Cost == unit.attackRange && // and we can attack the target from that tile...

                            (CheckTileIsOccupied(tileInfo) == null || // And that tile is empty
                            tileInfo.Tile == unit.tilePosition)){ // or is just the enemy unit's tile
                                availSpaces.Add(tileInfo.Tile); // we found a potential candidate
                            
                        } else if (unit.CheckTileInMoveAttackRange(tileInfo.Tile) >= 0 && // if the tile is in m+a range 
                                tileInfo.Cost <= unit.attackRange){ // and we can attack from it (is this needed?)
                            frontier.Enqueue(tileInfo); // explore its surroundings
                            reachedTiles.Add(tileInfo.Tile); // we reached this tile...
                        }
                        
                    } // dont iterate on tiles that are not in our m+a range, in the movement range, or reached before...
                }
            }
        } else if (unit.CheckTileInMoveRange(dst) >= 0){ // Assume dst is in move range
            Debug.Log("Hole"); 

            // Start looking from the target's tile
            frontier.Enqueue(new TileInfo(dst, 0));
            reachedTiles.Add(dst);

            // keep track of the maximum distance WE COULD BE from the destination
            int bestCost = 0; //Mathf.Min(unit.attackRange, GetDistanceBetweenTiles(unit.tilePosition, dst));

            // While there are more tiles to discover
            while (frontier.Count > 0){
                // Get neighbors
                Debug.Log(bestCost);
                currTile = frontier.Dequeue();
                neighbors = GetNeighboringTiles(currTile);
                
                foreach(TileInfo tileInfo in neighbors){ // We look in every neighbor

                    if (!reachedTiles.Exists(tile => tile == tileInfo.Tile) && 
                            unit.CheckTileInMoveRange(tileInfo.Tile) >= 0){ // We only search for tiles we could move to.

                                                                                                            // If we are at a position where the target unit is ...
                        if (((GetDistanceBetweenTiles(tileInfo.Tile, dst) == unit.attackRange) ||            // on the edge of attack range (basically makes sure bestCost > attackRange is impossible)
                                                                                         // OR
                            ((IsEdgeTile(tileInfo.Tile) || unit.CheckTileInMoveRange(tileInfo.Tile) == unit.moveRange) && // we are on the end of the frontier...)
                             GetDistanceBetweenTiles(tileInfo.Tile, dst) <= unit.attackRange)) && // we can still attack from it
                            
                            GetDistanceBetweenTiles(tileInfo.Tile, dst) >= bestCost && // and we either get further away or maintain our distance from the target...
                            
                            (CheckTileIsOccupied(tileInfo) == null || tileInfo.Tile == unit.tilePosition)){// and we can move to this tile physically...
                           
                                availSpaces.Add(tileInfo.Tile); // a candidate is found
                                if (tileInfo.Cost > bestCost){ // update the current best if found
                                    bestCost = tileInfo.Cost; 
                                    // remove all tiles with a shorter distance from the target
                                    availSpaces.RemoveAll(tile => GetDistanceBetweenTiles(tile, dst) < bestCost);
                                }
                        }
                        
                        if (unit.CheckTileInMoveRange(tileInfo.Tile) >= 0 && // Basically iterating throughout all possible move positions that can attack enemy 
                                GetDistanceBetweenTiles(tileInfo.Tile, dst) <= unit.attackRange){
                            frontier.Enqueue(tileInfo);
                            reachedTiles.Add(tileInfo.Tile);
                        }
                    } 
                }
            }

            // foreach(Vector2Int tilePos in reachedTiles) { // this is in tile coordinates...
            //     Vector3 position = tmap.GetCellCenterLocal(new Vector3Int(tilePos.x, tilePos.y, 0));
            //     position.z = 0;
            //     GameObject tile = Instantiate(highlightTile, position, Quaternion.identity);
            //     tile.GetComponent<SpriteRenderer>().color = Color.red;
            // }
        } 
        
        // foreach(Vector2Int tilePos in availSpaces) { // this is in tile coordinates...
        //     Vector3 position = tmap.GetCellCenterLocal(new Vector3Int(tilePos.x, tilePos.y, 0));
        //     position.z = 0;
        //     GameObject tile = Instantiate(highlightTile, position, Quaternion.identity);
        //     tile.GetComponent<SpriteRenderer>().color = Color.blue;
        // }

        
        // check if availspaces is not full..
        if (availSpaces.Count == 0){
            return maxCoord + new Vector2Int(1, 1);
        } else {
            // Return a random tile location... // YOu can make this smarter if you have time...
            int index = Random.Range(0, availSpaces.Count);
            return availSpaces[index];
        }
    }
    

            /* not in range... find closests? but this is

                // distances
                List<int > distances = new List<int>();
                foreach(UnitBaseClass target in phaseManager.playerUnits){
                    distances.Add(unit.GetDistanceFromTile(target.tilePosition));
                }
                LETS DO A*!!!! 
                Anyway, https://www.redblobgames.com/pathfinding/a-star/introduction.html again.


                PriorityQueue<TileInfo, int> frontier = new PriorityQueue<TileInfo, int>();
                Dictionary<TileInfo, TileInfo> cameFrom = new Dictionary<TileInfo, TileInfo>();    
                Dictionary<TileInfo, int> costSoFar = new Dictionary<TileInfo, int>();    
                
                List<TileInfo> neighbors;

                List<Vector2Int> availSpaces = new List<Vector2Int>(); // potential options...


                frontier.Add((unit.tilePosition, 0), 0);
                came_from = Dictionary<Vector2Int>();
                cost_so_far = 
                came_from[start] = D>
                cost_so_far[start] = 0

                while not frontier.empty():
                current = frontier.get()

                if current == goal:
                    break
                
                for next in graph.neighbors(current):
                    new_cost = cost_so_far[current] + graph.cost(current, next)
                    if next not in cost_so_far or new_cost < cost_so_far[next]:
                        cost_so_far[next] = new_cost
                        priority = new_cost + heuristic(goal, next)
                        frontier.put(next, priority)
                        came_from[next] = current
            */

    /*Checks if a tile is inside the map?*/
    public bool checkValidTile(Vector2Int tilePos){
        return (tilePos.x <= maxCoord.x &&
            tilePos.x >= minCoord.x &&
            tilePos.y <= maxCoord.y &&
            tilePos.y >= minCoord.y);
    }


    public int heuristic(Vector2Int a, Vector2Int b){
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }


    public bool IsEdgeTile(Vector2Int tilePos){ // are we on the edge of the board?
        return (tilePos.x + 1 > maxCoord.x ||
            tilePos.x - 1 < minCoord.x ||
            tilePos.y + 1 > maxCoord.y ||
            tilePos.y - 1 < minCoord.y);
    }

    public int GetDistanceBetweenTiles(Vector2Int a, Vector2Int b){ 
        Vector2Int result = a - b;
        return Mathf.Abs(result.x) + Mathf.Abs(result.y); // cost
    }

    public IEnumerator StartUnitCommandSequence(AttackingClass unit, UnitBaseClass target, Vector2Int destTile){
        yield return StartCoroutine(Pause());
        yield return StartCoroutine(SelectUnit(unit));
        yield return StartCoroutine(DisplayMoveHighlights(unit, destTile));
        
        if (target != unit) // Attack only if needed
            yield return StartCoroutine(DisplayAttackHighlights(unit, target));
        
        yield return StartCoroutine(Pause());
        
    }

// Replace with animations? --------------------------------------------
    private IEnumerator SelectUnit(AttackingClass unit){
        Vector3 position = tmap.GetCellCenterLocal(new Vector3Int(unit.tilePosition.x, unit.tilePosition.y, 0));
        position.z = 0;
        aiCursor.transform.position = position;
        Debug.Log("Selecting");

        yield return new WaitForSeconds(1.0f);
    }


    private IEnumerator DisplayMoveHighlights(AttackingClass unit, Vector2Int destTile){
        Debug.Log("Moving");
        // Display Highlights
        tileManager.CreateRangeTiles(unit.tilePosition, unit.moveRange, Color.blue);

        yield return new WaitForSeconds(2.0f);

        // Show cursor at destination
        Vector3 position = tmap.GetCellCenterLocal(new Vector3Int(destTile.x, destTile.y, 0));
        position.z = 0;
        aiCursor.transform.position = position;
        yield return new WaitForSeconds(2.0f);

        // move for real
        unit.MoveToSpace(destTile);
        unit.transform.position = aiCursor.transform.position;
        tileManager.RemoveRangeTiles();
        yield return new WaitForSeconds(2.0f);


    }

    private IEnumerator DisplayAttackHighlights(AttackingClass unit, UnitBaseClass target){
        Debug.Log("Attacking");
        // Display Highlights
        tileManager.CreateRangeTiles(unit.tilePosition, unit.attackRange, Color.red);

        yield return new WaitForSeconds(2.0f);

        // Show cursor at target
        Vector3 position = tmap.GetCellCenterLocal(new Vector3Int(target.tilePosition.x, target.tilePosition.y, 0));
        position.z = 0;
        aiCursor.transform.position = position;
        yield return new WaitForSeconds(2.0f);

        // attack for real
        Debug.Log("YAAAAAAAAAAAA");
        unit.Attack(target);
        /*Forgot about the warrior case...I'll try to add later if time is available*/
        tileManager.RemoveRangeTiles();
        /*Since attacking will cause an animation to play for the unit and target, we'll need some delay between unit.Attack() and when the AI moves its next unit
         Could try to set up something that will inform the AI of when an animation is finished, but most likely, just using a coroutine wait will be much easier.*/
        yield return new WaitForSeconds(2.0f);
    }

    private IEnumerator Pause(){
        Debug.Log("pause");
        yield return new WaitForSeconds(2.0f);
    }
// End of coroutines---------------------------------------

    // Update is called once per frame
    void Update()
    {
        
    }
}
