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
    private List<KeyValuePair<string, bool>> selectedUnitActions = null;

    private Vector3 pastPosition;

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

    public void OnClickBoard(InputAction.CallbackContext context){ // Called when left mouse button is selected
        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        
        if (context.phase == InputActionPhase.Started){
            // Cast a raycast
            RaycastHit hit;
            bool hasSelectedUnit = Physics.Raycast(Camera.main.transform.position, cursor.transform.position - Camera.main.transform.position, out hit);
            UnitBaseClass hitUnit = (!hasSelectedUnit) ? null : hit.collider.gameObject.GetComponent<UnitBaseClass>();
            
            //Debug.Log("Click");
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
                    if (hitUnit){ // If we have touched another unit
                        if (selectedUnit == hit.collider.gameObject){ // It is self
                            gridMode = SelectMode.PickActionMode; // Don't move. Just pick action.
                        }
                        if (!hitUnit.turnOver) // And they are friendly
                            SetSelUnit(hit.collider.gameObject);
                    } else { // If empty tile selected
                        selectedUnit.transform.position = cursor.transform.position;
                        gridMode = SelectMode.PickActionMode;
                            selectedUnitActions = selectedUnit.GetComponent<UnitBaseClass>().actions;
                            selectedUnitActions.Add(new KeyValuePair<string, bool>("Wait", false));
                            Debug.Log("Picking Action Mode!");
                            foreach(KeyValuePair<string, bool> actionPair in selectedUnitActions)
                            {
                                Debug.Log("Available action: " + actionPair.Key);
                            }
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

    public void OnSelectOption(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started && gridMode == SelectMode.PickActionMode)
        {
            string action = "";
            bool requiresTarget = false;
            switch (context.control.name)
            {
                case "1":
                    if(selectedUnitActions.Count > 0)
                    {
                        action = selectedUnitActions[0].Key;
                        requiresTarget = selectedUnitActions[0].Value;
                    }
                    break;
                case "2":
                    if (selectedUnitActions.Count > 1)
                    {
                        action = selectedUnitActions[1].Key;
                        requiresTarget = selectedUnitActions[1].Value;
                    }
                    break;
                case "3":
                    if (selectedUnitActions.Count > 2)
                    {
                        action = selectedUnitActions[2].Key;
                        requiresTarget = selectedUnitActions[2].Value;
                    }
                    break;
                case "4":
                    if (selectedUnitActions.Count > 3)
                    {
                        action = selectedUnitActions[3].Key;
                        requiresTarget = selectedUnitActions[3].Value;
                    }
                    break;
            };

            if(action != "") //Do not end action select if there is no action for the chosen value
            {
                Debug.Log("Doing action " + action);
                //Note that wait action is always last (for user friendliness)
                if (requiresTarget)
                {
                    Debug.Log("Choose target mode!");
                    gridMode = SelectMode.ChooseTargetMode;
                }
                else
                    EndSelUnitTurn();
            }
            //After selecting target, we execute the action
            //Can be done by seeing what the action was, then getting component of the class that action is defined in, and doing it.
        }
    }
    
    public void OnUndoMove(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started){
            switch (gridMode){
                case SelectMode.MoveMode:{
                    // Unselect unit
                    selectedUnit  = null;
                    // Back to IdleMode
                    gridMode = SelectMode.IdleMode;
                    Debug.Log("Back to Idle");
                } break;

                case SelectMode.PickActionMode:{
                    // Unit goes back to past position
                    // Set to MoveMode (there is a possibility Idle mode makes more sense but...)
                    selectedUnit.transform.position = pastPosition;
                    gridMode = SelectMode.MoveMode;
                    Debug.Log("Back to Move");
                } break;

                case SelectMode.ChooseTargetMode:{
                    // Unit Goes back to picking action
                    gridMode = SelectMode.PickActionMode;
                    Debug.Log("Back to PickAction");
                } break;

            }
        }
    }

    private void SetSelUnit(GameObject unit){
        selectedUnit = unit;
        pastPosition = selectedUnit.transform.position;
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
