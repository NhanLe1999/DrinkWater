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

        LogicGame.Instance.txtLog.text = "DCMMMMMMMMMMMM_OnMouseDown";
        Debug.Log("DCMMMMMMMMMMMM_OnMouseDown");
    }

    private void OnMouseUp()
    {
        LogicGame.Instance.txtLog.text = "DCMMMMMMMMMMMM_OnMouseUp";
        Debug.Log("DCMMMMMMMMMMMM_OnMouseUp");
        _Spawner.IsPlayIng = false; ;
        _Spawner?.StopSpawning();
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
