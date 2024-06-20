using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePopup : MonoBehaviour
{
    public virtual void Hide()
    {
        Destroy(gameObject);
    }
}