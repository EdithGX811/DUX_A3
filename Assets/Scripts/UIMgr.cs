using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMgr : Singleton<UIMgr>
{
    public Text CurrentSceneName;
    public Text CurrentScore;
    public TextMeshProUGUI LoadPrecess;
    public Slider LoadSlider;
    public GameObject LoadCanvas;
    FirstPersonController firstPerson;
    int score;
    int needScore;
    public Text TimeText;
    public Text TipText;
    public AudioSource BGMusic;
    public AudioSource EffectMusic;
    float t;
    public AudioClip GameFail;
    public AudioClip GameWin;
    public AudioClip Eat;
    public AudioClip Btn;
    public AudioClip Hit;
    public AudioClip BadFood_Clip;
    int Game_State;
    // Start is called before the first frame update
    void Start()
    {
        Game_State = 0;
        t = 60;
        BGMusic = GameObject.Find("BGAudio").GetComponent<AudioSource>();
        EffectMusic = GameObject.Find("EffectAudio").GetComponent<AudioSource>();
        firstPerson = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                break;
            case 1:
                needScore = 15;
                Init();
                break;
            case 2:
                needScore = 10;
                Init();
                break;
            case 3:
                needScore = 6;
                Init();
                break;
            case 4:
                needScore = 15;
                Init();
                break;
            case 5:
                needScore = 15;
                Init();
                break;
            default:
                break;
        }
   
        EventManager.Instance.AddEventListener("AddScore", AddScore);
        EventManager.Instance.AddEventListener("Start_BadFood", Start_BadFood);
    }

    public void Init() {


        CurrentSceneName.text = "CurrentScene:" + SceneManager.GetActiveScene().name;
        GameObject.Find("MainCanvas").transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {


            LoadHome();
        
        });
        score = 0;
        CurrentScore.text = "Score:" + score + "/" + needScore;
    }
    private void OnDestroy()
    {
        EventManager.Instance.RemoveEvent("AddScore", AddScore);
        EventManager.Instance.RemoveEvent("Start_BadFood", Start_BadFood);
    }
    public void AddScore()
    {
        score += 1;
        Player_EffectMusic(Eat);
        CurrentScore.text = "Score:" + score + "/" + needScore;
    }

    public void Player_EffectMusic(AudioClip clip)
    {
        EffectMusic.clip = clip;
        EffectMusic.Play();
    }
    // 加载上一个场景
    public void LoadPreviousScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int previousIndex = Mathf.Max(1, currentIndex - 1); // 确保不会小于0
        StartCoroutine(LoadSceneAsync(previousIndex));
    }

    // 加载下一个场景
    public void LoadNextScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = Mathf.Min(SceneManager.sceneCountInBuildSettings - 1, currentIndex + 1); // 确保不会超出场景总数
        StartCoroutine(LoadSceneAsync(nextIndex));
    }
    public void LoadHome()
    {
        StartCoroutine(LoadSceneAsync(0));
    }
    //开始界面用选择关卡按钮绑定的按键
    public void LoadLevelScene(int index)
    {
        StartCoroutine(LoadSceneAsync(index));
    }
    public void Start_BadFood()
    {
        StopAllCoroutines();
        StartCoroutine(BadFood());
    }
    IEnumerator BadFood()
    {
        yield return new WaitForSeconds(0.5f);
        Player_EffectMusic(BadFood_Clip);
        TipText.text = "Food poisoning prevents movement for 5 seconds";
        firstPerson.enabled = false;
        yield return new WaitForSeconds(4.5f);
        TipText.text = "Poisoning relief";
        firstPerson.enabled = true;
        yield return new WaitForSeconds(1f);
        TipText.text = "";
    }
    IEnumerator LoadSceneAsync(int index)
    {
        LoadCanvas.SetActive(true);
        // 异步加载场景，并获取AsyncOperation对象
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);
        operation.allowSceneActivation = false;
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            LoadSlider.value = progress * 100;
            LoadPrecess.text = "Loading..." + progress * 100  + "%";
            // 当加载进度接近完毕时，激活场景
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void WinGame()
    {
        if (Game_State <= 0)
        {
            TipText.text = "Game successful Go back to the homepage";
            Player_EffectMusic(GameWin);
            TipText.raycastTarget = true;
            TipText.maskable = true;
            Invoke("LoadHome", 3);
            Game_State += 1;
        }
    }


    public void FailGame()
    {
        if (Game_State <= 0)
        {
            TipText.text = "Game failed Go back to the homepage";
            Player_EffectMusic(GameFail);
            TipText.raycastTarget = true;
            TipText.maskable = true;
            Invoke("LoadHome", 3);
            Game_State += 1;

        }
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (t > 0)
            {
                t -= Time.deltaTime;
                TimeText.text = "Time:" + t.ToString("F1");
                if (score == needScore )
                {
                    WinGame();
                }
            }
            else
            {
                t -= Time.deltaTime;
                FailGame();
            }
        }
    }
}
