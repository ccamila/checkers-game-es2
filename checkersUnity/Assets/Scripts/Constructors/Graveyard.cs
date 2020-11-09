using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Graveyard : MonoBehaviour
{
    // Start is called before the first frame update

    int rowsLenght = 4;
    int colsLenght = 3;
    public int spaceBetweenPieces = -2;
    private int countPiecesGraveyard = 0;
    private Vector3[] GraveyardPositions= new Vector3[12];

    void Start()
    {
        
        float posY = transform.position.y;
        float posX = transform.position.x;
        float posZ = transform.position.z;
        int posIndex = 0;

        for (int i = 0; i < rowsLenght; i++)
        {
            for (int j = 0; j < colsLenght; j++)
            {
                posX += spaceBetweenPieces;
                GraveyardPositions[posIndex] = new Vector3(posX, posY, posZ);
                posIndex++;

            }

            posX -= (colsLenght) * spaceBetweenPieces;
            posY += spaceBetweenPieces;
        }

}

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Vector3 GetNewPosition()
    {
        return GraveyardPositions[countPiecesGraveyard++];
    }
}
