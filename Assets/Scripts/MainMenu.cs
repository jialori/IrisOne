using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject loadingInterface;
    public Image loadingProgressBarFill;

    private List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public void OnStartGameButtonPressed()
    {
        HideMenu();
        ShowLoadingScreen();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Game"));
        StartCoroutine(LoadingScreen());
    }

    private void HideMenu()
    {
        menu.SetActive(false);
    } 

    private void ShowLoadingScreen()
    {
        loadingInterface.SetActive(true);
    }

    private IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        foreach (AsyncOperation sceneLoad in scenesToLoad)
        {
            while (!sceneLoad.isDone)
            {
                totalProgress += sceneLoad.progress;
                loadingProgressBarFill.fillAmount = totalProgress / scenesToLoad.Count;
                yield return null;
            }
        }

    }
}
