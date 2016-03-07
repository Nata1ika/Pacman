using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreController
{
	public static System.Action<int> ScoreChanged;

	public static void NewGame()
	{
		currentScore = 0;
	}

	public static void Eat()
	{
		currentScore += 10;
	}

	public static void EatEnemy()
	{
		currentScore += 100;
	}

	public static int currentScore
	{
		get
		{
			if (PlayerPrefs.HasKey(_currentScoreTag))
			{
				return PlayerPrefs.GetInt(_currentScoreTag);
			}
			else
			{
				return 0;
			}
		}
		private set
		{
			PlayerPrefs.SetInt(_currentScoreTag, value);
			if (ScoreChanged != null)
			{
				ScoreChanged(value);
			}
		}
	}

	public static List<int> GetRecords()
	{
		if (PlayerPrefs.HasKey(_recordsTag))
		{
			string record = PlayerPrefs.GetString(_recordsTag);
			if (! string.IsNullOrEmpty(record))
			{
				string[] records = record.Split(',');
				if (records != null)
				{
					List<int> rezult = new List<int>();
					for (int i=0; i<records.Length; i++)
					{
						rezult.Add(int.Parse(records[i]));
					}
					return rezult;
				}
			}
		}
		return null;
	}

	public static void SetRecord(int record)
	{
		List<int> records = GetRecords();
		if (records == null)
		{
			records = new List<int>();
		}
		records.Add(record);
		//если таблица рекордов уже заполнена, уберем минимальное количество очков
		if (records.Count == _countRecords)
		{
			int index = -1;
			for (int i=0; i<records.Count; i++)
			{
				if (index < 0 || records[i] < records[index])
				{
					index = i;
				}
			}
			records.Remove(records[index]);
		}
		//сохраним таблицу рекордов
		string rezult = records[0].ToString();
		for (int i=1; i<records.Count; i++)
		{
			rezult += "," + records[i].ToString();
		}
		PlayerPrefs.SetString(_recordsTag, rezult);
	}

	const string 		_currentScoreTag = "currentScore";
	const string		_recordsTag = "records";
	const int			_countRecords = 5;
}
