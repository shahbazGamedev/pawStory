
// Class which is used to customize the player models and weapons.
// TODO : Work in Progress


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System; 
using System.Text;
//using System.Web;
using MiniJSON;
//using SimpleJSON;

[System.Serializable]
public class Weapons : IComparable<Weapons>  // Class which holds the Weapon Types and Weapon Properties
{
	
	//public Weapons(){}
	
	public enum E_WeaponType
	{  
	  	e_Sword,
		e_Bow,
		e_Arrow

		 
	};


	  
	public E_WeaponType eWeaponType; 
	public string weaponName;
	public float  weaponDamage;
	public float  weaponCostInGoldCoins;
	public bool   weaponLockedStatus;
	public bool   weaponEquippedStatus;
	public bool   weaponModelChangeNeeded;
	public string weaponModelPath;
	public string weaponTextureMaterialPath;
	
	IComparable<Weapons> Members; // IComparable class used to compare the values in the List.
	 
	public int CompareTo(Weapons other)
	{
		return 1;
	}
 	
}

[System.Serializable]
public class Armor     // Class which holds the Armor Types and Armor Properties
{
	
	public enum E_ArmorType
	{  
	  	e_Chest,
		e_Shoulders,
		e_Hand,
		e_Legs,
		e_Feet
		 
	};
	
	public E_ArmorType eArmorType; 
	public string armorName;
	public float  armorDamage;
	public float  armorCostInGoldCoins;
	public bool   armorEquippedStatus;
	public bool   armorLockedStatus;
	public bool   armorModelChangeNeeded;
	public string armorModelPath;
	public string armorTextureMaterialPath;
 	 
}


[System.Serializable]
public class PlayerModelTexture  // Class which holds the Player Model Texture Types and Player Model Properties
{
	public string  playerTextureName;
	public float   playerTextureCostInGoldCoins;
	public bool    playerTextureEquipped;
	public bool    playerTextureLockedStatus;
	public string  playerTextureMaterialPath; 
 	 
}

class CompareWeaponBasedOnDamage : IComparer<Weapons>  // To sort the Weapon List based on Higher Damage
{
	 // IComparer<Weapons> Members
		
	public int Compare(Weapons x, Weapons y)
	{
		if(x.weaponDamage>y.weaponDamage) return -1;
		else if(x.weaponDamage<y.weaponDamage) return 1;
		else return 0;
	}
}


//Class which takes care of the player customization.

public class PlayerCustomizationManager : MonoBehaviour
{
	 
	public GameObject playerModelReference;
	public GameObject playerBow;
	public  string  weaponXmlPath , armorXmlPath , playerModelXmlPath;
	public List<PlayerModelTexture > playerModelList = new List<PlayerModelTexture>();
	public List<Weapons> weaponList = new List<Weapons>();
	public List<Armor>armorList = new List<Armor>();
	private GameObject[] wp; 
	private GameObject[] weapon;
	bool showModels;
 	private static PlayerCustomizationManager instance = null; 
	private GameObject newObject;
	private GameObject parentObject;
	List<Weapons> weaponLoadList; 
	string weaponLoadPath;
	Ray ray;
	RaycastHit hit;
	string clickedGameObject;
	int currentIndex=-1 , prevIndex =-1;
	
	Encoding enCode = new UTF8Encoding();
	
	public static PlayerCustomizationManager getInstance
	{
		get{return instance;}
	}
	
	 //CompareWeaponBasedOnDamage cwbd = new CompareWeaponBasedOnDamage(); // for Icomparer
	
	 
	void OnEnable()
	{
		instance = this;
	}
	
	// Use this for initialization
	void Start ()
	{ 
		SaveWeaponDataListToXML(weaponList);

		string jsonText =  MiniJSON.Json.Serialize(weaponList);

		Debug.Log(jsonText.ToString());
		//SaveArmorDataListToXML(armorList);
		//SavePlayerModelDataListToXML(playerModelList);
		 
  
	}
	
	void SetWeaponToPlayerModel()
	{
		
		//Debug.Log("Inside weapon");
	 	for(int i=0; i<weaponList.Count; i++)
		{ 
			if(weaponList[i].weaponName == clickedGameObject && weaponList[i].weaponModelChangeNeeded )
			{
				 currentIndex = i;
			if(currentIndex!=prevIndex && prevIndex!=-1) 
			{
				 Destroy(newObject); 
				 weaponList[prevIndex].weaponEquippedStatus = false;
			}
			Debug.Log("Current Index "+ currentIndex + "prevIndex " + prevIndex);
				
			}
		}
		
		if(currentIndex!=prevIndex)
		{
			if(!weaponList[currentIndex].weaponEquippedStatus )
				{
					weaponLoadPath = weaponList[currentIndex].weaponModelPath;
					weaponList[currentIndex].weaponEquippedStatus = true;
				 
					playerBow.SetActiveRecursively(false);
				 	newObject = Instantiate(Resources.Load(weaponLoadPath)) as GameObject;
					newObject.gameObject.layer = playerBow.gameObject.layer;
					parentObject =GameObject.FindGameObjectWithTag("Hand"); // Give the tag name of the part to attach the bow.
				 	newObject.transform.parent = parentObject.transform;  
					newObject.transform.localScale = playerBow.transform.localScale;
					newObject.transform.localRotation = playerBow.transform.localRotation;
					newObject.transform.localPosition = playerBow.transform.localPosition;
					
					 
					prevIndex = currentIndex; 
		   		}
			 
		}
	}	
	 
					
	 
	
	void DisplayModels()
	{  
		wp = new GameObject[2];
		wp[0] = GameObject.FindGameObjectWithTag("Weapon").transform.GetChild(0).gameObject;
		wp[1] = GameObject.FindGameObjectWithTag("Weapon").transform.GetChild(3).gameObject ;
		
		 weapon = new GameObject[wp.Length];
				
		 weapon[0] = GameObject.Instantiate(Resources.Load(weaponLoadList[0].weaponModelPath),wp[0].transform.position ,Quaternion.identity) as GameObject ;
		 weapon[1] = GameObject.Instantiate(Resources.Load(weaponLoadList[1].weaponModelPath),wp[1].transform.position ,Quaternion.identity) as GameObject ;   
		 
		 weapon[0].transform.parent = wp[0].transform;
		weapon[1].transform.parent = wp[1].transform;
		
		/*weapon[0].transform.localPosition = new Vector3(3,-0.6f,-7);
		weapon[1].transform.localPosition = new Vector3(3,-0.6f,-7); */
	}
	
