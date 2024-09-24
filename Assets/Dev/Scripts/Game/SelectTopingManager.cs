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
        
    }

    public void OnLoadData()
    {
        for(int i = 0; i < trsContence.childCount; i++)
        {
            Destroy(trsContence.GetChild(i).gameObject);
        }

        foreach (var it in LogicGame.Instance.dataToping)
        {
            var ob = Instantiate(objPrefabCupChange, trsContence);
            var cpn = ob.GetComponent<TopingSelectItem>();
            cpn.SetData(it);
            cpn.UpdateUi();
        }
    }
}
