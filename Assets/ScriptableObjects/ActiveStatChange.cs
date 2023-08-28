using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveStatChange : ScriptableObject 
{
    public string _statChanged;
    public int _statChangedEffect;
    public int _statChangedDuration;

    public void Init(string _statChanged, int _statChangedEffect, int _statChangedDuration) {
        this._statChanged = _statChanged;
        this._statChangedEffect = _statChangedEffect;
        this._statChangedDuration = _statChangedDuration;
    }

    public static ActiveStatChange CreateInstance(string _statChanged, int _statChangedEffect, int _statChangedDuration)
    {
        var data = ScriptableObject.CreateInstance<ActiveStatChange>();
        data.Init(_statChanged, _statChangedEffect, _statChangedDuration);
        return data;
    }
}