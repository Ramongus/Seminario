﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGhost : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * 5 * Time.deltaTime;
    }
}
