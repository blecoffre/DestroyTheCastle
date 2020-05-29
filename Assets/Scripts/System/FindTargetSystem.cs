using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class FindTargetSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.WithAll<Unit>().ForEach((Entity entity) =>
        {
            //Code running on all entities with "Unit" Tag
        });
    }
}
