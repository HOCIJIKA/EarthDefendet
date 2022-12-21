using UnityEngine;
using UnityEngine.UI;

public class ButtonUp : MonoBehaviour
{
#pragma warning disable 0649
    [Header("Вартість апгрейда скіла")]
    [SerializeField] private int costUp;
    [Space (10)]
    [SerializeField] private Text costUpText;
    [Space(10)]
    [SerializeField] private int skillUpIndex;
    [SerializeField] private Button button;
#pragma warning restore 0649

    private PointsCollector pointsCollector;
    private SkillsContainer skillsContainer;
    private SkillsUpgrade skillsUpgrade;

    private void Awake()
    {
        pointsCollector = FindObjectOfType<PointsCollector>();
        skillsContainer = FindObjectOfType<SkillsContainer>();
        skillsUpgrade = FindObjectOfType<SkillsUpgrade>();
    }

    public void UpgradeSkill()
    {
        if (pointsCollector.GetPoints() > 0)
        {
            // тут апаєм скіл. Скіл апгрейдиться в незалежності від його контейнера(перента).
            //Також потрібно зберігати і обновляти вартісь скіла.
            //Також міняти параметри скіла який апається.

            //Дістали і підняли рівень скіла
            skillsUpgrade.UpgradeSkill(skillUpIndex);

            var AllContainers = skillsContainer.SkillsContainers;

            for (int i = 0; i < AllContainers.Count; i++)
                AllContainers[i].GetComponent<Container>().ActiveButtonUp();

            // відняли очки
            pointsCollector.SubPoints(costUp);

            Debug.Log(string.Format("Upgrade skiil: {0}", skillUpIndex));
        }

        if (pointsCollector.GetPoints() < skillsUpgrade.GetValueSkillUp(skillUpIndex))
            ButtonInteractable(false);
    }

    private void Update()
    {
        // виправити
        if (pointsCollector.GetPoints() < skillsUpgrade.GetValueSkillUp(skillUpIndex))
            ButtonInteractable(false);
    }

    public void SetSkillUpIndex(int ind)
    {
        skillUpIndex = ind;
    }

    public void ButtonInteractable(bool f)
    {
        button.interactable = f;
    }

    public void SetCostUp(int i)
    {
        costUp = i;
        costUpText.text = costUp.ToString();
    }
}
