using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Medicine_1 : Bullet
{

    protected override void Init()
    {
        _medicineType = MedicineType.Medicine1;
    }
}
