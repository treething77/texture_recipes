using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void goBack()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    }

    public void refresh()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void goExample1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("soccer");
    }

    public void goExample2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Spaceships");
    }

    public void goExample3()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Spaceships2");
    }
}
