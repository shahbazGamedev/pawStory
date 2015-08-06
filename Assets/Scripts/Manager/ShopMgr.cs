using UnityEngine;
using System.Collections.Generic;

public enum ShopItemType
{
	Decoration,

}


public class ShopItem
{
	public ShopItem(){

	}

	string uid;
	bool isLocked;
	string imgName;
	long cost;


}


public class ShopMgr : MonoBehaviour 
{
	List<ShopItem> ShopItemList;
	void Start () {
	}
	

	void Update () {
	}


	void Init()
	{
		ShopItemList = new List<ShopItem> ();
	}


	bool BuyItem( string curItemId)
	{
		return true;
	}



}



