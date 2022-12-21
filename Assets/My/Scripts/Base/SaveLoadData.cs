using System;

[Serializable]
public class SaveLoadData 
{
	public SettingsData Settings;

    public SaveLoadData()
	{
		Settings = new SettingsData();
    }
}