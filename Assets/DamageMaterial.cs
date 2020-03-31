using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMaterial : MonoBehaviour
{
    public Material damage;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage();
        }   
    }
    public void Damage()
    {
        damage.SetFloat("_Slice", 5);
        Debug.Log("Danio");
        StartCoroutine("DamageChange");
    }

    IEnumerator DamageChange()
    {
        damage.SetFloat("_Slice", 5);
        yield return new WaitForSeconds(2.5f);
        damage.SetFloat("_Slice", 0);
    }
}
