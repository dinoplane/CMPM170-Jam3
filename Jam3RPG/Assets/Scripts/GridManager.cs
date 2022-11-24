using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

// Clean up code tasks
// selected could be of type unit base!
// 

enum SelectMode{
    IdleMode, // Nothing selected
    MoveMode, // Moving selected unit
    PickActionMode, // Picking action from UI
    ChooseTargetMode, // Choosing target for action
    SacrificeMode // Sacrificing??? Not sure if we need.
};

public class GridManager : MonoBehaviour
{

    private GameObject cursor;
    public Sprite valid;
    public Sprite invalid;

    private Grid tmap;

    private Vector2Int cursorTileCoords;// of cursor

    private GameObject selectedUnit = null;

    private Vector2Int pastTile;// of units
    private Vector3 pastPosition;

    // private (i think we should make a grid???)
    private SelectMode gridMode =  SelectMode.IdleMode;

    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        tmap = GameObject.Find("Grid").GetComponent<Grid>();
        cursorTileCoords = new Vector2Int(0, 0);

        SetCursorPos(cursorTileCoords);

        // Snap all units to grid
        UnitBaseClass[] units = FindObjectsOfType<UnitBaseClass>();
        foreach(UnitBaseClass unit in units)
        {
            SnapUnitToGrid(unit);
        }
    }

    public void OnGridMovement(InputAction.CallbackContext context){ // Called when WASD is pressed... kinda useless
        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        // Vector2Int curr_val = Vector2Int.CeilToInt(context.ReadValue<Vector2>());
        
        // switch (context.phase){
        //     case InputActionPhase.Started:
        //         break;
            
        //     case InputActionPhase.Performed:
        //         Vector3Int newval = new Vector3Int();
        //         newval.x = +curr_val.y;
        //         newval.y = -curr_val.x;
        //         //Debug.Log(newval);
        //         UpdateCursorPos(newval);
        //         break;
            
        //     case InputActionPhase.Canceled:

        //         break;
        // }
    }

    public void OnCursorMove(InputAction.CallbackContext context){
        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        //Gets the tile that the mouse cursor (not the in-game one) is pointing at
        //Then, sets the in-game cursor to be hovering over that tile
        Vector2Int curr_val = Vector2Int.CeilToInt(context.ReadValue<Vector2>());
        Vector3 dest = Camera.main.ScreenToWorldPoint(new Vector3(curr_val.x, curr_val.y, Camera.main.nearClipPlane));
        Vector3Int tile = tmap.LocalToCell(dest);
        cursorTileCoords.x = tile.x;
        cursorTileCoords.y = tile.y;

        SetCursorPos(cursorTileCoords);

        if (gridMode == SelectMode.MoveMode){
            Debug.Log(cursorTileCoords);
            UnitBaseClass unit = selectedUnit.GetComponent<UnitBaseClass>();
            UpdateCursorSprite(unit.tilePosition, unit.moveRange);

        } else if (gridMode == SelectMode.ChooseTargetMode){
            AttackingClass unit = selectedUnit.GetComponent<AttackingClass>();
            UpdateCursorSprite(unit.tilePosition, unit.attackRange);
        }
    }

    public void OnClickBoard(InputAction.CallbackContext context){ // Called when left mouse button is selected
        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        
        if (context.phase == InputActionPhase.Started){
            // Cast a raycast
            RaycastHit hit;
            bool hasSelectedUnit = Physics.Raycast(Camera.main.transform.position, cursor.transform.position - Camera.main.transform.position, out hit);
            UnitBaseClass hitUnit = (!hasSelectedUnit) ? null : hit.collider.gameObject.GetComponent<UnitBaseClass>();
            
            Debug.Log("Click");
            switch (gridMode){
                case SelectMode.IdleMode: // We don't have anything selected
                {
                    // If we selected a unit 
                    if (hitUnit && !hitUnit.turnOver){ //Selected a unit class? 
                        SetSelUnit(hit.collider.gameObject);
                        gridMode = SelectMode.MoveMode; // Moving units now...
                        Debug.Log("Moving to Move Mode!");
                    }
                } break;

                case SelectMode.MoveMode: // We have something selected
                {
                    UnitBaseClass unit = selectedUnit.GetComponent<UnitBaseClass>();
                    if (hitUnit){ // If we have touched another unit
                        if (selectedUnit == hit.collider.gameObject) // And it is self
                            gridMode = SelectMode.PickActionMode; // Don't move. Just pick action.

                        else if (!hitUnit.turnOver) // And they are friendly
                            SetSelUnit(hit.collider.gameObject);

                    } else { // If empty tile selected
                        if (checkTileInRange(unit.tilePosition, cursorTileCoords, unit.moveRange)){
                            unit.MoveToSpace(cursorTileCoords);
                            selectedUnit.transform.position = cursor.transform.position;
                            gridMode = SelectMode.PickActionMode;
                            Debug.Log("Picking Action Mode!");
                        }
                    }
                } break;
                
                //Case: Pick action mode!
                // 1 -> Action -> ChooseTargetMode -> IdleMode
                // 2 -> Wait -> IdleMode
                // 3 -> Sacrifice

                case SelectMode.ChooseTargetMode: // We are choosing a target
                {
                    // Assume actions only hit enemies...
                    AttackingClass unit = selectedUnit.GetComponent<AttackingClass>();
                    if (checkTileInRange(unit.tilePosition, cursorTileCoords, unit.attackRange) && 
                            hitUnit && hitUnit.turnOver){ // check if target is in attack range and if target is valid
                                hitUnit.ChangeHealth(-5);
                                EndSelUnitTurn();
                    }
                } break;
                    
            }
        }
    }

    public void OnSelectOption(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started && gridMode == SelectMode.PickActionMode)
            switch(context.control.name){
                case "1": // Action
                    gridMode = SelectMode.ChooseTargetMode;
                    AttackingClass unit = selectedUnit.GetComponent<AttackingClass>();
                    UpdateCursorSprite(unit.tilePosition, unit.attackRange);
                    break;
                
                case "2": // Wait
                    EndSelUnitTurn();
                    break;

                case "3": // Sacrifice but i dunno what do...
                    break;
            };
    }
    
    public void OnUndoMove(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started){
            switch (gridMode){
                case SelectMode.MoveMode:{
                    // Unselect unit
                    selectedUnit  = null;
                    // Back to IdleMode
                    gridMode = SelectMode.IdleMode;
                    UpdateCursorSprite(cursorTileCoords, 0);
                    Debug.Log("Back to Idle");
                } break;

                case SelectMode.PickActionMode:{
                    // Unit goes back to past position
                    // Set to MoveMode (there is a possibility Idle mode makes more sense but...)
                    UnitBaseClass unit = selectedUnit.GetComponent<UnitBaseClass>();
                    selectedUnit.transform.position = pastPosition;
                    unit.MoveToSpace(pastTile);
                    gridMode = SelectMode.MoveMode;

                    UpdateCursorSprite(unit.tilePosition, unit.moveRange);

                    Debug.Log("Back to Move");
                } break;

                case SelectMode.ChooseTargetMode:{
                    // Unit Goes back to picking action
                    gridMode = SelectMode.PickActionMode;
                    UpdateCursorSprite(cursorTileCoords, 0);
                    Debug.Log("Back to PickAction");
                } break;

            }
        }
    }

    public void SnapUnitToGrid(UnitBaseClass unit){
        // You have unit WORLD coordinates so use WorldToCell
        Vector3Int tile = tmap.LocalToCell(unit.transform.position);
        unit.tilePosition = new Vector2Int(tile.x, tile.y);
        tile.z = 0;

        // Then snap the unit to the tile!
        Vector3 dest = tmap.GetCellCenterLocal(tile);
        //Debug.Log(pos);
        dest.z = 0;
        unit.transform.position = dest;
    }



    private bool checkTileInRange(Vector2Int src, Vector2Int dst, int range){ // checks if given tile coordinate is in range
        // Get the relative tile position from the source
        Vector2Int diff = src - dst; 
        
        // Get abs val
        diff.x = Mathf.Abs(diff.x);
        diff.y = Mathf.Abs(diff.y);

        // get length of path
        int length = diff.x + diff.y;        
        Debug.Log(length <= range);

        return length <= range;
    }

    private void SetSelUnit(GameObject unit){
        selectedUnit = unit;
        pastTile = selectedUnit.GetComponent<UnitBaseClass>().tilePosition;
        pastPosition = selectedUnit.transform.position;
    }

    private void EndSelUnitTurn(){
        selectedUnit.GetComponent<UnitBaseClass>().FinishTurn(); 
        selectedUnit = null;
        gridMode = SelectMode.IdleMode;
    }

    private void UpdateCursorSprite(Vector2Int tilePosition, int range){ // updates cursor state based on cursor position
        if (!checkTileInRange(tilePosition, cursorTileCoords, range))
            cursor.GetComponent<SpriteRenderer>().sprite = invalid;
        else cursor.GetComponent<SpriteRenderer>().sprite = valid;
    }

    private void UpdateCursorPos(Vector2Int vIn){
        //cursorTileCoords
        cursorTileCoords += vIn;
        SetCursorPos(cursorTileCoords);
    }
    
    private void SetCursorPos(Vector2Int pos){
        Vector3 dest = tmap.GetCellCenterLocal(new Vector3Int(pos.x, pos.y, 0));
        //Debug.Log(pos);
        dest.z = 0;
        cursor.transform.position = dest;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
