using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsContainer : MonoBehaviour
{
    public delegate void UpdateCostSkiilUse();
    public event UpdateCostSkiilUse UpdateSkillsCost;
    public delegate void ShowNonActiveSkiils();
    public event ShowNonActiveSkiils ShowNonActiveSkill;

    [SerializeField] private GameObject _containerPref;
    [SerializeField] private List<Transform> _containersTarget;
    [SerializeField] private List<GameObject> _skillsContainers;
    public List<GameObject> SkillsContainers => _skillsContainers;
    
    [SerializeField] private List<int> _costForContainers;

    private List<bool> isActiveContainers;

    private static SkillsContainer instance;
    public static SkillsContainer Instance => instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Init()
    {
        CreateContainers();
        UnlockContainers(PointsCollector.Instance.GetPoints());
    }

    private void CreateContainers()
    {
        for (int i = 0; i < 4; i++)
        {
            _skillsContainers.Add(Instantiate(_containerPref, _containersTarget[i]));
        }

        for (int i = 0; i < _skillsContainers.Count; i++)
        {
            if (GameC.Instance.GetData().IsActiveContainers[i])
            {
                _skillsContainers[i].GetComponent<Container>().Init(i,true);
            }
            else
            {
                _skillsContainers[i].GetComponent<Container>().Init(i);

            }

            _skillsContainers[i].GetComponent<Container>().SetValueButtonBuy(_costForContainers[i]);
        }
    }

    public void UnlockContainers(int allPoints)
    {
        for (int i = 0; i < _skillsContainers.Count; i++)
        {
            _skillsContainers[i].GetComponent<Container>().SetInteractableButtonBuy(allPoints > _costForContainers[i]);
        }
    }

    public void UpdateCostSkills()
    {
        UpdateSkillsCost?.Invoke();
    }

    public void ShowNonActiveSkills()
    {
        ShowNonActiveSkill?.Invoke();
    }
}
