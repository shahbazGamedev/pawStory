using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class StoreItems
{
	public string itemName;
	public int itemIndex;
	public int itemCost;
	public int itemUnlockLevel;
	//public GameObject itemGameObject;
	public bool isPurchased;
	public bool isEquipped;
}

public class StoreManager : MonoBehaviour 
{
	public static StoreManager instance = null;

	public List<StoreItems> storeItems;

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
		 
	}


	void OnSceneEnd()
	{

	}

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	public void CheckItemStatus(int index)
	{
		for(int i =0;i<storeItems.Count;i++)
		{
			if(storeItems[index].isPurchased == true)
			{
				Debug.Log ("Item already purchased");
			}
		}
	}

	public void BuyItem(int index)
	{
		if(storeItems[index].itemCost<=GlobalVariables.totalCoins )
		{
			if(storeItems[index].itemUnlockLevel>= PuppyManager.instance.beagle.dogLevel)
			{	
				storeItems [index].isPurchased = true;
			}
		}

		else
		{
			// Call in app purchase screen
		}
		

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
