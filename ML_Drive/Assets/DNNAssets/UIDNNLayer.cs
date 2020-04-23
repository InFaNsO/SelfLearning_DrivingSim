using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIDNNLayer : MonoBehaviour
{
    public RectTransform myArea;
    public GameObject neuronLook;
    [HideInInspector] public List<GameObject> myNeurons = new List<GameObject>();
    public TMP_InputField inputText;

    public UIDNN myNetwork = null;
    public GameObject myInputField;

    int numNeurons = 1;

    public void SetNumberOfNeurons(int num)
    {
        for(int i = 0; i < myNeurons.Count; ++i)
        {
            Destroy(myNeurons[i]);
        }
        myNeurons.Clear();
        for (int i = 0; i < num; ++i)
        {
            myNeurons.Add(Instantiate(neuronLook, transform));
            myNeurons[i].SetActive(true);
        }
        SpaceNeurons();
        inputText.text = num.ToString();
    }

    private void OnValidate()
    {
        //if(myNeurons.Count == 0)
        //{
        //    SetNumberOfNeurons(numNeurons);
        //}
        //if (inputText)
        //    ParseInputToIntForLayer(inputText.text);
    }

    private void OnGUI()
    {
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InputTextChanged()
    {
        string s = inputText.text;
        ParseInputToIntForLayer(ref s);
        if (s == "")
            inputText.text = s;
    }

    void ParseInputToIntForLayer(ref string input)
    {
        bool hasInt = input.All(char.IsDigit);
        if (!hasInt)
        {
            input = "";
            return;
        }
        int inputNum;
        int.TryParse(input, out inputNum);

        if (inputNum < 1)
            return;
        numNeurons = inputNum;

        if (myNeurons.Count != numNeurons)
        {
            for (int i = 0; i < myNeurons.Count; ++i)
            {
                Destroy(myNeurons[i]);
            }

            myNeurons.Clear();

            for (int i = 0; i < numNeurons; ++i)
            {
                myNeurons.Add(Instantiate(neuronLook, transform));
                myNeurons[i].SetActive(true);
            }

            SpaceNeurons();
            myNetwork.MakeConnections();
        }
    }

    void SpaceNeurons()
    {
        int yArea = (int)myArea.rect.height;
        int deltaY =  yArea / 20;   //circle area

        float startY = myNeurons.Count * deltaY * -0.5f;

        for(int i = 0; i < myNeurons.Count; ++i)
        {
            RectTransform rt = myNeurons[i].GetComponent<RectTransform>();
            rt.localPosition = new Vector3(0.0f, (float)i * deltaY + startY, 0.0f);
        }
    }
}
