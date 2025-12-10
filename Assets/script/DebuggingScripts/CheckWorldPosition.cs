using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWorldPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("World Position dari " + transform.name + " Parent: " + transform.parent.name + " : " + transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
