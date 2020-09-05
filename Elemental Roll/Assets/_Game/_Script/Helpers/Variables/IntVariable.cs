
using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject {

    public int value;

    public void SetValue(int _value)
    {
        value = _value;
    }

    public void SetValue(IntVariable _value)
    {
        value = _value.value;
    }

    public void ApplyChange(int amount)
    {
        value += amount;
    }

    public void ApplyChange(IntVariable amount)
    {
        value += amount.value;
    }

}
