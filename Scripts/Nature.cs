using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nature : MonoBehaviour {
    public float bestDistanceTillNow = 0;
    public float bestDistance = 0.0f;
    public int bestIndex = 0;
    public float secondBestDistance = 0.0f;
    public int secondBestIndex = 0;
    public int totalPopulation = 10;
    int currentPopCount;
    int Genetation=0;
    public GameObject Car;

    List<NN> children = new List<NN>();

    //For GUI
    public Text Populationtext;
	// Use this for initialization
	void Start () {
        //Create initial population
	    for(int i=0; i<totalPopulation; i++)
        {
            children.Add(new NN());
        }
       
                StartCoroutine(Generate());
               
	}

	// Update is called once per frame
	void Update () {
        Populationtext.text = "Population: "+(currentPopCount+1)+"/"+totalPopulation
            +"\nGeneration: "+Genetation
            +"\nBest: "+ bestDistanceTillNow
            + "\nBest distance: " + bestDistance
            + "\nSecond Best distance: " + secondBestDistance;
	}

    void ResetCar()
    {
        Car.transform.position = new Vector3(7, -3.61f, -50);
        Car.transform.eulerAngles = new Vector3(0, 0, 0);
        Car.GetComponent<CustomCarController>().TotalDistance = 0;
        Car.GetComponent<CustomCarController>().hasCollided = false;
        Car.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        Car.gameObject.GetComponent<CustomCarController>().lastLoc = new Vector3(7, -3.61f, -50);
    }


    IEnumerator Generate()
    {
    while (true)
    {

        Genetation++;
        bestDistance = 0;
        secondBestDistance = 0;
        for (currentPopCount = 0; currentPopCount < totalPopulation; currentPopCount++)
        {
            ResetCar();
            Car.GetComponent<CustomCarController>().brain = children[currentPopCount];
            yield return new WaitUntil(()=>Car.GetComponent<CustomCarController>().hasCollided);
            float dist = Car.GetComponent<CustomCarController>().TotalDistance;
            if (dist > bestDistanceTillNow)
                bestDistanceTillNow = dist;
            if (dist > bestDistance)
            {
                secondBestDistance = bestDistance;
                secondBestIndex = bestIndex;
                bestDistance = dist;
                bestIndex = currentPopCount;
            }
            else if (dist > secondBestDistance)
            {
                secondBestDistance = dist;
                secondBestIndex = currentPopCount;
            }


        }
            NN A = new NN(children[bestIndex]);
            NN B = new NN(children[secondBestIndex]);
            
            children.Clear();

            for (int i=0; i<totalPopulation; i++)
            {
                children.Add(new NN(A, B));
            }

        }
    }


    
}
