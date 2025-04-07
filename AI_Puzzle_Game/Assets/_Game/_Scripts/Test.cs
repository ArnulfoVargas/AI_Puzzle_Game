using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    [SerializeField] private string tutorial = "Tutorial";
    [SerializeField] private string mainMenu = "MainMenu";
    [SerializeField] private Slider progressBar;

    void Start()
    {
        var firstPlay = ES3.Load<bool>("first_play", true);
        var op = SceneManager.LoadSceneAsync(firstPlay ? tutorial : mainMenu);

        var coroutine = LoadingScene(op);
        StartCoroutine(coroutine);
    }
    
    IEnumerator LoadingScene(AsyncOperation op) {
        yield return new WaitUntil(() => {
            return op.isDone;
        });
    }
}
