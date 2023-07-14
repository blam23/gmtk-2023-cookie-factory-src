using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mash : MonoBehaviour
{
    // Singleton shiz
    public static mash Instance;

    // Editable Fields
    public int EventRandomTimeMin = 5;
    public int EventRandomTimeMax = 20;
    public ulong Target = 100;
    public KeyCode[] MashableKeys = new KeyCode[] 
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F,
        KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L,
        KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R,
        KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X,
        KeyCode.Y, KeyCode.Z
    };

    public keybutton[] Keys;

    public string[] EventWords = new string[]
    {
        "cat",
        "jam",
        "counter",
        "boss",
        "uranium",
        "close",
        "open",
        "cookie",
    };

    public List<KeyCode> KeysDown = new();

    // Sockets
    public GameObject CounterObject;
    public GameObject EventObject;
    public lever ActionLever;
    public clickbutton BlueButton;
    public clickbutton GreenButton;
    public clickbutton RedButton;
    public valve Valve;
    public GameObject ClockHand;
    public GameObject BeltHolder;
    public GameObject BeltEnd;
    public GameObject BeltPrefab;
    public GameObject CookiePrefab;
    public Camera cam;

    public Texture2D HandOpenTexture;
    public Texture2D HandHoverGrabTexture;
    public Texture2D HandHoverPressTexture;
    public Texture2D HandGrabTexture;
    public Texture2D HandClickTexture;

    public TextMeshProUGUI RecipeText;
    public GameObject RecipeBackground;
    public GameObject FadeToBlack;
    public GameObject[] DisableOnFade;
    public GameObject[] ShowOnEvent;

    public AudioSource[] KeySounds;
    public AudioSource RecipeSound;
    public AudioSource BeltSound;
    public AudioSource FadeMusic;
    public AudioSource EventSound;

    // Components
    private TMPro.TextMeshProUGUI _counterTextComp;
    private TMPro.TextMeshProUGUI _eventTextComp;

    private List<GameObject> _belts = new();
    private List<GameObject> _cookies = new();

    //
    // State
    //
    ulong _count = 0;

    private Color _cookieColor = Color.white;
    private bool _cookieGlow = false;

    // Action State
    private string _cookieName = "Nuclear Cookie";
    private List<RecipeStep> _actionLoop;

    private int _actionIndex = 0;
    private BaseAction _currentAction = null;

    // Event State
    private Stack<string> _queue = new();
    bool _inEvent = false;
    string _eventText = "";
    string _nextKey = "";
    bool _eventTextKeyDown = false;
    float _nextEventTime = 0f;
    float _nextEventDiff = 30f;

    // Timer
    float _timer = 120f;
    bool _started = false;
    bool _ended = false;
    bool _fadeIn = false;

    //
    // Helpers
    //

    public void LoadLevel()
    {
        var level = PlayerPrefs.GetInt("level");
        var levelData = LevelData.GetLevel(level);

        SetActionLoop(levelData.Actions);
        _cookieName = levelData.CookieName;
        _cookieColor = levelData.CookieColor;
        _cookieGlow = levelData.CookieGlow;

        PlayerPrefs.SetInt("quota", levelData.Quota);
        PlayerPrefs.SetInt("cookies", 0);
    }

    public void FadeIn()
    {
        FadeToBlack.SetActive(true);

        FadeToBlack.LeanColor(new Color(0, 0, 0, 0), 1f)
            .setEaseInCubic()
            .setOnComplete(() =>
                {
                    foreach (var obj in DisableOnFade)
                        obj.SetActive(true);
                });
    }

    public void FadeOut()
    {
        EndEvent();
        _currentAction.ActionEnded();

        FadeMusic.Play();
        _eventTextComp.text = "";
        FadeToBlack.SetActive(true);

        FadeToBlack.LeanColor(new Color(0, 0, 0, 255), 2f)
            .setEaseInCubic()
            .setOnComplete(
                () => LeanTween.delayedCall(2f, () => SceneManager.LoadScene("Score")
            ));

        foreach(var obj in DisableOnFade)
            obj.SetActive(false);
    }

    public void ShowRecipe()
    {
        RecipeBackground.SetActive(true);
        RecipeSound.Play();
        RecipeText.gameObject.SetActive(true);
        RecipeText.text = GenerateRecipe(_cookieName, _actionLoop);
    }

    public void HideRecipe()
    {
        RecipeSound.Play();
        RecipeBackground.SetActive(false);
        RecipeText.text = "";
        RecipeText.gameObject.SetActive(false);
    }

    internal void ToggleRecipeBook()
    {
        if (RecipeBackground.activeSelf)
            HideRecipe();
        else
            ShowRecipe();
    }

    public void SetActionLoop(List<RecipeStep> actions)
    {
        _actionLoop = actions;

        foreach(var action in _actionLoop)
        {
            if (action.StepType == StepType.Lever)
                action.Lever = ActionLever;
            else if (action.StepType == StepType.BlueButton)
                action.Button = BlueButton;
            else if (action.StepType == StepType.GreenButton)
                action.Button = GreenButton;
            else if (action.StepType == StepType.RedButton)
                action.Button = RedButton;
            else if (action.StepType == StepType.Valve)
                action.Valve = Valve;
            else if (action.StepType == StepType.Key)
            {
                foreach(var key in Keys)
                {
                    if (key.Key == action.Key)
                    {
                        action.KeyButton = key;
                        break;
                    }
                }
            }
        }
    }

    public static string GenerateRecipe(string name, List<RecipeStep> steps)
    {
        StringBuilder sb = new();

        sb.Append("Recipe for ");
        sb.Append(name);
        sb.AppendLine(":");

        foreach(var step in steps)
        {
            sb.AppendLine($" - {step}");
        }

        return sb.ToString();
    }

    public void SpawnCookie()
    {
        var cookie = Instantiate(CookiePrefab, BeltHolder.transform);
        cookie.transform.localPosition = new Vector3(0,0,-1f);

        cookie.GetComponent<SpriteRenderer>().color = _cookieColor;

        if (_cookieGlow)
            global.LightsOn(cookie);

        _cookies.Add(cookie);
    }

    public void NextAction()
    {
        var step = _actionLoop[_actionIndex];
        _currentAction = step.CreateActionFromStep();
        _currentAction.ActionStart();

        _actionIndex++;
        _actionIndex %= _actionLoop.Count;

        if (_actionIndex == 0)
        {
            SpawnCookie();
        }
    }

    public void PushWordEvent(string eventString)
    {
        _queue.Push(eventString);
    }
    public void CreateRandomWordEvent() => PushWordEvent(EventWords[Random.Range(0, EventWords.Length)]);

    public bool NextEvent()
    {
        if (_queue.Count == 0)
            return false;

        foreach (var obj in ShowOnEvent)
            obj.SetActive(true);

        _inEvent = true;
        _eventText = _queue.Pop();
        _eventTextComp.text = _eventText;
        NextLetter();

        EventSound.Play();

        return true;
    }

    public void NextLetter()
    {
        _nextKey = _eventText.Length == 0 ? "" : _eventText.Substring(0, 1);
        if (_nextKey == " ")
            _nextKey = "space";
    }

    public void EndEvent()
    {
        _inEvent = false;

        foreach (var obj in ShowOnEvent)
            obj.SetActive(false);
    }

    public void UpdateEventText()
    {
        if (_eventText.Length == 0)
            return;

        if (_eventText.Length == 1)
            EndEvent();

        _eventText = _eventText.Substring(1, _eventText.Length - 1);
        NextLetter();

        _eventTextComp.text = _eventText;
    }


    public void UpdateCount()
    {
        _counterTextComp.text = _count.ToString();
    }

    public void GenerateBelts()
    {
        float width = 0.5f;
        float x = width;
        float y = 0;
        float z = -0.1f;
        float endX = BeltEnd.transform.position.x - (width * 4);

        Vector3 offset = BeltHolder.transform.position;

        while (x > endX)
        {
            y = Random.Range(-0.02f, 0.02f);

            var belt = Instantiate(BeltPrefab, BeltHolder.transform);
            belt.transform.position = new Vector3(x, y, z) + offset;

            x -= width;
            z += 0.01f;

            _belts.Add(belt);
        }
    }

    bool rotating = false;

    public void RotateBelts()
    {
        var width = 0.5f;
        var limit = BeltEnd.transform.position.x - width;
        if (rotating)
            return;

        BeltSound.Play();

        rotating = true;

        foreach (var belt in _belts)
        {
            belt.LeanMoveX(belt.transform.position.x - width, 0.1f)
                .setEaseOutCubic()
                .setOnComplete(() =>
                {
                    rotating = false;
                    if (belt.transform.position.x < limit)
                    {
                        belt.transform.position =
                            new Vector3(BeltHolder.transform.position.x + width,
                                        belt.transform.position.y,
                                        belt.transform.position.z);
                    }
                });
        }

        foreach (var cookie in _cookies)
        {
            cookie.LeanMoveX(cookie.transform.position.x - width, 0.1f)
               .setEaseOutCubic()
               .setOnComplete(() =>
               {
                   rotating = false;
                   if (cookie.transform.position.x < limit)
                   {
                       _cookies.Remove(cookie);
                       Destroy(cookie);
                       _count++;
                       PlayerPrefs.SetInt("cookies", (int)_count);
                       UpdateCount();
                   }
               });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        _counterTextComp = CounterObject.GetComponent<TMPro.TextMeshProUGUI>();
        _eventTextComp = EventObject.GetComponent<TMPro.TextMeshProUGUI>();

        _eventTextComp.text = "";

        GenerateBelts();

        LoadLevel();

        NextAction();

        ShowRecipe();

        foreach (var obj in ShowOnEvent)
            obj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (_ended)
            Cursor.SetCursor(HandOpenTexture, Vector2.zero, CursorMode.ForceSoftware);
        else if (global.grabbing)
            Cursor.SetCursor(HandGrabTexture, new Vector2(20, 40), CursorMode.ForceSoftware);
        else if(global.clicking)
            Cursor.SetCursor(HandClickTexture, new Vector2(0, 40), CursorMode.ForceSoftware);
        else if (global.hoverGrab)
            Cursor.SetCursor(HandHoverGrabTexture, new Vector2(20, 40), CursorMode.ForceSoftware);
        else if (global.hoverPress)
            Cursor.SetCursor(HandHoverPressTexture, new Vector2(20, 20), CursorMode.ForceSoftware);
        else 
            Cursor.SetCursor(HandOpenTexture, Vector2.zero, CursorMode.ForceSoftware);

        if (_cookies.Count > 0)
        {
            RotateBelts();
        }
        else if (_ended && _fadeIn)
        {
            FadeOut();
            _fadeIn = false;
        }
        else
        {
            BeltSound.Stop();
        }

        if (_ended)
            return;

        if (!_fadeIn)
        {
            FadeIn();
            _fadeIn = true;
        }

        if (_started)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _started = false;
                EndDay();

                _currentAction?.ActionEnded();
            }

            if (_timer <= _nextEventTime)
            {
                _nextEventTime -= _nextEventDiff;
                if (_nextEventTime < 10f)
                    _nextEventTime = -10f;

                CreateRandomWordEvent();
                NextEvent();
            }

            ClockHand.transform.rotation = Quaternion.AngleAxis((_timer / 120f * 360f) - 90f, Vector3.forward);
        }

        if (_inEvent)
        {
            if (_eventTextKeyDown)
            {
                if (!Input.GetKeyDown(_nextKey))
                {
                    _eventTextKeyDown = false;
                    UpdateEventText();
                }
            }
            else
            {
                if (Input.GetKeyDown(_nextKey))
                {
                    _eventTextKeyDown = true;
                }
            }
        }
        else
        {

            if (_currentAction != null)
            {
                _currentAction.CheckComplete();
                if (_currentAction.Complete)
                {
                    if (!_started)
                    { 
                        _started = true;
                        _nextEventTime = _timer - _nextEventDiff;
                    }

                    if (RecipeBackground.activeSelf)
                        HideRecipe();

                    _currentAction.ActionEnded();
                    NextAction();
                }
            }
        }
    }

    private void EndDay()
    {
        _ended = true;
    }

    internal void PlayKeyboardClick()
    {
        if (KeySounds.Length == 0)
            return;

        KeySounds[Random.Range(0, KeySounds.Length)].Play();
    }
}
