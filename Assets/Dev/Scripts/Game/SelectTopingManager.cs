using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectTopingManager : MonoBehaviour
{
    [SerializeField] GameObject objPrefabCupChange = null;
    [SerializeField] Transform trsContence = null;
    void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        foreach (var it in LogicGame.Instance.dataTopingGame.dataToping)
        {
            var ob = Instantiate(objPrefabCupChange, trsContence);
            var cpn = ob.GetComponent<TopingSelectItem>();
            cpn.SetData(it);
            cpn.UpdateUi();
        }
    }
}
