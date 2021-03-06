﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public Vector3 target;
    public Collider moveColider;
    public NavMeshAgent agent;
    public Vector3 velocity;

    [Header("Scale Managment")]
    public float characterMinSize;
    public float characterMaxSize;
    public float x;
    public float scaleModifier;
    public float screenAmount = 0.7f;
    public Transform scaleSprite;

    [Header("Sorting")]
    public SpriteRenderer sr;

    [Header("Other")]
    public UIManager ui;

    public Sprite front;
    public Sprite back;
    public Sprite left;
    public Sprite right;

    void Update()
    {
        // Die Pixelwerte der Maus werden in die Weltposition umgerechnet
        Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Wir ziehen aus der 3D Position die für uns relevanten 2D Werte der X und Y Achse
        Vector2 mousePosWorld2D = new Vector2(mousePosWorld.x, mousePosWorld.y);

        // Wenn unsere linke Maustaste gedrückt wird
        if (Input.GetMouseButtonDown(0) && !ui.commandMenu.activeSelf)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            // Wir machen den Raycast in die Szene an der Stelle wo unsere Maus sich befindet
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.collider.gameObject.name);
                //target = hit.point;
                agent.SetDestination(hit.point);

                if (hit.collider == moveColider)
                {
                    // Wir setzen die ZielPosition unseres Characters an die getroffene Stelle
                    
                }
            }
            



        }

        #region display sprite
        // Richtige Darstellung der Sprites
        velocity = agent.velocity;
        // Wenn sie nach vorne geht
        if (agent.velocity.z < 0 && (Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.z)))
        {
            sr.sprite = front;
        }
        // Wenn sie nach hinten geht
        else if (agent.velocity.z > 0 && (Mathf.Abs(agent.velocity.x) < Mathf.Abs(agent.velocity.z)))
        {
            sr.sprite = back;
        }
        // Wenn sie nach rechts geht
        else if (agent.velocity.x > 0 && (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.z)))
        {
            sr.sprite = right;
        }
        // Wenn sie nach links geht
        else if (agent.velocity.x < 0 && (Mathf.Abs(agent.velocity.x) > Mathf.Abs(agent.velocity.z)))
        {
            sr.sprite = left;
        }
        // Wenn sie stehen bleibt
        else if (agent.velocity.z == 0)
        {
            sr.sprite = front;
        }
        #endregion

        // Die Skalierung des Characters anhand der Position
        x = Mathf.InverseLerp((Screen.height * screenAmount), 0f, Camera.main.WorldToScreenPoint(transform.position).y);
        scaleModifier = Mathf.Lerp(characterMinSize, characterMaxSize, x);
        scaleSprite.transform.localScale = new Vector3(scaleModifier, 1f , scaleModifier);

        // Die Platzierung des Characters im Sorting
        RaycastHit rayHit;

        if (Physics.Raycast(agent.transform.position, Vector3.down, out rayHit))
        {
            switch (rayHit.collider.name)
            {
                case "Layer1":
                    sr.sortingOrder = 3;
                    Debug.Log("Layer1");
                    break;
                case "Layer2":
                    sr.sortingOrder = 2;
                    Debug.Log("Layer2");
                    break;
                case "Layer3":
                    sr.sortingOrder = 1;
                    Debug.Log("Layer3");
                    break;
                case "Layer4":
                    sr.sortingOrder = 0;
                    Debug.Log("Layer4");
                    break;
                default:
                    sr.sortingOrder = 4;
                    break;
            }
            
        }
    }
}
