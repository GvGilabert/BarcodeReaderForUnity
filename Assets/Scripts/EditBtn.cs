using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditBtn : MonoBehaviour
{
    Buttons btn;
    void Start()
    {
        btn = GameObject.FindGameObjectWithTag("Manager").GetComponent<Buttons>();
    }

    public void Click()
    {
        btn.EditScreenOC(transform.gameObject);
    }
}
