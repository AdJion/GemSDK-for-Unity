using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class UserScore : IComparable<UserScore> {
	public float LevelTime;
	public string UserName;

	public int CompareTo(UserScore another) {
		return LevelTime.CompareTo (another.LevelTime);
	}
}

[System.Serializable]
public class LevelProgress
{
	public List<UserScore> scores = new List<UserScore>();

	public UserScore AddScore(float score) {
		if (scores.Count < 5 || score < scores [4].LevelTime) {
			UserScore us = new UserScore() {
				UserName = "Anonymous", 
				LevelTime = score
			};

			scores.Add(us);
			scores.Sort();

			if(scores.Count == 6) scores.RemoveAt(5);

			return us;
		}
		return null;
	}
}

public class ProgressManager
{
	private static string fname = @"/scores.gd";
	public Dictionary<String, LevelProgress> levels;

	public ProgressManager() {
		load ();
		if(levels == null) levels = new Dictionary<String, LevelProgress>();
	}

	public void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + fname);
		bf.Serialize(file, levels);
		file.Close();	
	}

	private void load() {
		if (File.Exists (Application.persistentDataPath + fname)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + fname, FileMode.Open);
			levels = (Dictionary<String, LevelProgress>)bf.Deserialize (file);
			file.Close ();
		}
	}
}

