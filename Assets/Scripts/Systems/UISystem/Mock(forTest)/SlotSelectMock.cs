using UnityEngine;
using UnityEngine.InputSystem;

public class SlotSelectMock : MonoBehaviour
{
    void Update()
    {
        HandleBlockSelectionInput();
    }
    
    private void HandleBlockSelectionInput()
    {
        for (int i = 0; i < 9; i++)
        {
            Key key = GetKeyForIndex(i + 1);
            if (Keyboard.current[key].wasPressedThisFrame)
            {
                UIManager.Instance.SelectSlot(i);
                break;
            }
        }
    }
    
    private Key GetKeyForIndex(int index)
    {
        switch (index)
        {
            case 1: return Key.Digit1;
            case 2: return Key.Digit2;
            case 3: return Key.Digit3;
            case 4: return Key.Digit4;
            case 5: return Key.Digit5;
            case 6: return Key.Digit6;
            case 7: return Key.Digit7;
            case 8: return Key.Digit8;
            case 9: return Key.Digit9;
            default: return Key.None;
        }
    }
}
