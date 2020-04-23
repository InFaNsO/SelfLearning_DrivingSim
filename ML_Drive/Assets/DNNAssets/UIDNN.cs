using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class UIDNN : MonoBehaviour
{
    [HideInInspector] public List<GameObject> myLayers = new List<GameObject>();
    List<GameObject> myConnections = new List<GameObject>();

    [HideInInspector] public int PopulationSize = 20;

    //[SerializeField] TextMeshProUGUI LayerSizeText;
    [SerializeField] RectTransform myUIArea = null;

    [SerializeField] GameObject Layer = null;
    [SerializeField] GameObject Connection = null;

    [SerializeField] TMP_InputField myText;
    [SerializeField] TMP_InputField popText;



    //bool wasActive = false;

    int mDefaultLayerSize = 2;
    int layerSize = 2;

    private void OnValidate()
    {
        //if(myLayers.Count == 0)
        //{
        //    AddLayers(2);
        //    PositionLayers();
        //    MakeConnections();
        //}
        //
        //ParseInputToIntForLayer(LayerSizeText.text);
    }

    void Start()
    {
        if (myLayers.Count < 3)
        {
            AddLayers(3);
            PositionLayers();
            MakeConnections();
            DisableLayer1nlast();
        }
    }

    public void InputTextChanged()
    {
        string s = myText.text;
        ParseInputToIntForLayer(ref s);
        if (s == "")
            myText.text = s;
    }

    public void PopulationTextChanged()
    {
        if(!int.TryParse(popText.text, out PopulationSize))
        {
            PopulationSize = 20;
            popText.text = PopulationSize.ToString();
        }
    }

    void ParseInputToIntForLayer(ref string input)
    {
        int inputNum = 0;
        if (!int.TryParse(input, out inputNum))
        { 
            input = "";
            return;
        }

        layerSize = mDefaultLayerSize + inputNum;
        
        if (myLayers.Count != layerSize)
        {
            if (myLayers.Count < layerSize)
                AddLayers(layerSize - myLayers.Count);
            if (myLayers.Count > layerSize)
                RemoveLayers(myLayers.Count - layerSize);
            
            PositionLayers();
            MakeConnections();

            DisableLayer1nlast();
        }
    }

    void AddLayers(int numLayersToAdd)
    {
        if(myLayers.Count < 3)
        {
            for (int i = 0; i < numLayersToAdd; ++i)
            {
                myLayers.Add(Instantiate(Layer, transform));
                UIDNNLayer dnnL = myLayers[i].GetComponent<UIDNNLayer>();
                dnnL.myNetwork = this;
                dnnL.myArea.sizeDelta = new Vector2(182, myUIArea.rect.height);
                dnnL.SetNumberOfNeurons(4);
            }
            return;
        }

        int index = myLayers.Count - 2;

        for (int i = 0; i < numLayersToAdd; ++i)
        {
            myLayers.Insert(index, Instantiate(Layer, transform));
            UIDNNLayer dnnL = myLayers[index].GetComponent<UIDNNLayer>();
            dnnL.myNetwork = this;
            dnnL.myArea.sizeDelta = new Vector2(182, myUIArea.rect.height);
            dnnL.SetNumberOfNeurons(4);

            index++;
        }
    }

    void RemoveLayers(int numLayersToRemove)
    {
        int startInd = myLayers.Count - numLayersToRemove - 1;

        for(int i = 0; i < numLayersToRemove; ++i)
        {
            Destroy(myLayers[startInd + i]);
            myLayers.RemoveAt(startInd + i);
        }

    }

    void PositionLayers()
    {
        float width = myUIArea.rect.width;
        float deltaX = width / myLayers.Count;
        float startX = myLayers.Count * -0.5f * myLayers[0].GetComponent<RectTransform>().rect.width * 0.5f;

        for (int i = 0; i < myLayers.Count; ++i)
        {
            RectTransform tL = myLayers[i].GetComponent<RectTransform>();
            deltaX = tL.rect.width;
            tL.anchoredPosition = myUIArea.position;
            tL.localPosition = new Vector3(startX + i * deltaX, 0.0f, 0.0f);
            tL.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 183);
            UIDNNLayer nnL = myLayers[i].GetComponent<UIDNNLayer>();
        }

    }

    void DeleteConnections()
    {
        for(int i = 0; i < myConnections.Count; ++i)
        {
            Destroy(myConnections[i]);
        }
        myConnections.Clear();
    }

    public void MakeConnections()
    {
        DeleteConnections();
        int connectionCount = 0;

        float halfWidth = myLayers[0].GetComponent<RectTransform>().rect.width * 0.5f;
        for(int i = 1; i < myLayers.Count; ++i)
        {
            UIDNNLayer prvLayer = myLayers[i - 1].GetComponent<UIDNNLayer>();
            UIDNNLayer currentLayer = myLayers[i].GetComponent<UIDNNLayer>();

            for (int prvIndex = 0; prvIndex < prvLayer.myNeurons.Count; ++prvIndex)
            {
                for (int myIndex = 0; myIndex < currentLayer.myNeurons.Count; ++myIndex)
                {
                    RectTransform prvNTransform = prvLayer.myNeurons[prvIndex].GetComponent<RectTransform>();
                    RectTransform curNTransform = currentLayer.myNeurons[myIndex].GetComponent<RectTransform>();

                    myConnections.Add(Instantiate(Connection, transform));
                    RectTransform lineTrans = myConnections[connectionCount].GetComponent<RectTransform>();
                    UILineRenderer line = myConnections[connectionCount].GetComponent<UILineRenderer>();
                    lineTrans.anchoredPosition = myUIArea.position;
                    lineTrans.localPosition = new Vector3(-183.0f * 0.5f, 0.0f, 0.0f);

                    line.Points[0].x = prvLayer.myArea.localPosition.x + halfWidth;
                    line.Points[0].y = prvNTransform.localPosition.y;

                    line.Points[1].x = currentLayer.myArea.localPosition.x + halfWidth;
                    line.Points[1].y = curNTransform.localPosition.y;

                    connectionCount++;
                }
            }
        }
    }

    void DisableLayer1nlast()
    {
        myLayers[0].GetComponent<UIDNNLayer>().myInputField.SetActive(false);
        myLayers[myLayers.Count - 1].GetComponent<UIDNNLayer>().myInputField.SetActive(false);
    }
}
