using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour
{
    public GameObject ButtonHideList;

    [SerializeField] private int _index;
    [Header("Skills List")]
    [SerializeField] private List<Button> buttonsSkill;
    [Header("заголовок")]
    [Tooltip("спливаюча підказкав едіторі")]
    [SerializeField] private GameObject showList;
    [SerializeField] private GameObject listAllSkills;
    [SerializeField] private GameObject loсk;
    [SerializeField] private RectTransform anchor;
    [SerializeField] private ButtonBuy _buttonBuy;
    [SerializeField] private ButtonUp _buttonUp;

    public ButtonBuy ButtonBuy => _buttonBuy;
    public GameObject ListAllSkills => listAllSkills;
    
    /// <summary>
    /// Блокуєм контейнер якщо гравечь не відкрив його
    /// </summary>
    private bool lockcurrnetContainer;
    private GameObject curentSkill;
    private bool _unLockedCurrentContainer;
    private int _indexCurrentSkill;
    private GameObject _currentSkill;

    public void Init(int skillIndex, bool isUnlocked = false)
    {
        _index = skillIndex;
        
        _unLockedCurrentContainer = isUnlocked;
        if (!_unLockedCurrentContainer)
        {
            listAllSkills.SetActive(false);
            showList.SetActive(false);
            loсk.SetActive(true);
        }
        else
        {
            loсk.SetActive(false);
            showList.SetActive(true);
        }

        // розблокувати кнопку якщо вона була куплена.
        if (_unLockedCurrentContainer)
        {
            SetCurrentSkillOnStart(GameC.Instance.GetData().ContainerNumberSkill[_index]);
            
            if (!Social.localUser.authenticated) return;
            Social.ReportProgress(GPGSIds.achievement_opened_first_skill_container, 100.0f, success =>{});
        }
    }
    
    public void CheckContainerAfterBuy(bool isUnlocked)
    {
        _unLockedCurrentContainer = isUnlocked;

        if (!_unLockedCurrentContainer)
        {
            listAllSkills.SetActive(false);
            showList.SetActive(false);
        }
        else
        {
            loсk.SetActive(false);
            showList.SetActive(true);
        }
    }

    public void ShowSkillList(bool isShowed)
    {
        if (isShowed)
        {
            SkillsContainer.Instance.ShowNonActiveSkills();
        }
        ButtonHideList.SetActive(true);
        listAllSkills.SetActive(isShowed);
        showList.SetActive(!isShowed);       
    }

    public void ShowSkillListWithoutButtonHide(bool isShowed,  bool isFirstShowing = false)
    {
        if (isShowed)
        {
            SkillsContainer.Instance.ShowNonActiveSkills();
        }
        
        ButtonHideList.SetActive(false);
        listAllSkills.SetActive(isShowed);
        showList.SetActive(!isShowed);       
    }
    /// <summary>
    /// Задаєм скіл для контейнеру.
    /// </summary>
    /// <param name="skillIndex">номер скілав списку, задається з кнопки.</param>
    public void SetCurrentSkill(int skillIndex)
    {        
        // відміняєм дію попереднього скіла якщо такий є
        if (curentSkill != null)
            Destroy(curentSkill);

        var buttonSkill = ListButtonsSkills.Instance.GetButtonSkill(skillIndex);
        curentSkill = Instantiate(buttonSkill, anchor);
        curentSkill.SetActive(true);
        ShowSkillList(false);

        // зберігаєм дані про контейнер (який скіл активований)
        GameC.Instance.GetData().ContainerNumberSkill[_index] = skillIndex;
        GameC.Instance.SaveData();
        _buttonUp.SetSkillUpIndex(skillIndex);
        _indexCurrentSkill = skillIndex;//

        ActiveButtonUp();
    }
    
    /// <summary>
    /// Активували і задали вартість всіх кнопок апгрейда 
    /// </summary>
    public void ActiveButtonUp()
    {
        if (_unLockedCurrentContainer)
        {
            SetActiveButtonBuy(false);
            SetActiveButtonUp(true);
            SetCostUp(SkillsUpgrade.Instance.GetValueSkillUp(_indexCurrentSkill));

            if ( PointsCollector.Instance.GetPoints() > SkillsUpgrade.Instance.GetValueSkillUp(_indexCurrentSkill))
            {        
                var costSkillsUp = SkillsUpgrade.Instance.GetCurrentCostSkillUp();

                for (int c = 0; c < costSkillsUp.Count; c++)
                {
                    if (c == _indexCurrentSkill)
                        if (costSkillsUp[c] < PointsCollector.Instance.GetPoints())
                        {
                            // включили кнопку
                            SetInteractableButtonUp(true);
                        }
                        else
                        {
                            SetInteractableButtonUp(false);
                        }
                }
            }
        }
    }
    
    private void OnEnable()
    {
        SkillsContainer.Instance.ShowNonActiveSkill += ShowNonActiveSkills;
    }
    private void OnDestroy()
    {
        SkillsContainer.Instance.ShowNonActiveSkill -= ShowNonActiveSkills;
    }

    private void ShowNonActiveSkills()
    {
        var listAllSkillContainerActive = GameC.Instance.GetData().ContainerNumberSkill;

        foreach (var button in buttonsSkill)
        {
            button.interactable = true;
        }

        foreach (var item in listAllSkillContainerActive)
        {
            for (var i = 0; i < buttonsSkill.Count; i++)
            {
                if (i == item)
                {
                    buttonsSkill[i].interactable = false;
                    break;
                }
            }
        }
    }
    
    public void SetValueButtonBuy(int cost)
    {
        _buttonBuy.SetCost(cost);
        _buttonBuy.SetParent(this);
        _buttonBuy.SetIndex(_index);
    }
    public void SetInteractableButtonBuy(bool isInteractable)
    {
        _buttonBuy.ButtonInteractable(isInteractable);
    }

    public void SetCostUp(int cost)
    {
        _buttonUp.SetCostUp(cost);
    }

    private void SetActiveButtonBuy(bool f)
    {
        _buttonBuy.gameObject.SetActive(f);
    }
    private void SetActiveButtonUp(bool isActive)
    {
        _buttonUp.gameObject.SetActive(isActive);
        
        if (isActive)
        {
            loсk.SetActive(false);
        }
        
    }
    
    private void SetInteractableButtonUp(bool value)
    {
        _buttonUp.ButtonInteractable(value);
    }

    /// <summary>
    /// Задаєм скіл для контейнеру.
    /// </summary>
    /// <param name="skillIndex">номер скілав списку, задається з кнопки.</param>
    private void SetCurrentSkillOnStart(int skillIndex)
    {        
        // відміняєм дію попереднього скіла якщо такий є
        if (_currentSkill != null)
        {
            Destroy(_currentSkill);
        }

        var buttonSkill = ListButtonsSkills.Instance.GetButtonSkill(skillIndex);
        _currentSkill = Instantiate(buttonSkill, anchor);
        _currentSkill.SetActive(true);
        ShowSkillList(false);

        // зберігаєм дані про контейнер (який скіл активований)
        GameC.Instance.GetData().ContainerNumberSkill[_index] = skillIndex;
        GameC.Instance.SaveData();
        _buttonUp.SetSkillUpIndex(skillIndex); 
        _indexCurrentSkill = skillIndex;

        ActiveButtonUp();
    }
}
