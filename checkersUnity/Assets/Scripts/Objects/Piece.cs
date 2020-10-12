using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : IPieces
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildPieces()
    {
        
    }
}
