using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class ShopItems
{
	public string shopScreenName;
	public GameObject shopScreenGameObject;
}
public class ShopManager : MonoBehaviour 
{

	public static ShopManager instance = null;
	public List<ShopItems> shopItems;

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () 
	{
		for(int i =0;i<=shopItems.Count-1;i++)
		{	 
			shopItems [i].shopScreenGameObject.SetActive (false); 
		}
		shopItems [0].shopScreenGameObject.SetActive (true);
	}

	 

	public void ClickButton(string buttonName)
	{

		for(int i =0;i<=shopItems.Count-1;i++)
		{
			if(buttonName == shopItems[i].shopScreenName && shopItems [i].shopScreenGameObject.activeSelf==false )
			{
				shopItems [i].shopScreenGameObject.SetActive (true);
			}

			else
			{
				shopItems [i].shopScreenGameObject.SetActive (false);
			}
		}

	}

	// Update is called once per frame
	void Update ()
	{
	
	}
}
