using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
// Todo change tile coords to vector2int

public class GridManager : MonoBehaviour
{
    private GameObject cursor;
    private Grid tmap;

    private Vector3Int tileCoords;

    // Start is called before the first frame update
    void Start()
    {
        cursor = GameObject.Find("Cursor");
        tmap = GameObject.Find("Grid").GetComponent<Grid>();
        tileCoords = new Vector3Int(0, 0, 0);

        setCursorPos();
    }

    public void OnGridMovement(InputAction.CallbackContext context){

        //Debug.Log($"Movement {context.phase} {context.ReadValue<Vector2>()}");
        Vector2Int curr_val = Vector2Int.CeilToInt(context.ReadValue<Vector2>());

        
        
        switch (context.phase){
            case InputActionPhase.Started:
                break;
            
            case InputActionPhase.Performed:
                Vector3Int newval = new Vector3Int();
                newval.x = +curr_val.y;
                newval.y = -curr_val.x;
                Debug.Log(newval);
                updateCursorPos(newval);
                break;
            
            case InputActionPhase.Canceled:

                break;
        }
    }

    private void updateCursorPos(Vector3Int vIn){
        //tileCoords
        tileCoords += vIn;
        setCursorPos();
    }
    
    private void setCursorPos(){
        Vector3 dest = tmap.GetCellCenterLocal(tileCoords);
        dest.z = 0;
        cursor.transform.position = dest;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
