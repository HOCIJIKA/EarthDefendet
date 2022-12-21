using System;
using System.Collections.Generic;

[Serializable]
public class SettingsData
{
	public bool NotFirsStart;

	#region Settings
	public bool toggleFoneMusic;
	public bool toggleSoundEffect;
	public bool toggleVibro;
	#endregion

	#region Skills
	public bool Autoplay;
	public bool OnSatellite;
	#endregion

	#region Panel free points on session
	public int Day;
	public int Hour;
	public int Minute;
	public int Second;

	public int countMultipleFreePoints;
	#endregion

	/// <summary>
	/// Всі очки гравця.
	/// </summary>
	public int AllPoints;

	#region Робота з контейнерами
	/// <summary>
	/// Список активованих контейнерів
	/// </summary>
	public List<bool> IsActiveContainers;

	/// <summary>
	/// Кількість контейнерів
	/// </summary>
	public List<int> CountContainers;

	/// <summary>
	/// Який контейнер який містить скіл. ("[0] == 1" Перший контейнер містиь другий скіл).
	/// </summary>
	public List<int> ContainerNumberSkill;

	/// <summary>
	/// Які скіли випрані на панелі.
	/// </summary>
	public List<bool> IsScillSelected;

	#endregion

	#region Вдосконалення скілів і вартості
	/// <summary>
	/// Список зі стартовою вартістю апгрейда скілів.
	/// </summary>
	public List<int> CostSkillsUp;

	/// <summary>
	/// Поточна вартість апгрейду скілів
	/// </summary>
	public List<int> CurrentCostSkillsUp;

	/// <summary>
	/// Вартість викоритання скілів
	/// </summary>
	public List<int> CostSkillsUse;

	/// <summary>
	/// Рівень прокачки скілів.
	/// </summary>
	public List<int> LvlsSkills;
	#endregion

	/// <summary>
	/// Поточна хвиля(рівень). Починається з 1
	/// </summary>
	public int CurrentLvl;
}