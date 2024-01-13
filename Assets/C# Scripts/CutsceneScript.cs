using GlobalEnums;
using HutongGames.PlayMaker.Actions;
using Modding;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class CutsceneScript : MonoBehaviour
{
    public GameObject Blanker;
    public bool CanSkip = false;

    public bool SkipPrompt;
    public bool Skipping;
    public bool Skipped;
    public float SecondsForSkip;

    public TextMeshProUGUI skipText;
    public Image Haze;
    public Image Haze2;

    public AudioSource SoundOfSkipping;

    private Coroutine CurrentSkipRoutine;

    private bool didInput = false;

    private void Start()
    {
        StartCoroutine(Cutscene());
        skipText.color = new Color(1, 1, 1, 0);
        Haze.color = new Color(1, 1, 1, 0);
        Haze2.color = new Color(1, 1, 1, 0);
    }

    public static IEnumerator FadeImageCoroutine(Image image, float duration, float alpha)
    {
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            image.color = Color.Lerp(image.color, new Color(image.color.r, image.color.g, image.color.b, alpha), currentTime / duration);
            yield return null;
        }
        yield break;
    }

    private IEnumerator FadeInTextCoroutine()
    {
        while (skipText.color.a < 1)
        {
            skipText.color = new Color(1, 1, 1, skipText.color.a + 0.005f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator FadeOutTextCoroutine()
    {
        while (skipText.color.a > 0)
        {
            skipText.color = new Color(1, 1, 1, skipText.color.a - 0.005f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    private IEnumerator SkipRoutine()
    {
        yield return new WaitForSeconds(SecondsForSkip);
        CanSkip = false;
        Blanker.SetActive(true);
        FindObjectOfType<VideoPlayer>().Stop();
        StartCoroutine(FadeOutTextCoroutine());
        Skipped = true;
    }

    private IEnumerator SkipPromptRoutine()
    {
        yield return new WaitForSeconds(1);
        StartCoroutine(FadeOutTextCoroutine());
    }

    private void Update()
    {
        if (CanSkip)
        {
            if (Input.anyKeyDown && didInput == false)
            {
                didInput = true;
                if (!SkipPrompt && !Skipping && !Skipped)
                {
                    SkipPrompt = true;
                    StartCoroutine(FadeInTextCoroutine());
                    StartCoroutine(SkipPromptRoutine());
                }
                if (SkipPrompt)
                {
                    SkipPrompt = false;
                    CurrentSkipRoutine = StartCoroutine(SkipRoutine());
                    Skipping = true;
                    SoundOfSkipping.Play();
                }
                
            }
            else
            {
                didInput = false;
            }
        }
    }

    private IEnumerator Cutscene()
    {
        Blanker.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        Blanker.SetActive(false);

        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Knight_Pickup", LoadSceneMode.Additive);
        asyncOperation.allowSceneActivation = false;

        AsyncOperation asyncWorldLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Tutorial_01", LoadSceneMode.Single);
        asyncWorldLoad.allowSceneActivation = false;

        //StartCoroutine(WaitForLoads(asyncOperation, asyncWorldLoad));

        for (int i = 0; i < 37; i++)
        {
            yield return new WaitForSeconds(1);
            if (Skipped)
            {
                break;
            }
        }
        

        asyncOperation.allowSceneActivation = true;
        yield return asyncOperation;

        GameManager.instance.OnWillActivateFirstLevel();
        GameManager.instance.nextSceneName = "Tutorial_01";


        asyncWorldLoad.allowSceneActivation = true;

        yield return asyncWorldLoad;

        AsyncOperation asyncUnload = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameObject.scene);

        GameManager.instance.SetupSceneRefs(refreshTilemapInfo: true);
        GameManager.instance.BeginScene();
        GameManager.instance.OnNextLevelReady();
        GameCamerasHK.instance.EnableImageEffects(isGameplayLevel: true, isBloomForced: false);
    }
}
