using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loc_ClockTower : MonoBehaviour
{

    //List<string> places = new List<string>() { "UMS", "Suria Sabah" };

    private float currentLongitude = 0f;
    private float currentLatitude = 0f;

    public static Loc_ClockTower Instance { set; get; }
    public float originalLatitude = 6.034815f;
    public float originalLongitude = 116.119234f;
    public float radius;
    public Text range, Distance, lon, lat;
    public GameObject Model, Model2;
    //public Dropdown dropdown;

    public void Awake()
    {
        range.text = "Initializing";
        Distance.text = " Distance: " + "0 m";

    }

    public void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        //PopulateList();
        StartCoroutine(GetCoordinates());
    }

    //void PopulateList()
    //{
    //    dropdown.AddOptions(places);
    //}

    //public void Dropdown_IndexChanged(int index)
    //{
    //    switch (index)
    //    {
    //        case 0: //UMS			
    //            originalLatitude = 6.036501f;
    //            originalLongitude = 116.1222f;
    //            break;
    //        case 1: //suria sabah		
    //            originalLatitude = 5.987002f;
    //            originalLongitude = 116.0775f;
    //            break;

    //    }
    //}

    IEnumerator GetCoordinates()
    {
        while (true)
        {
            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("Location is Not enabled by user ");
                yield break;
            }

            Input.location.Start(1f, .1f);
            int maxwait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxwait > 0)
            {
                yield return new WaitForSeconds(1);
                maxwait--;
            }

            if (maxwait <= 0)
            {
                Debug.Log("Timed Out");
                yield break;
            }

            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determine location");
                yield break;
            }
            else
            {
                currentLatitude = Input.location.lastData.latitude;
                currentLongitude = Input.location.lastData.longitude;
                Vector2 pass = new Vector2(currentLatitude, currentLongitude);
                Calc(originalLatitude, originalLongitude, currentLatitude, currentLongitude, radius);
            }
        }
    }

    Vector2 CartesianToPolar(Vector3 point)
    {
        Vector2 polar;

        //calc longitude
        polar.y = Mathf.Atan2(point.x, point.z);

        //this is easier to write and read than sqrt(pow(x,2), pow(y,2))!
        Vector2 xzLen = new Vector2(point.x, point.z);
        //xzLen = xzLen.magnitude;

        //atan2 does the magic
        polar.x = Mathf.Atan2(-point.y, xzLen.x);

        //convert to deg
        polar *= Mathf.Rad2Deg;
        print(polar.x + "    " + polar.y);

        return polar;
    }


    Vector3 PolarToCartesian(Vector2 polar)
    {

        //an origin vector, representing lat,lon of 0,0. 
        Vector3 origin = new Vector3(originalLatitude, originalLongitude, 1);

        //build a quaternion using euler angles for lat,lon
        Quaternion rotation = Quaternion.Euler(polar.x, polar.y, 0);

        //transform our reference vector by the rotation. Easy-peasy!
        Vector3 point = rotation * origin;
        print(point.x + "    " + point.y);
        return point;
    }

    public void Calc(float xc, float yc, float xp, float yp, float r)
    {
        float distance = 0f, x = 0f, y = 0f, r1, r2, r3, c;
        float R = 6378.137f;

        r1 = xc * Mathf.Deg2Rad;
        r2 = xp * Mathf.Deg2Rad;
        x = (xp - xc) * Mathf.Deg2Rad;
        y = (yp - yc) * Mathf.Deg2Rad;
        r3 = Mathf.Sin(x / 2) * Mathf.Sin(x / 2) + Mathf.Cos(r1) * Mathf.Cos(r2) * Mathf.Sin(y / 2) * Mathf.Sin(y / 2);
        c = 2 * Mathf.Atan2(Mathf.Sqrt(r3), Mathf.Sqrt(1 - r3));
        distance = Mathf.RoundToInt(R * c * 1000f);
        Distance.text = " Distance: " + distance + " m";
        if (distance <= r)
        {
            range.text = "In range";
            Model.SetActive(true);
            Model2.SetActive(true);
        }
        else
        {
            range.text = "Not In range";
            Model.SetActive(false);
            Model2.SetActive(false);
        }
    }

    public void Update()
    {
        currentLatitude = Input.location.lastData.latitude;
        currentLongitude = Input.location.lastData.longitude;
        lon.text = "Longitude: " + currentLongitude;
        lat.text = "Latitude: " + currentLatitude;
    }

}