using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class AnimationsEvents : MonoBehaviour
{
    [SerializeField] private bool hasToActivate;
    [ShowIf("hasToActivate")][SerializeField] private GameObject[] gameObjectsToActivate;

    public void Deactivate() { gameObject.SetActive(false); }
    public void ActivateGameObject()
    {
        if (hasToActivate) { foreach (GameObject go in gameObjectsToActivate) { go.SetActive(true); } }

    }
    public void ToggleGameObject()
    {
        if (hasToActivate) { foreach (GameObject go in gameObjectsToActivate) { go.SetActive(!go.activeSelf); } }

    }
}
