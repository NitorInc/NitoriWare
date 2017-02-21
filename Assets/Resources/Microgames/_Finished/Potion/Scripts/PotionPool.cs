using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionPool : MonoBehaviour
{

	public GameObject[] ingredients;

	public PotionIngredient[] createPool(int count)
	{
		shuffleIngredients();

		PotionIngredient[] createdIngredients = new PotionIngredient[count];
		for (int i = 0 ; i < count; i++)
		{
			createdIngredients[i] = (Instantiate(ingredients[i]) as GameObject).GetComponent<PotionIngredient>();
		}
		return createdIngredients;
	}

	void shuffleIngredients()
	{
		int index = 0, choice;
		GameObject hold;
		while (index < ingredients.Length)
		{
			choice = Random.Range(index, ingredients.Length);
			if (choice != index)
			{
				hold = ingredients[index];
				ingredients[index] = ingredients[choice];
				ingredients[choice] = hold;
			}
			index++;
		}
	}
	
	void Update ()
	{
		
	}
}
