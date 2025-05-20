using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionsGroups", menuName = "ScriptableObjects/Entities/Items/!Common/ActionsGroups")]
public class ActionsGroupList : ScriptableObject
{
    public List<ActionsGroup> ActionsGroups;
}
