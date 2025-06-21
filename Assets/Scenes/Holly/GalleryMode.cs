using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GalleryMode : MonoBehaviour
{
    public float timeoutAfterSecondsOfInactivity = 60.0f;
    public float timeoutHintTime = 10.0f;
    private float idleTimer = 0.0f;
    public Volume resettingHintVolume;
    public CanvasGroup canvasGroup;
    private bool gameStarted = false;
    private bool fadedIn = false;
    void Start()
    {
        resettingHintVolume.weight = 1.0f;
        canvasGroup.alpha = 0.0f;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.anyKeyDown || Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") != 0.0f)
        {
            idleTimer = 0.0f;
            gameStarted = true;
        }
        if (!fadedIn)
        {
            resettingHintVolume.weight = Mathf.Lerp(resettingHintVolume.weight, 0.0f, Time.deltaTime);
            if (!gameStarted)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1.0f, Time.deltaTime);
            }
            if (resettingHintVolume.weight < .001f)
            {
                fadedIn = true;
                resettingHintVolume.weight = 0.0f;
            }
        }

        if (!gameStarted)
        {
            return;
        }
        idleTimer += Time.deltaTime;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0.0f, Time.deltaTime * 3.0f);


        if (idleTimer > timeoutAfterSecondsOfInactivity - timeoutHintTime)
        {
            float hintPercentage = (timeoutHintTime - (timeoutAfterSecondsOfInactivity - idleTimer)) / timeoutHintTime;
            resettingHintVolume.weight = Mathf.Pow(hintPercentage,2.0f);
        }

        if(idleTimer > timeoutAfterSecondsOfInactivity)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
