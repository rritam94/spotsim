using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerTray : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 1.5f;
    [SerializeField]
    private float spinrate = 180;
    [SerializeField]
    private float uprate = 1f;
    [SerializeField]
    private int index;

    private bool deleting;
    List<GameObject> ingredients;


    // Start is called before the first frame update
    void Start()
    {
        ingredients = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!deleting && CheckIngredients())
        {
            // Log to task
            FriesAndDrinkTask.Instance.EndATrayTask(index);

            StartCoroutine(DespawnTray());
        }
    }

    IEnumerator DespawnTray()
    {
        deleting = true;

        List<GameObject> ingredients_to_destroy = new List<GameObject>();
        foreach (GameObject ingredient in ingredients)
        {
            ingredients_to_destroy.Add(ingredient);
        }

        // waits for approx a second

        float fps = Mathf.Max(60, 1 / Time.deltaTime);

        for (float i = 0; i < fps; i += 1)
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // turns off physics

        foreach (GameObject ingredient in ingredients_to_destroy)
        {
            if (ingredient.GetComponent<Rigidbody>())
            {
                ingredient.GetComponent<Rigidbody>().isKinematic = true;
                ingredient.transform.parent = transform;
            }
        }

        // plays animation

        for (float i = 0; i < despawnTime * fps; i += 1)
        {
            transform.Rotate(Vector3.up, spinrate * Time.deltaTime);
            transform.position += Vector3.up * uprate * Time.deltaTime;
            
            yield return new WaitForSeconds(Time.deltaTime);
        }

        // delete them all after the fun animation plays

        for (int i = ingredients_to_destroy.Count - 1; i >= 0; i--)
        {
            GameObject temp = ingredients_to_destroy[i];
            try
            {
                ingredients.Remove(temp);
            }
            catch (System.Exception)
            {
                continue;
            }
            Destroy(temp);
        }
        for (int i = ingredients.Count - 1; i >= 0; i--)
        {
            GameObject temp = ingredients[i];
            try
            {
                ingredients.Remove(temp);
            }
            catch (System.Exception)
            {
                continue;
            }
            Destroy(temp);
        }

        deleting = false;
        Destroy(transform.gameObject);
        yield return null;
    }



    private bool CheckIngredients()
    {
        int drink = 1;
        int fries = 1;

        foreach (GameObject ingredient in ingredients)
        {
            if (ingredient.name.Contains("Drink")) { drink--; }
            if (ingredient.name.Contains("Fries")) { fries--; }
        }

        // at least 1 of everything we want
        return drink <= 0 && fries <= 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && (other.name.Contains("Fries") || other.name.Contains("Drink")))
        {
            ingredients.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item") && (other.name.Contains("Fries") || other.name.Contains("Drink")))
        {
            ingredients.Remove(other.gameObject);
        }
    }
}
