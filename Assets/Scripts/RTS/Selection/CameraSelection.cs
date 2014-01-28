using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using System.Collections;

public class CameraSelection : MonoBehaviour
{
    public Texture SelectionGraphics;
    public Rect SelectionRect;

    public List<GameObject> SelectableUnits;

    private Vector2 selectionOrigin;
    private Vector2 selectionSize;

    private Rect backupRect;

    private GameManager gameManager;

    void OnGUI()
    {
        if (gameManager.GameMode == GameModeType.RTS)
        {
            SelectionRect = new Rect(selectionOrigin.x, selectionOrigin.y,
                selectionSize.x, selectionSize.y);

            GUI.color = new Color(0, 0, 0, .3f);
            GUI.DrawTexture(SelectionRect, SelectionGraphics);
        }
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	void Update()
    {
        if (gameManager.GameMode == GameModeType.RTS)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SelectableUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("SelectableUnit"));

                float _invertedY = Screen.height - Input.mousePosition.y;
                selectionOrigin = new Vector2(Input.mousePosition.x, _invertedY);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    SelectableUnits.Remove(hit.transform.gameObject);

                    if (hit.transform.gameObject.GetComponent<SelectableEntity>() == null)
                    {
                        // Debug.LogError("Object: " + hit.transform.gameObject.name + " is not selectable!");
                    }
                    else
                        hit.transform.gameObject.GetComponent<SelectableEntity>().OnSelect();
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                SelectionRect.width = 0;
                SelectionRect.height = 0;
                backupRect.width = 0;
                backupRect.height = 0;
                selectionSize = Vector2.zero;
            }

            if (Input.GetMouseButton(0))
            {
                float _invertedY = Screen.height - Input.mousePosition.y;
                selectionSize = new Vector2(Input.mousePosition.x - selectionOrigin.x, (selectionOrigin.y - _invertedY) * -1);

                if (SelectionRect.width < 0)
                {
                    backupRect = new Rect(SelectionRect.x - Mathf.Abs(SelectionRect.width), SelectionRect.y,
                        Mathf.Abs(SelectionRect.width), SelectionRect.height);
                }
                else if (SelectionRect.height < 0)
                {
                    backupRect = new Rect(SelectionRect.x, SelectionRect.y - Mathf.Abs(SelectionRect.height),
                        SelectionRect.width, Mathf.Abs(SelectionRect.height));
                }

                if (SelectionRect.width < 0 && SelectionRect.height < 0)
                {
                    backupRect = new Rect(SelectionRect.x - Mathf.Abs(SelectionRect.width),
                        SelectionRect.y - Mathf.Abs(SelectionRect.height), Mathf.Abs(SelectionRect.width),
                        Mathf.Abs(SelectionRect.height));
                }

                foreach (GameObject unit in SelectableUnits)
                {
                    Vector3 _screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);
                    Vector2 _screenPoint = new Vector2(_screenPos.x, Screen.height - _screenPos.y);

                    if (!SelectionRect.Contains(_screenPoint) || !backupRect.Contains(_screenPoint))
                    {
                        if (unit.GetComponent<SelectableEntity>() == null)
                        {
                            //  Debug.LogError("Object: " + unit.name + " is not selectable!");
                        }
                        else
                            unit.GetComponent<SelectableEntity>().OnUnSelect();
                    }
                    if (SelectionRect.Contains(_screenPoint) || backupRect.Contains(_screenPoint))
                    {
                        if (unit.GetComponent<SelectableEntity>() == null)
                        {
                            //   Debug.LogError("Object: " + unit.name + " is not selectable!");
                        }
                        else
                            unit.GetComponent<SelectableEntity>().OnSelect();
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                SelectableUnits = new List<GameObject>(GameObject.FindGameObjectsWithTag("SelectableUnit"));


                foreach (var selectableUnit in SelectableUnits)
                {
                    if (selectableUnit.GetComponent<SelectableEntity>() == null)
                    {
                        // Debug.LogError("Object: " + selectableUnit.name + " is not selectable!");
                    }
                    else
                        selectableUnit.GetComponent<SelectableEntity>().GoTo();
                }
            }
        }
    }
}
