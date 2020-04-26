using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour
{
    public GameObject soulExplosion;
    float _timer;
    bool playerColission;

    private void Start()
    {
        _timer = 0;
    }

    private void Update()
    {
        if (playerColission)
        {
            _timer += Time.deltaTime;
            if (_timer >= 5f)
            {
                _timer = 0;
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11)
        {
            soulExplosion.gameObject.SetActive(true);
            playerColission = true;
        }
    }
}
