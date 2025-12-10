using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CountryInteractable : XRBaseInteractable
{
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        
        Debug.Log($"Country selected via XR Interaction {this.gameObject.name}");

        CountryBehavior countryBehavior = GetComponent<CountryBehavior>();
        if (countryBehavior != null)
        {
            countryBehavior.ToggleSelection();
        }
    }
}
