using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public float circleCastRadius = 0.2f;

    public CatBehaviour pickedCat;

    private bool foodStolen = false;

    private void Start()
    {
        AddListeners();
    }

    void Update()
    {
        if (foodStolen) { return; }

        if (pickedCat != null)
        {
            // look for mouse button to release picked cat
            if (Input.GetMouseButtonUp(0))
            {
                pickedCat.SetState(CatBehaviour.CatStates.FALLING);
                pickedCat = null;
            }
        }
        else
        {
            // look for cat to pick

            Vector2 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Collider2D[] allCol = Physics2D.OverlapCircleAll(worldPos, circleCastRadius);

            Debug.DrawRay(worldPos, Vector3.up * circleCastRadius);

            foreach (Collider2D col in allCol)
            {
                if (col != null)
                {
                    CatBehaviour cat = col.gameObject.GetComponent<CatBehaviour>();
                    if (cat != null)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            pickedCat = cat;
                            pickedCat.SetState(CatBehaviour.CatStates.PICKED);
                            return;
                        }
                    }
                }
            }
        }
    }

    private void AddListeners()
    {
        SceneHandler.onSceneChange += SubtractListeners;
        FoodBehaviour.onCatSteal += OnCatSteal;
    }

    public void SubtractListeners()
    {
        SceneHandler.onSceneChange -= SubtractListeners;
        FoodBehaviour.onCatSteal -= OnCatSteal;
    }

    public void OnCatSteal()
    {
        foodStolen = true;
    }
}
