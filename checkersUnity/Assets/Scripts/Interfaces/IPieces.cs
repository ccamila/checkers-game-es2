using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPieces 
{
    void walk();
    int  getColor();
    void setColor(int colorId); // 0 for white, 1 for black
}
