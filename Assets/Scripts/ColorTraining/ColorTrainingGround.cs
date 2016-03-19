using UnityEngine;
using System.Collections;

public class ColorTrainingGround : MonoBehaviour {

	public static ColorTrainingGround instref;

	string layerName;

	void Awake()
	{
		instref = this;
	}

	void OnCollisionEnter(Collision col)
	{
		layerName = LayerMask.LayerToName (col.gameObject.layer);

		switch (layerName) 
		{
		case "RedObject":
			if (ColorTrainingMgr.instRef.red) 
			{
				ColorTrainingMgr.instRef.isMoving = true;
			}
			else 
			{
				ColorTrainingMgr.instRef.GameOver ();
			}
			break;

		case "GreenObject":
			if (ColorTrainingMgr.instRef.green) 
			{
				ColorTrainingMgr.instRef.isMoving = true;
			}
			else 
			{
				ColorTrainingMgr.instRef.GameOver ();
			}
			break;

		case "BlueObject":
			if (ColorTrainingMgr.instRef.blue) 
			{
				ColorTrainingMgr.instRef.isMoving = true;
			}
			else 
			{
				ColorTrainingMgr.instRef.GameOver ();
			}
			break;

		case "YellowObject":
			if (ColorTrainingMgr.instRef.yellow) 
			{
				ColorTrainingMgr.instRef.isMoving = true;
			}
			else 
			{
				ColorTrainingMgr.instRef.GameOver ();
			}
			break;	
		}
	}
}
