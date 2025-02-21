using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputPanel : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> inputs;

    public void UpdateInputs()
    {
        Debug.Log("Updating Inputs");
        int total = GameManager.Instance.tileInputs.Count;
        for (int i = 0; i < 12; ++i)
        {
            for (int j = 0; j < inputs[i].transform.childCount; ++j)
            {
                inputs[i].transform.GetChild(j).gameObject.SetActive(false);
            }
            if (i < total)
            {
                inputs[i].transform.GetChild(GameManager.Instance.tileInputs[i]).gameObject.SetActive(true);
            }

        }
    }
}
