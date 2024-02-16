using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AmmoCount : MonoBehaviour
{
    public TMP_Text ammunitionText;
    public TMP_Text magText;

    public static AmmoCount occurence;

    private void Awake()
    {
        occurence = this;
    }

    public void UpdateAmmoText(int presentAmmunition)
    {
        ammunitionText.text = "Ammo " + presentAmmunition;
    }

    public void UpdateMagText(int mag)
    {
        magText.text = "Magazines " + mag;
    }
}
