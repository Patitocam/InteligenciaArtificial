using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class StartMenu : MonoBehaviour
{
 public void OnStartClicked()
    {
        SceneManager.LoadScene("Game");

    }

    public void OnExitClick()
    {
#if UNITY_EDITOR    
        UnityEditor.EditorApplication.isPlaying = false;

#endif
        Application.Quit(); 
    }
}
