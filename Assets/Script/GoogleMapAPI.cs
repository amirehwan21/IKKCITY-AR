using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMapAPI : MonoBehaviour {

    public RawImage img;



    public bool autoLocateCenter = true;
    public GoogleMapLocation centerLocation;
    public int zoom = 10;
    //public int size;
    public int mapWidth = 1500;
    public int mapHeight = 1500;
    public bool doubleResolution = false;
    public GoogleMapMarker[] markers;


    public enum mapType { roadmap, satellite, hybrid, terrain }
    public mapType mapSelected;






    IEnumerator Map()
    {
        var url = "http://maps.googleapis.com/maps/api/staticmap";
        var qs = "";

        if (centerLocation.address != "")
            qs += "center=" + WWW.UnEscapeURL(centerLocation.address);
        else
        {
            qs += "center=" + WWW.UnEscapeURL(string.Format("{0},{1}", centerLocation.latitude, centerLocation.longitude));
        }


        foreach (var i in markers)
        {
            qs += "&markers=" + string.Format("size:{0}|color:{1}|label:{2}", i.size.ToString().ToLower(), i.color, i.label);
            foreach (var loc in i.locations)
            {
                if (loc.address != "")
                    qs += "|" + WWW.UnEscapeURL(loc.address);
                else
                    qs += "|" + WWW.UnEscapeURL(string.Format("{0},{1}", loc.latitude, loc.longitude));
            }
        }


        qs += "&size=" + mapWidth + "x" + mapHeight;
        qs += "&scale=" + (doubleResolution ? "2" : "15");
        qs += "&zoom=" + zoom.ToString();
        qs += "&maptype=" + mapSelected.ToString().ToLower();

        WWW www = new WWW(url + "?" + qs);
        yield return www;
        img.texture = www.texture;
        img.SetNativeSize();

    }
    // Use this for initialization
    void Start()
    {
        img = gameObject.GetComponent<RawImage>();
        StartCoroutine(Map());
    }

    // Update is called once per frame
    void Update()
    {

    }
}

public enum GoogleMapColor
{
    black,
    brown,
    green,
    purple,
    yellow,
    blue,
    gray,
    orange,
    red,
    white
}

[System.Serializable]
public class GoogleMapMarker
{
    public enum GoogleMapMarkerSize
    {
        Tiny,
        Small,
        Mid
    }
    public GoogleMapMarkerSize size;
    public GoogleMapColor color;
    public string label;
    public GoogleMapLocation[] locations;

}

[System.Serializable]
public class GoogleMapLocation
{
    public string address;
    public float latitude;
    public float longitude;
}