using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatorState : MonoBehaviour
{
    public GameObject addObjectPrefab;
    public GameObject subtractObjectPrefab;
    public GameObject multiplyObjectPrefab;
    public GameObject divideObjectPrefab;
    public Text operatorText;

    public enum OperatorType
    {
        None,
        Adding,
        Subtracting,
        Multiplying,
        Dividing,
        Overload
    }

    public OperatorType operatorState = OperatorType.None;
    private List<GameObject> objectsInCup = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Operator"))
        {
            objectsInCup.Add(other.gameObject);

            operatorState = DetermineOperatorState();
            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Operator"))
        {
            objectsInCup.Remove(other.gameObject);

            operatorState = DetermineOperatorState();
        }
    }

    private OperatorType DetermineOperatorState()
    {
        bool hasAddingObject = objectsInCup.Contains(addObjectPrefab);
        bool hasSubtractingObject = objectsInCup.Contains(subtractObjectPrefab);
        bool hasMultiplyingObject = objectsInCup.Contains(multiplyObjectPrefab);
        bool hasDividingObject = objectsInCup.Contains(divideObjectPrefab);

        switch (objectsInCup.Count)
        {
            case 0:
                return OperatorType.None;
            case 1:
                if (hasAddingObject)
                {
                    operatorText.text = "+";
                    return OperatorType.Adding;
                }
                else if (hasSubtractingObject)
                {
                    operatorText.text = "-";
                    return OperatorType.Subtracting;
                }
                else if (hasMultiplyingObject)
                {
                    operatorText.text = "X";
                    return OperatorType.Multiplying;
                }
                else if (hasDividingObject)
                {
                    operatorText.text = "/";
                    return OperatorType.Dividing;
                }
                break;
            default:
                operatorText.text = "Error";
                return OperatorType.Overload;
        }
        return OperatorType.None;
    }

    public OperatorType GetCurrentOpperatorState()
    {
        return operatorState;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
