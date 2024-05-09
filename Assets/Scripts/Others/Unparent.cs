using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unparent : MonoBehaviour
{
    //SCRIPT PARA DESEMPARENTAR AL INSTANCIAR UN OBJETO

    private void Start(){transform.parent = null;}
}
