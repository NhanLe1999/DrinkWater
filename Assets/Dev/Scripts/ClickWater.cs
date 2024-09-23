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
        Audio.Play(ScStaticScene.SFX_rot_nuoc, 1.0f,true);
    }

    private void OnMouseUp()
    {
        _Spawner.IsPlayIng = false; ;
        _Spawner?.StopSpawning();
        Audio.Stop(ScStaticScene.SFX_rot_nuoc);
    }

    void OnPlay()
    {
        _Spawner.IsPlayIng = false; ;
        _Spawner?.StopSpawning();
        _Spawner.IsPlayIng = true;
        _Spawner?.Spawn();
    }

    void EndPlay()
    {
        _Spawner.IsPlayIng = false; ;
        _Spawner?.StopSpawning();
    }

    private void Start()
    {
        if (ScStaticScene.State == 3 || ScStaticScene.State == 2)
        {
            this.StartCoroutine(OnPlayWater());
        }
    }

    IEnumerator OnPlayWater()
    {
        yield return new WaitForSeconds(0.2f);
        OnPlay();
        yield return new WaitForSeconds(1.0f);
        EndPlay();
    }

}
