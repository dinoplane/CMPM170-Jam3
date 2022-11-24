using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;


public class GridManager : MonoBehaviour
{
    private GameObject cursor;
    private Grid tmap;

    private Vector3Int tileCoords;
    private GameObject selected = null;

    // private (i think we should make a grid???)

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
        Debug.Log("Click");

        switch (context.phase){
            case InputActionPhase.Started:
                RaycastHit hit;

                // Cast a raycast 
                if (Physics.Raycast(Camera.main.transform.position, cursor.transform.position - Camera.main.transform.position, out hit)) {
                    // select collided object
                    Debug.DrawRay(Camera.main.transform.position, cursor.transform.position - Camera.main.transform.position, Color.yellow);
                    Debug.Log("Did Hit");

                    /*Add a check here to determine if selected the same square that the unit was just in (as in selected same unit)
                      In which case, go into action selection
                      This allows a unit to act without moving*/

                    UnitBaseClass hitUnit = hit.collider.gameObject.GetComponent<UnitBaseClass>();

                    // TESTING ATTACKING
                    hitUnit.Attack(hitUnit);
                    // TESTING OVER

                    if (hitUnit && !hitUnit.turnOver) //Selected a unit class? Unit class's turn is NOT over?
                    {
                            selected = hit.collider.gameObject;
                    }
                } else { // place selected object if exists
                    Debug.DrawRay(Camera.main.transform.position, cursor.transform.position - Camera.main.transform.position , Color.white);
                    Debug.Log("Did not Hit");
                    if (selected != null){
                        //Arrian can get the unit's movement range here to see if the selected tile is within that range or not.

                        /*Also having moved the selected unit, can tell the grid manager to go into an action selection right now,
                        instead of ending the unit's turn
                        After action select is finished (maybe attacked a target, maybe just waited), then end the units turn and
                        go back into click-and-move mode
                         */

                        selected.transform.position = cursor.transform.position;
                        //(Below) Unit has moved, set turn as over (Later, turn will be set as finished via a different method)
                        //
                        selected.GetComponent<UnitBaseClass>().FinishTurn(); 
                        selected = null;
                    }
                }
                break;
            
            case InputActionPhase.Performed:
                break;
            
            case InputActionPhase.Canceled:
                break;
        }
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
