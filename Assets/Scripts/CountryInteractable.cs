using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class CountryInteractable : XRBaseInteractable
{
    private Manager manager;

    void Start()
    {
        manager = FindObjectOfType<Manager>();
    }

    protected override void OnActivated(ActivateEventArgs args)
    {
        base.OnActivated(args);

        Debug.Log($"Country clicked {gameObject.name}");

        manager.OnCountryClicked(gameObject);
    }
}
