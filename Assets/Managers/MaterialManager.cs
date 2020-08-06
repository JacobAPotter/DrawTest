using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    Material scribbleMat;
    float scribbleTimeStamp;
    const float scribbleInterval = 0.133f;

    void Start()
    {
        Transform scrib = transform.Find("Scribble");
        scribbleMat = scrib.GetComponent<Renderer>().sharedMaterial;
        scrib.gameObject.SetActive(false);
        scribbleMat.SetTextureScale("_MainTex", new Vector2(0.2f, 1.0f));
    }

    void Update()
    {

        if (Time.timeSinceLevelLoad - scribbleTimeStamp > scribbleInterval)
        {
            scribbleTimeStamp = Time.timeSinceLevelLoad;
            scribbleMat.mainTextureOffset = Vector2.right * Random.value;
        }



    }
}
