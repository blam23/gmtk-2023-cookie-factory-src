using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectController : MonoBehaviour
{
    public GameObject LevelPrefab;
    public Camera MainCamera;

    // Start is called before the first frame update
    void Start()
    {
        var x = 200;
        var y = 450;

        for(int i = 0; i < LevelData.LevelCount; i++)
        {
            var data = LevelData.GetLevel(i);

            var levelButton = Instantiate(LevelPrefab, transform);
            levelButton.GetComponentInChildren<Canvas>().worldCamera = MainCamera;

            var levelButtonButton = levelButton.GetComponentInChildren<Button>();
            var pos = MainCamera.ScreenToWorldPoint(new Vector3(x, y, 0));
            pos.z = -2;
            levelButtonButton.transform.position = pos;

            var levelButtonText = levelButtonButton.GetComponentInChildren<TextMeshProUGUI>();
            levelButtonText.text = data.CookieName;

            var cookieSprite = levelButton.GetComponentInChildren<SpriteRenderer>();
            cookieSprite.color = data.CookieColor;
            cookieSprite.transform.position = pos;

            if (data.CookieGlow)
                global.LightsOn(cookieSprite.gameObject);

            x += 350;

            if (x > 1000)
            {
                x = 200;
                y -= 250;
            }

            int ii = i;
            levelButtonButton.onClick.AddListener(() =>
            {
                PlayerPrefs.SetInt("level", ii);
                SceneManager.LoadScene("MainGame");
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
