using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameStateSO", menuName = "Scriptable Objects/Item")]
public class GameStateSO : ScriptableObject
{
    public int gameCase;
    public int ragdollSaved;
    public int ragdollTotal;
}
