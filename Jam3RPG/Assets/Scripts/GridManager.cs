using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;

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
    private Grid tmap;

    private Vector3Int tileCoords;
    private GameObject selectedUnit = null;

    // private (i think we should make a grid???)
    private SelectMode gridMode =  SelectMode.IdleMode;

    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        tmap = GameObject.Find("Grid").GetComponent<Grid>();
        tileCoords = new Vector3Int(0, 0, 0);

        SetCursorPos(tileCoords);
    }

    public void OnGridMovement(InputAction.CallbackContext context){ // Called when WASD is pressed... kinda useless
        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        Vector2Int curr_val = Vector2Int.CeilToInt(context.ReadValue<Vector2>());
        
        switch (context.phase){
            case InputActionPhase.Started:
                break;
            
            case InputActionPhase.Performed:
                Vector3Int newval = new Vector3Int();
                newval.x = +curr_val.y;
                newval.y = -curr_val.x;
                //Debug.Log(newval);
                UpdateCursorPos(newval);
                break;
            
            case InputActionPhase.Canceled:

                break;
        }
    }

    public void OnCursorMove(InputAction.CallbackContext context){
        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        //Gets the tile that the mouse cursor (not the in-game one) is pointing at
        //Then, sets the in-game cursor to be hovering over that tile
        Vector2Int curr_val = Vector2Int.CeilToInt(context.ReadValue<Vector2>());
        Vector3 dest = Camera.main.ScreenToWorldPoint(new Vector3(curr_val.x, curr_val.y, Camera.main.nearClipPlane));
        Vector3Int tile = tmap.LocalToCell(dest);
        tile.z = 0; // 
        SetCursorPos(tile);
    }

    public void OnSelect(InputAction.CallbackContext context){ // Called when left mouse button is selected
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
                        selectedUnit = hit.collider.gameObject; // CH1
                        gridMode = SelectMode.MoveMode; // Moving units now...
                        Debug.Log("Moving to Move Mode!");
                    }
                } break;

                case SelectMode.MoveMode: // We have something selected
                {
                    if (hitUnit){ // If we have touched another unit
                        if (selectedUnit == hit.collider.gameObject){ // It is self
                            gridMode = SelectMode.PickActionMode; // Don't move. Just pick action.
                        }
                        if (!hitUnit.turnOver) // And they are friendly
                            selectedUnit = hit.collider.gameObject; // CH1
                    } else { // If empty tile selected
                        selectedUnit.transform.position = cursor.transform.position;
                        gridMode = SelectMode.PickActionMode;
                        Debug.Log("Picking Action Mode!");
                    }
                } break;
                
                //Case: Pick action mode!
                // 1 -> Action -> ChooseTargetMode -> IdleMode
                // 2 -> Wait -> IdleMode
                // 3 -> Sacrifice

                case SelectMode.ChooseTargetMode: // We are choosing a target
                {
                    Debug.Log("MOOO");
                    // Assume actions only hit enemies...
                    if (hitUnit && hitUnit.turnOver){
                        hitUnit.ChangeHealth(-5);
                        EndSelUnitTurn();
                    }
                } break;
                    
            }
        }
    }

    public void SelectOption(InputAction.CallbackContext context){
        if (gridMode == SelectMode.PickActionMode)
        switch(context.control.name){
            case "1": // Action
                gridMode = SelectMode.ChooseTargetMode;
                Debug.Log("MOOOOOOO");
                break;
            
            case "2": // Wait
                EndSelUnitTurn();
                break;

            case "3": // Sacrifice but i dunno what do...
                break;
        };
    }
    

    private void EndSelUnitTurn(){
        selectedUnit.GetComponent<UnitBaseClass>().FinishTurn(); 
        selectedUnit = null;
        gridMode = SelectMode.IdleMode;
    }

    private void UpdateCursorPos(Vector3Int vIn){
        //tileCoords
        tileCoords += vIn;
        SetCursorPos(tileCoords);
    }
    
    private void SetCursorPos(Vector3Int pos){
        Vector3 dest = tmap.GetCellCenterLocal(pos);
        //Debug.Log(pos);
        dest.z = 0;
        cursor.transform.position = dest;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
