using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SquadStuff : MonoBehaviour
{
    public List<Vector2Int> squad0;
    public List<Vector2Int> squad1;
    public List<Vector2Int> squad2;
    public List<Vector2Int> squad3;
    public List<Vector2Int> squad4;
    public List<Vector2Int> squad5;
    public List<Vector2Int> squad6;
    public List<Vector2Int> squad7;
    public List<Vector2Int> squad8;
    public List<Vector2Int> squad9;

    static public Dictionary<int, bool> squadAggro = new  Dictionary<int, bool>();
    static public List<List<Vector2Int>> squadList = new List<List<Vector2Int>>();


    void Start(){
        AddSquad(squad0,0);
        AddSquad(squad1,1);
        AddSquad(squad2,2);
        AddSquad(squad3,3);
        AddSquad(squad4,4);
        AddSquad(squad5,5);
        AddSquad(squad6,6);
        AddSquad(squad7,7);
        AddSquad(squad8,8);
        AddSquad(squad9,9);
    }

    private void AddSquad(List<Vector2Int> squad, int num){
        if(squad.Count == 2){
            squadList.Add(squad);
            squadAggro.Add(num, false);
        }

    }

}
