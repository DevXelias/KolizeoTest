using UnityEngine;

public class ChangeBtn : MonoBehaviour
{
    public string sceneName;
    public void Change()
    {
        if(LoadScene.instance != null)
        {
            LoadScene.instance.ChangeScene(sceneName);
        }
    }
}
