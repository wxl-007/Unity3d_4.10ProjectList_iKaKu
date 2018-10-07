using UnityEngine;
using System.Collections;

public interface IItemValue
{
    void SetItemValue(object item);
    object GetItemValue();
}