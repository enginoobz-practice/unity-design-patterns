﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tree", order = 51, menuName = "Plant Data")]
public class PlantData : ScriptableObject
{
    public enum Threat { NONE, LOW, MEDIUM, HIGH }

    [SerializeField] string planName;
    [SerializeField] Threat plantThreat;
    [SerializeField] Texture icon;
}
