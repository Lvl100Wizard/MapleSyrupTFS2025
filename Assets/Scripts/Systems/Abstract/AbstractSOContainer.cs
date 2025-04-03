using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractSOContainer : ScriptableObject
{

    [Header("Scriptable Entity")] //shared by all child classes
    #region variables
    public string name;
    #endregion

}
