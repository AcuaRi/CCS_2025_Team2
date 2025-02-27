using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellLine : MonoBehaviour
{
    public List<Cell> cells = new List<Cell>();
    
    private void Start()
    {
        StartCoroutine(RecoveryCheckRoutine()); 
    }

    private IEnumerator RecoveryCheckRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            CheckCells();
        }
    }

    private void CheckCells()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (cells[i].RecoveryTimer <= 0) continue; 
            
            if (HasActiveNeighbor(i))
            {
                cells[i].DecreaseRecoveryTimer(1);
                var random = Random.Range(0f, 1f);
                if (random >= (float)cells[i].RecoveryTimer / cells[i].RecoveryTime)
                {
                    cells[i].RecoveryCell(); 
                }
            }
            else
            {
                //cells[i].ResetRecoveryTimer();
                var random = Random.Range(0f, 1f);
                if (random >= 0.99f)
                {
                    cells[i].RecoveryCell(); 
                }
            }
        }
    }

    private bool HasActiveNeighbor(int index)
    {
        if (index == 0 || index == cells.Count - 1) return true;
        
        bool leftActive = (index > 0) && cells[index - 1].RecoveryTimer == 0;
        bool rightActive = (index < cells.Count - 1) && cells[index + 1].RecoveryTimer == 0;
        return leftActive || rightActive;
    }
}