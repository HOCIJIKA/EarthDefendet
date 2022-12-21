using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBuy : MonoBehaviour
{
    [Header("Вартість відкриття контейнера")]
    [Header("CostText")]
    [SerializeField] private Text costText;

    [SerializeField] private Button button;
    [SerializeField] private Container _container;
    
    private int _parentIndex;
    private int _cost;
    
    public void ButtonInteractable(bool f)
    {
        button.interactable = f;
    }

    public void Buy()
    {
        ButtonInteractable(false); // або взагалі прибрати кнопку.

        _container.CheckContainerAfterBuy(true);
        GameC.Instance.GetData().IsActiveContainers[_parentIndex] = true;
        GameC.Instance.SaveData();
        gameObject.SetActive(false);
        _container.ButtonHideList.SetActive(false);
        //_container.ShowSkillList(true);
        _container.ShowSkillListWithoutButtonHide(true);

        PointsCollector.Instance.SubPoints(_cost);

        foreach (var openedContainer in GameC.Instance.GetData().IsActiveContainers)
        {
            if (!openedContainer) return;
            Social.ReportProgress(GPGSIds.achievement_open_all_containers, 100.0f, success =>{});
        }
    }

    public void SetCost(int cost)
    {
        _cost = cost;
        costText.text = _cost.ToString();
    }

    public void SetParent(Container c)
    {
        _container = c;
    }
    public void SetIndex(int i)
    {
        _parentIndex = i;
    }
}
