using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Water2D;

public class ClickWater : MonoBehaviour
{
    public Water2D_Spawner _Spawner;

    private void OnMouseDown()
    {
        _Spawner.IsPlayIng = false; ;
        _Spawner?.StopSpawning();
        _Spawner.IsPlayIng = true;
        _Spawner?.Spawn();
    }

    private void OnMouseUp()
    {
        _Spawner.IsPlayIng = false; ;
        _Spawner?.StopSpawning();
    }

}
