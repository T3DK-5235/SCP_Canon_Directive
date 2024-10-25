using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStatChange : MonoBehaviour
{
    private string statChanged;
    private int statChangedEffect;
    private int statChangedDuration;
    // Start is called before the first frame update

    public ActiveStatChange(string statChanged, int statChangedEffect, int statChangedDuration)
    {
        this.statChanged = statChanged;
        this.statChangedEffect = statChangedEffect;
        this.statChangedDuration = statChangedDuration;
    }

    public void updateStatDuration() {
        //It only gets decremented if it isn't 0, and it is the end of a month
        statChangedDuration -= 1;
    }

    public string getStatChanged() {
        return statChanged;
    }

    public int getStatChangedEffect() {
        return statChangedEffect;
    }

    public int getStatChangedDuration() {
        return statChangedDuration;
    }
}
