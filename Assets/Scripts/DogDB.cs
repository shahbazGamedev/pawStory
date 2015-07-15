using UnityEngine;
using System.Collections.Generic;


public enum DogTypes
{
	Beagle = 0,
	Bulldog,
	Chihuahua,
	GermanShepard,
};


public class Dog
{
	public int Uid;
	public DogTypes DogType;
	public string Name; // given by player

	public int Age; // in days

	public float Energy; // should have int values only
	public float Happiness;
	public float Health;
	public float Experience;

	public float WalkSpeed;
	public float RunSpeed;

	public Dog ()
	{
	}
}


public class DogDB
{
	List<Dog> DogList;
	public DogDB () 
	{
		DogList = new List<Dog> ();
		InitDB ();
	}

	// move this to file
	void InitDB()
	{
		Dog curDog = new Dog ();
		curDog.Name = "Tommy";
		curDog.Uid = Random.Range (0, 30000);// ponz.tmp
		curDog.DogType = DogTypes.Beagle;
		curDog.Age = 12 * 7;// 12 weeks;

		curDog.Energy = 60;
		curDog.Happiness = 80;
		curDog.Health = 100;
		curDog.Experience = 30;

		curDog.WalkSpeed = 10;
		curDog.RunSpeed = 10;
	}
}
