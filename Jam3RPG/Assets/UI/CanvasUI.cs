using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    public Animation uiAnims;

    public void StartPlayerPhase()
    {
        uiAnims.Play("CanvasPlayerPhaseStart");
    }

    public void StartAiPhase()
    {
        uiAnims.Play("CanvasAiPhaseStart");
    }

    public void StartPlayerVictory()
    {
        uiAnims.Play("CanvasPlayerVictory");
    }

    public void StartPlayerLose()
    {
        uiAnims.Play("CanvasPlayerLose");
    }
}
