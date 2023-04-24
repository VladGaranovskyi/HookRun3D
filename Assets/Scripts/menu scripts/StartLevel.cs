using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLevel : MonoBehaviour
{
    private StartLevel.PassedLevels passed_levels = new StartLevel.PassedLevels();

    private class PassedLevels
    {
        public List<int> pass_levels = new List<int>();
    }

    private bool IsPassed = false;

    private void Start()
    {
        PlayerPrefs.SetFloat("Coins_Game", 0f);
        PlayerPrefs.DeleteKey("time-code");
        if (PlayerPrefs.HasKey("Level"))
        {
            if (PlayerPrefs.HasKey("pass"))
            {
                if (PlayerPrefs.HasKey("IsLevelNew"))
                {
                    if (PlayerPrefs.GetString("IsLevelNew") != "new")
                    {
                        if(PlayerPrefs.GetInt("CurrentLevel") % 10 == 0)
                        {
                            PlayerPrefs.SetInt("Level", 10);
                            PlayerPrefs.SetString("IsLevelNew", "new");
                            passed_levels.pass_levels.Add(10);
                            PlayerPrefs.SetString("PassedLevels", JsonUtility.ToJson(passed_levels));
                        }
                        else
                        {
                            passed_levels = JsonUtility.FromJson<PassedLevels>(PlayerPrefs.GetString("PassedLevels"));
                            while (true)
                            {
                                int i = Random.Range(1, 9);
                                if (passed_levels.pass_levels.Count == 0)
                                {
                                    PlayerPrefs.SetInt("Level", i);
                                    PlayerPrefs.SetString("IsLevelNew", "new");
                                    passed_levels.pass_levels.Add(i);
                                    PlayerPrefs.SetString("PassedLevels", JsonUtility.ToJson(passed_levels));
                                    break;
                                }
                                else
                                {
                                    for (int j = 0; j < passed_levels.pass_levels.Count; j++)
                                    {
                                        if (i == passed_levels.pass_levels[j])
                                        {
                                            IsPassed = true;
                                            break;
                                        }
                                    }
                                    if (IsPassed)
                                    {
                                        IsPassed = false;
                                        continue;
                                    }
                                }
                                PlayerPrefs.SetInt("Level", i);
                                PlayerPrefs.SetString("IsLevelNew", "new");
                                if (passed_levels.pass_levels.Count > 5)
                                {
                                    passed_levels.pass_levels.RemoveAt(0);
                                    passed_levels.pass_levels.Add(i);
                                    PlayerPrefs.SetString("PassedLevels", JsonUtility.ToJson(passed_levels));
                                }
                                else
                                {
                                    passed_levels.pass_levels.Add(i);
                                    PlayerPrefs.SetString("PassedLevels", JsonUtility.ToJson(passed_levels));
                                }
                                break;
                            }
                        }
                        SceneManager.LoadScene($"Level {PlayerPrefs.GetInt("Level")}");
                    }
                    else
                    {
                        SceneManager.LoadScene($"Level {PlayerPrefs.GetInt("Level")}");
                    }
                }
                else
                {
                    PlayerPrefs.SetString("PassedLevels", JsonUtility.ToJson(passed_levels));
                    passed_levels = JsonUtility.FromJson<PassedLevels>(PlayerPrefs.GetString("PassedLevels"));
                    while (true)
                    {
                        int i = Random.Range(1, 9);
                        if (passed_levels.pass_levels.Count == 0)
                        {
                            PlayerPrefs.SetInt("Level", i);
                            PlayerPrefs.SetString("IsLevelNew", "new");
                            passed_levels.pass_levels.Add(i);
                            PlayerPrefs.SetString("PassedLevels", JsonUtility.ToJson(passed_levels));
                            break;
                        }
                        else
                        {
                            for (int j = 0; j < passed_levels.pass_levels.Count; j++)
                            {
                                if (i == passed_levels.pass_levels[j])
                                {
                                    IsPassed = true;
                                    break;
                                }
                            }
                            if (IsPassed)
                            {
                                continue;
                            }
                            PlayerPrefs.SetInt("Level", i);
                            PlayerPrefs.SetString("IsLevelNew", "new");
                            if (passed_levels.pass_levels.Count > 5)
                            {
                                passed_levels.pass_levels.RemoveAt(0);
                                passed_levels.pass_levels.Add(i);
                                PlayerPrefs.SetString("PassedLevels", JsonUtility.ToJson(passed_levels));
                            }
                            else
                            {
                                passed_levels.pass_levels.Add(i);
                                PlayerPrefs.SetString("PassedLevels", JsonUtility.ToJson(passed_levels));
                            }
                            break;
                        }
                    }
                    SceneManager.LoadScene($"Level {PlayerPrefs.GetInt("Level")}");
                }
            }
            else
            {
                SceneManager.LoadScene($"Level {PlayerPrefs.GetInt("Level")}");
            }
        }
        else
        {
            PlayerPrefs.SetInt("Level", 1);
            SceneManager.LoadScene($"Level {PlayerPrefs.GetInt("Level")}");
        }
    }
}
