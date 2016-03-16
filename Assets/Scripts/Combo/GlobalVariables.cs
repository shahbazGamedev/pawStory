using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour
{

	public static int numberOfRetry;
	public static float distanceCovered;
	public static long score;

	public static bool isMuted =false;

	public static string APIKEY ="011a237454522dffd2201b5196b98ea6979a2b44fc2944032309ed760a5fe870";
	public static string SECRETKEY="0b13ef445010a56e7b10ba2a65294a29d66921dfdb0a7ccdf019080c274851b4";

	public static string userID,userAccessToken;

	public static string dataBaseName = "PuppyDataBase";
	public static string collectionName = "User";

	public static string collectionBeagleName= "Beagle" ,collectionBulldogName ="BullDog" ,collectionChihuahuaName ="Chihuahua" ,collectionGermanShepardName ="GermanShepard";

	public static string collectionUtility = "Utility";

	public static int puppyID;

	public static int totalCoins;


}
