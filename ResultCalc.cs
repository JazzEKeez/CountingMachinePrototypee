using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCalc : MonoBehaviour
{
    public GameObject holdBox;
    public GameObject value1Box;
    public GameObject value2Box;
    public GameObject operatorBox;
    public GameObject spherePrefab;
    public bool resultCalculated;

    private float result;
    private Counter count1Script;
    private Counter count2Script;
    private OperatorState operatorStateScript;

    // Start is called before the first frame update
    void Start()
    {
        count1Script = value1Box.GetComponent<Counter>();
        count2Script = value2Box.GetComponent<Counter>();
        operatorStateScript = operatorBox.GetComponent<OperatorState>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Calculate()
    {
        resultCalculated = false;

        if (count1Script.count != 0 && count2Script.count != 0 && !resultCalculated)
        {
            if (operatorStateScript.GetCurrentOpperatorState() == OperatorState.OperatorType.Adding)
            {
                result = count1Script.count + count2Script.count;
                Debug.Log("result = " + result);
                InstantiateResultSpheres(1);
                DestroySpheresInCup(value1Box);
                DestroySpheresInCup(value2Box);
            }

            if (operatorStateScript.GetCurrentOpperatorState() == OperatorState.OperatorType.Subtracting)
            {
                result = count1Script.count - count2Script.count;
                Debug.Log("result = " + result);
                InstantiateResultSpheres(1);
                InstantiateReturnSphere(2);
                DestroySpheresInCup(value1Box);
                DestroySpheresInCup(value2Box);
            }

            if (operatorStateScript.GetCurrentOpperatorState() == OperatorState.OperatorType.Multiplying)
            {
                result = count1Script.count * count2Script.count;
                int setCount = Mathf.FloorToInt(count2Script.count);
                float remainder = count2Script.count - setCount;
                Debug.Log("result = " + result);

                StartCoroutine(InstantiateMultiplySpheres(setCount, remainder,() => { DestroySpheresInCup(value1Box); }));
                StartCoroutine (InstantiateBalancingSpheres(setCount - 1, remainder));

                if (resultCalculated)
                {
                    DestroySpheresInCup(value1Box);
                }
                
            }

            if (operatorStateScript.GetCurrentOpperatorState() == OperatorState.OperatorType.Dividing)
            {
                result = count1Script.count / count2Script.count;
                int setCount = Mathf.FloorToInt(count2Script.count);
                float remainder = count2Script.count - setCount;

                Debug.Log("result = " + result);
                InstantiateDivideSpheres(setCount, remainder);
                DestroySpheresInCup(value1Box);

            }
        }
    }
    // Instantiate result sphere for simple addition and subtraction operations

    private void InstantiateResultSpheres(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject resultSphere = Instantiate(spherePrefab, transform);
            resultSphere.transform.localScale = Vector3.one * (result * 0.5f);
            SphereScript sphereScript = resultSphere.GetComponent<SphereScript>();
            sphereScript.SetValue(result);
        }
    }

    private void InstantiateReturnSphere(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject returnedSphere = Instantiate(spherePrefab, holdBox.transform);
            returnedSphere.transform.localScale = Vector3.one * (count2Script.count * 0.5f);
            SphereScript sphereScript = returnedSphere.GetComponent<SphereScript>();
            sphereScript.SetValue(count2Script.count);
        }
    }

    private void InstantiateDivideSpheres(int count, float remainder)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject divideSphere = Instantiate(spherePrefab, transform);
            divideSphere.transform.localScale = Vector3.one * (result * 0.5f);
            SphereScript sphereScript = divideSphere.GetComponent<SphereScript>();
            sphereScript.SetValue(result);
        }

        if (remainder > 0)
        {
            GameObject remainderSphere = Instantiate(spherePrefab, transform);
            remainderSphere.transform.localScale = Vector3.one * (result * remainder);
            SphereScript sphereScript = remainderSphere.GetComponent<SphereScript>();
            sphereScript.SetValue(result * remainder);
        }
    }

    private IEnumerator InstantiateMultiplySpheres(int count, float remainder, Action callback )
    {
        SphereScript[] spheres = value1Box.GetComponentsInChildren<SphereScript>();

        for (int i = 0; i < spheres.Length; i++)
        {
            for (int j = 0; j < count; j++)
            {
                GameObject multiplySphere = Instantiate(spherePrefab, transform);
                multiplySphere.transform.localScale = Vector3.one * (spheres[i].value * 0.5f);
                SphereScript sphereScript = multiplySphere.GetComponent<SphereScript>();
                sphereScript.SetValue(spheres[i].value);

                yield return new WaitForSeconds(0.1f);
            }
        }

        if(remainder != 0)
        {
            for (int i = 0; i < spheres.Length; i++)
            {
                GameObject fractionalSphere = Instantiate(spherePrefab, transform);
                fractionalSphere.transform.localScale = Vector3.one * remainder * 0.5f;
                SphereScript sphereScript = fractionalSphere.GetComponent<SphereScript>();
                sphereScript.SetValue(spheres[i].value);
            }

            yield return new WaitForSeconds(0.1f);
        }

        resultCalculated = true;
        callback?.Invoke();

        yield break;
    }
    private IEnumerator InstantiateBalancingSpheres(int count, float remainder)
    {
        SphereScript[] spheres = value1Box.GetComponentsInChildren<SphereScript>();

        for (int i = 0; i < spheres.Length; i++)
        {
            for (int j = 0; j < count; j++)
            {
                GameObject multiplySphere = Instantiate(spherePrefab, holdBox.transform);
                multiplySphere.transform.localScale = Vector3.one * (spheres[i].value * 0.5f);
                SphereScript sphereScript = multiplySphere.GetComponent<SphereScript>();
                sphereScript.SetValue(spheres[i].value * -1);

                yield return new WaitForSeconds(0.1f);
            }
        }

        for (int i = 0; i < spheres.Length; i++)
        {
            GameObject fractionalSphere = Instantiate(spherePrefab, holdBox.transform);
            fractionalSphere.transform.localScale = Vector3.one * remainder * 0.5f;
            SphereScript sphereScript = fractionalSphere.GetComponent<SphereScript>();
            sphereScript.SetValue(spheres[i].value * -1);

            yield return new WaitForSeconds(0.1f);
        }

        yield break;
    }
    private void DestroySpheresInCup(GameObject cup)
    {
        SphereScript[] spheres = cup.GetComponentsInChildren<SphereScript>();
        Counter counter = cup.GetComponent<Counter>();
        Text cupCountText = cup.GetComponent<Counter>().CounterText;

        // Destroy spheres in cup
        foreach (SphereScript sphere in spheres)
        {
            Destroy(sphere.gameObject);
        }

        // Set sphere count in cup to 0
        counter.count = 0;
        cupCountText.text = "" + counter.count;
    }
}
