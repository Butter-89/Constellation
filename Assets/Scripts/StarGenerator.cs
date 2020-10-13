using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarGenerator : MonoBehaviour
{
    [Range(1,10)]
    public int density;
    public GameObject starPrefab;
    public Material lineMaterial;
    [Range(3,10)]
    public int edgeCount = 8;
    private int count;

    List<GameObject> starList = new List<GameObject>();
    List<GameObject> StarList => starList;

    private float halfWidth = 170f;
    private float halfHeight = 95f;
    private float minInterval = 20f;
    private int quantity = 100;
    private int maxTries = 100;
    void Start()
    {
        count = edgeCount;
        minInterval -= density;
        quantity += density * 10;
        for(int i = 0; i < quantity; i++)
        {
            SpawnStar();
        }
        BuildConstellation();
    }

    private GameObject SpawnStar()
    {
        Vector3 position = Vector3.zero;
        bool qualified = true;

        for (int i = 0; i < maxTries; i++)
        {
            position = new Vector3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0f);
            

            foreach (GameObject star in starList)
            {
                if ((star.transform.position - position).magnitude < minInterval)
                {
                    qualified = false;
                    break;
                }
            }

            if (qualified)
                break;
        }

        if (!qualified)
            return null;

        float rotation = Random.Range(0f, 360f);
        float brightness = Random.Range(150f, 255f);
        float scaleValue = Random.Range(0.05f, 0.1f);
        Vector3 scale = new Vector3(scaleValue, scaleValue, 1f); 
        GameObject gStar = Instantiate(starPrefab, this.transform);
        gStar.transform.position = position;
        gStar.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
        gStar.transform.localScale = scale;
        starList.Add(gStar);

        return gStar;
    }

    private void BuildConstellation()
    {
        GameObject start = starList[Random.Range(0, starList.Count - 1)];
        ConnectStars(start);
    }

    private void ConnectStars(GameObject i_start)
    {
        if (count <= 0)
        {

            return;
        }
            

        float range = 1.5f * minInterval; // Random.Range(minInterval, minInterval + density);
        bool connectionFound = false;
        foreach(GameObject star in starList)
        {
            bool connected = star.GetComponent<Star>().connected;
            if (!connected) // (i_start.transform.position - star.transform.position).magnitude <= range && 
            {
                connectionFound = true;
                count--;
                star.GetComponent<Star>().connected = true;
                LineRenderer lr = i_start.AddComponent<LineRenderer>();
                lr.enabled = true;

                lr.material = new Material(lineMaterial);
                lr.startColor = Color.white;
                lr.endColor = Color.white;
                lr.widthMultiplier = 0.6f;
                
                //lr.positionCount = 2;
                lr.SetPosition(0, i_start.transform.position);
                lr.SetPosition(1, star.transform.position);

                ConnectStars(star);
                break;
            }
        }

        if(!connectionFound)
        {
            return;
        }
    }

}
