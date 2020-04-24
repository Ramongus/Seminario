using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbbilitieCanvasManager : MonoBehaviour
{
    public GameObject[] abbilities = new GameObject[6];
    public GameObject abbilitie1;
    public GameObject abbilitie2;
    public GameObject abbilitie3;
    public GameObject abbilitie4;
    public GameObject abbilitie5;

    public int currentAbbilitie;
    public int lastAbbilitie;
    private void Start()
    {
        abbilities[1] = abbilitie1;
        abbilities[2] = abbilitie2;
        abbilities[3] = abbilitie3;
        abbilities[4] = abbilitie4;
        abbilities[5] = abbilitie5;
        currentAbbilitie = 3;
    }
    private void Update()
    {
        ChangeAbbilitie();
    }
    public void ChangeAbbilitie()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {   
            if (currentAbbilitie == 1) // returnear aca si no queremos que vuelva a empezar
            {            
                lastAbbilitie = currentAbbilitie;
                currentAbbilitie = 5;
                return;
            }
            lastAbbilitie = currentAbbilitie;
            currentAbbilitie--;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentAbbilitie == 5) // returnear aca si no queremos que vuelva a empezar
            {
                lastAbbilitie = currentAbbilitie;
                currentAbbilitie = 1;
                return;
            }
            lastAbbilitie = currentAbbilitie;
            currentAbbilitie++;
        }
        UpdateCanvas();
    }
    void UpdateCanvas()
    {
        abbilities[currentAbbilitie].gameObject.transform.localScale = new Vector3(1.3f,1.3f ,1.3f);
        if (lastAbbilitie == 0) return;
        abbilities[lastAbbilitie].gameObject.transform.localScale = new Vector3(1, 1, 1);
    }
}
