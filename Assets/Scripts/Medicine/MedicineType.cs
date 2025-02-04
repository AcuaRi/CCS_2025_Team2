using System;

[Flags]
public enum MedicineType
{
    None = 0,
    Medicine1 = 1 << 0,
    Medicine2 = 1 << 1,
    Medicine3 = 1 << 2,
    Medicine4 = 1 << 3,
    Medicine5 = 1 << 4,
    Medicine6 = 1 << 5,
    Medicine7 = 1 << 6,
}
