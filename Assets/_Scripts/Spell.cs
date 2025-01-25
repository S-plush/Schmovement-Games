using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Spell")]
public abstract class Spell : ScriptableObject
{

    [Header("Gameplay atributes")]
    public int manaCost;
    public SpellType Neutral;
    public bool directional;
    public int rangeValue;
    public int damageValue;
    public float knockbackValue;

    [Header("Other Atributes")]
    public Sprite image;

    public void KnockBack()
    {

    }
}

public enum SpellType
{
    Water,
    Fire,
    Earth,
    Lightning,
    Neutral
}