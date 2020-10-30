using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class DummyMovement : MonoBehaviour
{

    float t;
    GameObject pieceGO;
    Vector3 startPosition;
    Vector3 targetPosition;
    Vector3 middlePosition;
    float timeToReachTarget;

    // Start is called before the first frame update
    void Start()
    {
        pieceGO = transform.GetChild(0).gameObject;

        startPosition = targetPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z >= 15 && !pieceGO.GetComponent<Animator>().GetBool("IsLady"))
        {
            UnityEngine.Debug.Log("Virou lady");
            SetLady();
        }

        t += Time.deltaTime / timeToReachTarget;
        //UnityEngine.Debug.Log("t = " + t);

        // ===> middlePosition serve para levantar a peça durante o movimento
        if (t < 0.5) transform.position = Vector3.Lerp(startPosition, middlePosition, t*2);
        else transform.position = Vector3.Lerp(middlePosition, targetPosition, (t-0.5f)*2);
        
        //Volta o estado da animação para Idle
        if (t > 1) pieceGO.GetComponent<Animator>().SetBool("IsMoving", false);
        
    }

    public void SetDestination(Vector3 destination, float time)
    {
        UnityEngine.Debug.Log("Setou novo destino");

        //Muda o estado da animação para "Movement"
        pieceGO.GetComponent<Animator>().SetBool("IsMoving", true);

        t = 0;
        startPosition = transform.position;
        timeToReachTarget = time;
        targetPosition = destination;

        // ===> middlePosition serve para levantar a peça durante o movimento
        middlePosition = new Vector3(startPosition.x + (targetPosition.x - startPosition.x)/2,
                            startPosition.y + 1,
                            startPosition.z + (targetPosition.z - startPosition.z) / 2);
        /*
        UnityEngine.Debug.Log("targetPosition = " + targetPosition);
        UnityEngine.Debug.Log("startPosition = " + startPosition);
        UnityEngine.Debug.Log("middlePosition = " + middlePosition);*/
    }

    public void SetLady()
    {
        pieceGO.GetComponent<Animator>().SetBool("IsLady", true);
    }
}
