using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/AllEntities", order = 1)]

public class EntityTemplate : ScriptableObject
{
    public Entity player;
}
