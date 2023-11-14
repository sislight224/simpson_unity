using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLogic : MonoBehaviour
{
    private float fadeDuration = 0.5f; // Add this line for the fade duration

    public string nextScene;

    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void loadRandomIntermission()
    {
        //image.texture = textures[tIdx];
        //audioSource.clip = audioClips[aIdx];
        //audioSource.Play();
        StartCoroutine(ShowIntermissionScene());
    }

    private void SelectNextScene()
    {
        // Randomly select an index (0, 1, 2, 3, 4)
        int randomIndex = Random.Range(0, 4);



        // Determine the selected scene name
        switch (randomIndex)
        {
            case 0:
                nextScene = "sim_school";
                break;
            case 1:
                nextScene = "sim_nuclear_power";
                break;
            case 2:
                nextScene = "sim_bar";
                break;
            case 3:
                nextScene = "sim_town";
                break;
            case 4:
                nextScene = "sim_living";
                break;
        }
    }


    private IEnumerator ShowIntermissionScene()
    {
        SelectNextScene();

        Debug.Log("Loading intermission scene...");

        // Fade out the screen
        yield return StartCoroutine(FadeScreen(1f, 0f, fadeDuration));

        // Load the intermission scene
        SceneManager.LoadScene("IntermissionScene", LoadSceneMode.Additive);

        // Wait for the intermission scene to be fully loaded
        yield return new WaitUntil(() => SceneManager.GetSceneByName("IntermissionScene").isLoaded);

        Debug.Log("Intermission scene loaded. Waiting for audio to finish...");

        // Find IntermissionManager in the newly loaded scene
        GameObject[] rootGameObjects = SceneManager.GetSceneByName("IntermissionScene").GetRootGameObjects();
        IntermissionManager intermissionManager = null;
        foreach (var gameObject in rootGameObjects)
        {
            intermissionManager = gameObject.GetComponent<IntermissionManager>();
            if (intermissionManager != null)
            {
                break;
            }
        }

        // Set the correct image in the IntermissionManager
        if (intermissionManager != null)
        {
            intermissionManager.SetNextSceneImage(nextScene);
        }
        else
        {
            Debug.LogWarning("IntermissionManager not found in the IntermissionScene.");
        }

        // If found, wait for audio to finish playing
        if (intermissionManager != null && intermissionManager.audioSource != null) // Added null check for audioSource
        {
            while (intermissionManager.audioSource.isPlaying)
            {
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("AudioSource not found in the IntermissionScene.");
        }

        Debug.Log("Intermission audio finished. Unloading intermission scene...");

        // Fade in the screen
        yield return StartCoroutine(FadeScreen(0f, 1f, fadeDuration));

        // Unload the intermission scene
        SceneManager.UnloadSceneAsync("IntermissionScene");

        Debug.Log("Intermission scene unloaded. Loading next scene...");

        SceneManager.LoadScene(nextScene);

        // Fade out the screen again while the next scene is loading
        yield return StartCoroutine(FadeScreen(1f, 0f, fadeDuration));
    }

    private IEnumerator FadeScreen(float startAlpha, float endAlpha, float duration)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime = Time.time - startTime;
            float normalizedTime = Mathf.Clamp01(elapsedTime / duration);
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, normalizedTime);

            // Apply the alpha value to your screen fade effect (e.g., a UI panel, post-processing effect, etc.)
            // You can modify this part based on how you implement the screen fade effect in your project

            yield return null;
        }
    }
}
