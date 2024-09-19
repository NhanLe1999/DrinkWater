using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCupManager : MonoBehaviour
{
    [SerializeField] GameObject objPrefabCupChange = null;
    [SerializeField] Transform trsContence = null;

    void Start()
    {
        LoadData();
    }

    private void LoadData()
    {
        foreach(var it in LogicGame.Instance.dataCupGame.dataCups)
        {
            var ob = Instantiate(objPrefabCupChange, trsContence);
            var cpn = ob.GetComponent<ItemChangeCup>();
            cpn.SetData(it);
            cpn.UpdateUi();
        }
    }
   
}
