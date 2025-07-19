using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private Animator transitionController;
    [SerializeField]
    private List<RuntimeAnimatorController> controllerList;
    private float waitTime = 3f;
    private int random;
    public Scrollbar progressBar;
    public AudioSource transitionSound;


    public static LoadScene instance;

    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void ChangeScene(string sceneName)
    {
        random = Random.Range(0, controllerList.Count);
        transitionController.runtimeAnimatorController = controllerList[random];
        StartCoroutine(ChangeLevel(sceneName));
    }
    IEnumerator ChangeLevel(string name)
    {
        transitionController.SetBool("Start", true);
        yield return null;
        if (transitionController.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Out"))
            yield break;
 
            
        transitionSound.Play();

        waitTime = transitionController.GetCurrentAnimatorClipInfo(0)[0].clip.length; //Get the length of the animation currently playing
        Debug.Log(transitionController.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        yield return new WaitForSeconds(waitTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        random = Random.Range(0, 3);
        if (random == 0)
        {
            progressBar.gameObject.SetActive(true);
            float duration = 3f;
            float timer = 0f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                float progress = Mathf.Clamp01(timer / duration);
                progressBar.size = progress;

                yield return null;
            }

            progressBar.gameObject.SetActive(false);
        }

        transitionController.SetBool("Start", false);
        transitionController.SetBool("End", true);
        waitTime = transitionController.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(waitTime);

        transitionController.SetBool("End", false);
    }

}
