using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI QuotaText;
    public TextMeshProUGUI BossText;

    public string[] UnderParText;
    public string[] AverageText;
    public string[] OverParText;

    // Start is called before the first frame update
    void Start()
    {
        var cookies = PlayerPrefs.GetInt("cookies");
        var quota = PlayerPrefs.GetInt("quota");

        var arr = AverageText;
        if (cookies < quota)
        {
            arr = UnderParText;
        }
        else if (cookies - 5 > quota)
        {
            arr = OverParText;
        }

        var bossText = arr[Random.Range(0, arr.Length)];
        ScoreText.text = $"Cookies Produced:\n{cookies}";
        QuotaText.text = $"Quota:\n{quota}";
        BossText.text = $"{bossText}";
    }

    public void ReplayLevel()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void NextLevel()
    {
        var level = PlayerPrefs.GetInt("level");
        level++;
        PlayerPrefs.SetInt("level", level);

        if (level >= LevelData.LevelCount)
            SceneManager.LoadScene("EndGame");
        else
            SceneManager.LoadScene("MainGame");
    }
}
