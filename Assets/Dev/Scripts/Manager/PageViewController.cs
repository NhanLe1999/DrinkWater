using DG.Tweening;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PageViewController : MonoBehaviour
{
    [SerializeField] List<Sprite> sprId = new();

    [SerializeField] GameObject objPrefabIdencator = null;
    [SerializeField] GameObject prefabPageView = null;

    [SerializeField] GameObject prefabCupSelect = null;


    [SerializeField] Transform trParentId = null;

    public ScrollRect scrollRect; 
    public RectTransform contentPanel;
    private int totalPages;
    private float[] pagePositions;
    private List<RectTransform> ListPage = new();
    private List<Image> ListIden = new();

    private Vector2 startDragPosition;
    private Vector2 endDragPosition;

    int currentPage = 0;

    private void Start()
    {
        currentPage = 0;
    }

    public void SetData(List<DataCup> dataCups)
    {
        int numPage = dataCups.Count / 9;

        if(dataCups.Count % 9 != 0)
        {
            numPage += 1;
        }

        int indexItem = 0;

        for (int i = 0; i < numPage; i++)
        {
            var id = Instantiate(objPrefabIdencator, trParentId);
            id.gameObject.SetActive(true);
            ListIden.Add(id.GetComponent<Image>());

            var page = Instantiate(prefabPageView, contentPanel);
            page.gameObject.SetActive(true);
            ListPage.Add(page.GetComponent<RectTransform>());


            for(int k = 0; k < 9; k++)
            {
                if(indexItem >= dataCups.Count)
                {
                    break;
                }
                var itSelect = Instantiate(prefabCupSelect, page.transform);
                var cpn = itSelect.GetComponent<ItemSelectCup>();
                cpn.SetDataCup(dataCups[indexItem]);
                cpn.UpdateUI();
                indexItem++;

            }

        }
        LoadSprId();
    }    

    private void LoadSprId()
    {
        for(int i = 0; i < ListIden.Count; i++)
        {
            ListIden[i].sprite = i == currentPage ? sprId[0] : sprId[1];
        }
    }

    public void OnBeginDrag()
    {
        startDragPosition = Input.mousePosition;
    }

    public void OnEndDrag()
    {
        endDragPosition = Input.mousePosition;
        float dragDistance = Vector2.Distance(startDragPosition, endDragPosition);
        ScrollToTargetSmooth(ListPage[GetPage(dragDistance)], 0);
        LoadSprId();
    }


    private int GetPage(float dis)
    {
        if(dis > 100)
        {
            if(startDragPosition.x < endDragPosition.x)
            {
                currentPage--;
            }
            else
            {
                currentPage++;
            }
        }    

        if(currentPage < 0)
        {
            currentPage = 0;
        }

        if (currentPage >= ListPage.Count)
        {
            currentPage = ListPage.Count - 1;
        }

        return currentPage;
    }    

    public void ScrollToTargetSmooth(RectTransform target, float pointAdd)
    {
        var newPos = contentPanel.anchoredPosition;
        newPos.x =
            scrollRect.transform.InverseTransformPoint(contentPanel.position).x -
            scrollRect.transform.InverseTransformPoint(target.transform.position).x + target.sizeDelta.x / 2;

        contentPanel.DOAnchorPosX(newPos.x, 0.15f, true).SetEase(Ease.OutQuad);

    }

    private void Update()
    {
        
    }
}
