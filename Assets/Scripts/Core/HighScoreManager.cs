using System.IO;
using UnityEngine;

[System.Serializable]
public class HighScoreData
{
    public float highScore;
}
public class HighScoreManager : MonoBehaviour
{
    private static string filePath = Application.persistentDataPath + "/highscore.json";

    public static void SaveHighScore(float score)
    {
        HighScoreData data = new HighScoreData();
        data.highScore = score;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filePath, json);
    }

    public static float LoadHighScore()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
            return data.highScore;
        }
        return 0; // Trả về 0 nếu chưa có điểm cao nào
    }
}
