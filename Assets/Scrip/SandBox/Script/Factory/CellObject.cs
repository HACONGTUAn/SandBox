using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatesOfMatter
{
    solid,
    liquid,
    gas,
    plasma,
    boseEinsteinCondensate
}

[CreateAssetMenu(fileName = "New Cell")]
public class CellObject : ScriptableObject
{
    [Header("Info")]
    [Tooltip("The name of this particle.")]
    public string cellName;
    [Tooltip("coler")]
    public Color cellColor;
    [Tooltip("The state of matter of this particle.")]
    public StatesOfMatter state;
    [Header("Thiet Lap Vat Ly")]
    [Tooltip("Trong Luc")]
    public float Gravity;

    [Tooltip("va cham voi")]
    public CellObject[] morphCollision;

    [Tooltip("Phan Ung Voi")]

    public CellObject[] morphInto;


}
