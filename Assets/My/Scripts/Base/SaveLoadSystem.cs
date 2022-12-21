using UnityEngine;

public class SaveLoadSystem
{
	private const string DataKey = "Data";

    private SaveLoadData data;

    public SaveLoadData Data => data;

    public SaveLoadSystem()
	{
        var ppData = PlayerPrefs.GetString(DataKey, "");

        if (string.IsNullOrEmpty(ppData))
            data = new SaveLoadData();
        else
            data = JsonUtility.FromJson<SaveLoadData>(ppData);
    }

    public void Save()
	{
        if (data == null)
			return;

        PlayerPrefs.SetString(DataKey, JsonUtility.ToJson(data));
    }
}