	void ChangePlayerModelTexture()
	{
		for(int i=0; i<playerModelList.Count; i++)
		{
			if((!playerModelList[i].playerTextureEquipped) && ( !playerModelList[i].playerTextureLockedStatus) ) 
			{
				//playerModelReference.renderer.material= playerModelList[i].playerTextureMaterial ;
			}
		}
	}
	
  
	
# region Save Data To XML 

	public  void SaveWeaponDataListToXML( List<Weapons> weaponList)
	{
	   XmlSerializer serializerWeapons = new XmlSerializer(typeof(List<Weapons>));	
 
	  TextWriter textWriter = new StreamWriter(Path.Combine(Application.persistentDataPath ,weaponXmlPath));//,
		 
	  Debug.Log(Path.Combine(Application.persistentDataPath , weaponXmlPath));
	  serializerWeapons.Serialize(textWriter, weaponList); 
	  textWriter.Close();
	
	}
	
	
	public  void SaveArmorDataListToXML( List<Armor> armorList )
	{
	  XmlSerializer serializerArmor = new XmlSerializer(typeof(List<Armor>));	  
	  TextWriter textWriter = new StreamWriter(Path.Combine(Application.persistentDataPath , armorXmlPath));
	  serializerArmor.Serialize(textWriter, armorList); 
	  textWriter.Close();
	
	}
	
	public void SavePlayerModelDataListToXML(List<PlayerModelTexture> playerModelTList)
	{
	  XmlSerializer serializerPlayerModel = new XmlSerializer(typeof(List<PlayerModelTexture>));	  
	  TextWriter textWriter = new StreamWriter(Path.Combine(Application.persistentDataPath ,playerModelXmlPath));
	  serializerPlayerModel.Serialize(textWriter, playerModelTList); 
	  textWriter.Close();
	}
	
	
#endregion
	
#region Load Data From XML	
	
	public List<Weapons> LoadWeaponDataFromXML()
	{
	   XmlSerializer deserializer = new XmlSerializer(typeof(List<Weapons>));
	   TextReader textReader = new StreamReader(Path.Combine(Application.persistentDataPath ,weaponXmlPath));
	   List<Weapons> weaponD; 
	   weaponD = (List<Weapons>)deserializer.Deserialize(textReader);
	   textReader.Close();
	   Debug.Log("Loaded Weapon Details From Xml"); 
  	   return weaponD;
	 
	}
	
	
	public List<Armor> LoadArmorDataFromXML()
	{
	   XmlSerializer deserializer = new XmlSerializer(typeof(List<Armor>));
	   TextReader textReader =  new StreamReader(Path.Combine(Application.persistentDataPath , armorXmlPath));
	   List<Armor> armorD; 
	   armorD = (List<Armor>)deserializer.Deserialize(textReader);
	   textReader.Close();
	   Debug.Log("Loaded Armor Details From Xml");
  	   return armorD;
	 
	}
	
	
	public List<PlayerModelTexture> LoadPlayerModelDataFromXML()
	{
	   XmlSerializer deserializer = new XmlSerializer(typeof(List<PlayerModelTexture>));
	   TextReader textReader =  new StreamReader(Path.Combine(Application.persistentDataPath , playerModelXmlPath));
	   List<PlayerModelTexture> playerModelTextureD; 
	   playerModelTextureD = (List<PlayerModelTexture>)deserializer.Deserialize(textReader);
	   textReader.Close();
	   Debug.Log("Loaded Player Model Texture Details From Xml");
  	   return playerModelTextureD;
	 
	}
	
#endregion
	
	 
	
	void MouseRayCast()
	{
		
		if(Input.GetMouseButtonDown(0)) 
		{
			
			// Construct a ray from the current mouse coordinates
			ray = Camera.main.ScreenPointToRay (Input.mousePosition);
						
			if (Physics.Raycast (ray,out hit)) 
			{
				 
				clickedGameObject =hit.transform.gameObject.name;
				Debug.Log(clickedGameObject);
				 
			}
		}
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	//	MouseRayCast();
		
		// SetWeaponToPlayerModel();
		
		 
	}
	
	 
}
