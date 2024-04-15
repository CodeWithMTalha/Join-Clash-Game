using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplaySc : MonoBehaviour
{
    public static GameplaySc InstanceGamePlay;

    public GameObject CompletePnl;
    // Start is called before the first frame update
    void Start()
    {
        InstanceGamePlay = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Complete Panel
   
    public void CompletePanel()
    {
        CompletePnl.SetActive(true);
    }

    public void ToHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
