using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionManager {
    int getDiskNumber();
    void setDiskNumber(int num);
    void startThrow(Queue<GameObject> diskQueue);
}
