
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public enum LoadingScreen
{
	Default,
	Screen1,
	Screen2
}

public class LoadSceneManager : MonoBehaviour 
{


	public static LoadSceneManager instance = null;

	public string[] loadingScreenTipText;
	public GameObject def, screen1, screen2;
	public Slider loadingBar;
	public Text loadingScreenTip;
	public Text touchToContinue;

	AsyncOperation async;

	void OnEnable()
	{
		
		EventManager.SceneStart += OnSceneStart;
		EventManager.SceneEnd += OnSceneEnd;
	}


	void OnDisable()
	{
		EventManager.SceneStart -= OnSceneStart;
		EventManager.SceneEnd -= OnSceneEnd;
	}


	void OnSceneStart()
	{
		instance = this;
	}


	void OnSceneEnd()
	{

	}
	void Awake()
	{
		 
	}


	// Use this for initialization
	void Start () 
	{
		touchToContinue.gameObject.SetActive (false);
		loadingScreenTip.gameObject.SetActive (false);
	}

	public void LoadSceneAsyn(string sceneName,LoadingScreen loadingScreen)
	{
		StartCoroutine (LoadScene(sceneName,loadingScreen));
	}

	IEnumerator LoadScene(string sName,LoadingScreen ls)
	{
		yield return new WaitForSeconds (1);

		async = SceneManager.LoadSceneAsync (sName);
		async.allowSceneActivation = false;
		while(!async.isDone)
		{
			switch(ls)
			{

			case LoadingScreen.Screen1:
				screen1.SetActive (true);
				break;

			case LoadingScreen.Screen2:
				screen2.SetActive (true);
				break;

			default:
				def.SetActive (true);
				break;
			}

			loadingScreenTip.text = loadingScreenTipText[ Random.Range (0, loadingScreenTipText.Length)];

			loadingBar.value = async.progress;
			if(async.progress==0.9f)
			{
				Debug.Log ("SceneLoaded Add a text to continue");
				touchToContinue.gameObject.SetActive (true);
				if(Input.GetMouseButtonDown (0))
				async.allowSceneActivation = true;
			}

			Debug.Log (async.progress);
			yield return null;
		}
	}



}
