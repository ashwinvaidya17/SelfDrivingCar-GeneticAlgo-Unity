using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;
using UnityEngine.UI;

public class CustomCarController : MonoBehaviour {
    RaycastHit HITfront, HITleft, HITright, HIT45left, HIT45right; 
    private CarController _car;
    private float inSpeed = 0f;
    public Vector3 lastLoc;
    public NN brain;
    float inFrontDist, inLeft45Distance, inRight45Distance, inLeftDistance, inRightDistance;
    double outSteering, outAxel;
    Vector3 Left45 = new Vector3(0.5f,0, 0.5f);
    Vector3 Right45 = new Vector3(-0.5f, 0, 0.5f);
    Vector3 Left = new Vector3(1, 0, 0);
    Vector3 Right = new Vector3(-1, 0, 0);
    public Text StatsText;
    public float TotalDistance = 0;
    public bool hasCollided = false;
    // Use this for initialization
    void Start () {
        
        lastLoc = this.transform.position;
    }
    private void Awake()
    {
        _car = GetComponent<CarController>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
        Vector3 thisPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
        if (Physics.Raycast(thisPosition, transform.TransformDirection(Vector3.forward), out HITfront))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(Vector3.forward) * HITfront.distance, Color.yellow);
            inFrontDist = HITfront.distance;
        }
        if (Physics.Raycast(thisPosition, transform.TransformDirection(Left45), out HIT45left))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(Left45) * HIT45left.distance, Color.yellow);
            inLeft45Distance = HIT45left.distance;
        }
        if (Physics.Raycast(thisPosition, transform.TransformDirection(Right45), out HIT45right))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(Right45) * HIT45right.distance, Color.yellow);
            inRight45Distance = HIT45right.distance;
        }
        if (Physics.Raycast(thisPosition, transform.TransformDirection(Left), out HITleft))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(Left) * HITleft.distance, Color.yellow);
            inLeftDistance = HITleft.distance;
        }
        if (Physics.Raycast(thisPosition, transform.TransformDirection(Right), out HITright))
        {
            Debug.DrawRay(thisPosition, transform.TransformDirection(Right) * HITright.distance, Color.yellow);
            inRightDistance = HITright.distance;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            hasCollided = true;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Finish")
        {

            brain.save();
        }
    }


    private void FixedUpdate()
    {
        TotalDistance += Vector3.Distance(this.transform.position, lastLoc);
        inSpeed = Vector3.Distance(this.transform.position, lastLoc) / Time.deltaTime;
        lastLoc = transform.position;
        float[] Inputs = { inSpeed, inFrontDist, inLeftDistance, inRightDistance, inLeft45Distance , inRight45Distance };
        brain.predict(Inputs,ref outSteering, ref outAxel);
        //Debug.Log(outSteering + " " + outAxel+ " " + inSpeed + " " + inFrontDist + " " + inLeftDistance + " " + inRightDistance);

        _car.Move((float)outSteering, 1, 1, 0f);
        StatsText.text = "Speed: " + inSpeed + "\nDistance: " + TotalDistance;
    }
}
