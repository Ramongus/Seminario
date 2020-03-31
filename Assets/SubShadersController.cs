using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubShadersController : MonoBehaviour
{
    public Material damage;
    public Material heal;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage();
        }   
    }
    public void Damage()
    {
        StartCoroutine("DamageChange");
    }

    IEnumerator DamageChange()
    {
        damage.SetFloat("_Slice", 5);
        yield return new WaitForSeconds(1.5f);
        damage.SetFloat("_Slice", 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Debug.Log("heal should appear");
            heal.SetFloat("_Slice", 5);
        }
        else heal.SetFloat("_Slice", 0);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {        
            heal.SetFloat("_Slice", 0);
        }
    }
}
