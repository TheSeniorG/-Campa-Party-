using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Text : MonoBehaviour
{
    [Header("General Options")]
    [SerializeField] private GameObject[] gameObjectsToDeacivate;
    [SerializeField] private GameObject[] gameObjectsToActivate;
    [SerializeField] private Animator[] animatorsToActivate;

    [Header("Volume Options")]
    [SerializeField] private bool modifyVol;
    [ShowIf("modifyVol")][SerializeField] private VolumeController volumeController;
    [ShowIf("modifyVol")][SerializeField] private int volumeMod = 1;

    private GameObject selectedIcon;
    private bool triggerActivated = false;

    private void Awake(){selectedIcon = transform.GetChild(0).gameObject;}
    private void OnMouseDown()
    {
        //SI TIENE ANIMATOR ACTIVAMOS LA ANIMACION
        if (animatorsToActivate != null) 
        {
            //USAMOS EL BOOLEANO PARA QUE NO PUEDA ACTIVAR EL TRIGGER SEGUIDAMENTE
            if (!triggerActivated)
            {
                //DESACTIVAMOS QUE PUEDA ACTIVAR LA ANIMACION MULTIPLES VECES
                triggerActivated = true;
                foreach (var animator in animatorsToActivate) { animator.SetTrigger("next"); }
                //LLAMAMOS AL COOLDOWN
                Invoke("ReactivateTriggerBool", 3f);
            }
        }

        //DESACTIVAMOS SU OUTLINE
        if (selectedIcon != null) { selectedIcon.SetActive(false); }

        //SI TIENE OBJETOS POR ACTIVAR/DESACTIVAR
        if (gameObjectsToActivate != null) 
        {
            foreach (var gameObject in gameObjectsToActivate){gameObject.SetActive(true);}
        }
        if (gameObjectsToDeacivate!= null)
        {
            foreach (var gameObject in gameObjectsToDeacivate){gameObject.SetActive(false);}
        }

        if(modifyVol && volumeController != null) { volumeController.ModifyVolume(volumeMod); }
    }
    private void OnMouseEnter(){ if (selectedIcon != null) { selectedIcon.SetActive(true); } }
    private void OnMouseExit() { if(selectedIcon != null){selectedIcon.SetActive(false); } }
    
    //REACTIVAMOS EL BOOLEANO PARA QUE EL USUARIO PUEDE VOLVER A USARLO
    private void ReactivateTriggerBool() { triggerActivated = false; }
}
