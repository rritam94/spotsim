using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField]
    private float despawnTime = 3f;
    [SerializeField]
    private float spinrate = 360f;
    [SerializeField]
    private float uprate = 1f;

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
            try { SandwichTask.Instance.EndCurrentSandwichCycle(); }
            catch { }
            try { AcclimationTask.Instance.EndCurrentSandwichCycle(); }
            catch { }

            StartCoroutine(DespawnBurger());
        }
    }

    IEnumerator DespawnBurger()
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
            }
        }

        // plays animation

        for (float i = 0; i < despawnTime * fps; i += 1)
        {
            foreach (GameObject ingredient in ingredients_to_destroy)
            { 
                ingredient.transform.Rotate(Vector3.up, spinrate * Time.deltaTime);
                ingredient.transform.position += Vector3.up * uprate * Time.deltaTime;
            }
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
        yield return null;
    }



    private bool CheckIngredients()
    {
        int buns = 2;
        int cheese = 1;
        int lettuce = 1;
        int patty = 1;
        int tomato = 1;

        foreach (GameObject ingredient in ingredients)
        {
            if (ingredient.name.Contains("Bun")) { buns--; }
            if (ingredient.name.Contains("Cheese")) { cheese--; }
            if (ingredient.name.Contains("Lettuce")) { lettuce--; }
            if (ingredient.name.Contains("Patty")) { patty--; }
            if (ingredient.name.Contains("Tomato")) { tomato--; }
        }

        // at least 1 of everything we want
        return buns <= 0 && cheese <= 0 && lettuce <= 0 && patty <= 0 && tomato <= 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            ingredients.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            ingredients.Remove(other.gameObject);
        }
    }
}
