using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LayoutElement))]
public class ListNodeGeneric<M> : MonoBehaviour
{

    M data;

    #region Properties

    virtual public M Data
    {
        get
        {
            return data;
        }

        set
        {
            data = value;
        }
    }

    public bool Active
    {
        get
        {
            return gameObject.activeSelf;
        }

        set
        {
            gameObject.SetActive(value);
        }
    }
    #endregion Properties
}
