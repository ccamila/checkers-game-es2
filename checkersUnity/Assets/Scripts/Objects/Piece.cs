using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour, IPieces
{
    [SerializeField]
    private Material[] colorsTopaint;
    public int getColor()
    {
        throw new System.NotImplementedException();
        return 0;
    }

    public void setColor(int colorId)
    {
        Color colorImplementation = Color.black;

        if (colorId == 1)
        {
            colorImplementation = Color.white;
        } 
        throw new System.NotImplementedException();
    }

    public void walk()
    {
        throw new System.NotImplementedException();
    }

    public void BuildPieces()
    {
        
    }
}
