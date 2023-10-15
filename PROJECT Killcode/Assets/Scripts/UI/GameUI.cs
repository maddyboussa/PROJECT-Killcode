using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Killcode.Core;
using AYellowpaper.SerializedCollections;

public class GameUI : MonoBehaviour
{
    #region FIELDS
    // get reference to pause ui
    [SerializeField] private GameObject healthBar;
    #endregion

    public void UpdateHealthBar(Component sender, object data)
    {
        healthBar.GetComponent<Slider>().value = (float)data;

        Debug.Log(healthBar.GetComponent<Slider>().value);
    }


}
