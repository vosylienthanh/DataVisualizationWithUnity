using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataPlotter : MonoBehaviour
{
    [SerializeField]
    protected string inputFile = "Assets/Resources/iris.csv";
    [SerializeField]
    protected InputField XAxis;
    [SerializeField]
    protected InputField YAxis;
    [SerializeField]
    protected InputField ZAxis;
    [SerializeField]
    protected InputField CSVDir;
    [SerializeField]
    protected TextMeshProUGUI Name;
    [SerializeField]
    protected Terrain ground;

    private List<Dictionary<string, object>> pointList;
    
    public int columnX = 0;
    public int columnY = 1;
    public int columnZ = 2;
    
    public string xName;
    public string yName;
    public string zName;

    public float plotScale = 10;
    
    public GameObject PointPrefab;

    private List<GameObject> dataPoints = new List<GameObject>();
    
    void Start()
    {
        XAxis.text = columnX.ToString();
        YAxis.text = columnY.ToString();
        ZAxis.text = columnZ.ToString();
        CSVDir.text = inputFile;
        
        pointList = CSVReader.ReadCSVFile(inputFile);

        PlotDataPoint();   
    }

    public void PlotDataPoint()
    {
        try
        {

            columnX = int.Parse(XAxis.text);
            columnY = int.Parse(YAxis.text);
            columnZ = int.Parse(ZAxis.text);
            
            List<string> columnList = new List<string>(pointList[1].Keys);
            
            xName = columnList[columnX];
            yName = columnList[columnY];
            zName = columnList[columnZ];
            
            float xMax = FindMaxValue(xName);
            float yMax = FindMaxValue(yName);
            float zMax = FindMaxValue(zName);
            
            float xMin = FindMinValue(xName);
            float yMin = FindMinValue(yName);
            float zMin = FindMinValue(zName);

            //ground.transform.position.Set(0, yMin, 0);
            //ground.transform.SetPositionAndRotation(ground.transform.position + new Vector3(0, yMin, 0), ground.transform.rotation);
            
            for (var i = 0; i < pointList.Count; i++)
            {
                float x =
                    (System.Convert.ToSingle(pointList[i][xName]) - xMin)
                    / (xMax - xMin);

                float y =
                    (System.Convert.ToSingle(pointList[i][yName]) - yMin)
                    / (yMax - yMin);

                float z =
                    (System.Convert.ToSingle(pointList[i][zName]) - zMin)
                    / (zMax - zMin);
                
                GameObject dataPoint = Instantiate(
                        PointPrefab,
                        new Vector3(x, y, z) * plotScale,
                        Quaternion.identity);
                dataPoint.GetComponent<OnMouseClickSphere>().textMeshPro = Name;
                
                string dataPointName =
                    pointList[i][xName] + " - "
                    + pointList[i][yName] + " - "
                    + pointList[i][zName];
                
                dataPoint.transform.name = dataPointName;
                
                dataPoint.GetComponent<Renderer>().material.color = new Color(x, y, z, 1.0f);

                dataPoints.Add(dataPoint);
            }
        }
        catch (Exception ex)
        {
            Name.text = ex.Message;
        }
    }

    private float FindMaxValue(string columnName)
    {
        float maxValue = Convert.ToSingle(pointList[0][columnName]);
        
        for (var i = 0; i < pointList.Count; i++)
        {
            if (maxValue < Convert.ToSingle(pointList[i][columnName]))
                maxValue = Convert.ToSingle(pointList[i][columnName]);
        }
        
        return maxValue;
    }

    private float FindMinValue(string columnName)
    {

        float minValue = Convert.ToSingle(pointList[0][columnName]);
        
        for (var i = 0; i < pointList.Count; i++)
        {
            if (Convert.ToSingle(pointList[i][columnName]) < minValue)
                minValue = Convert.ToSingle(pointList[i][columnName]);
        }

        return minValue;
    }

    public void Reset()
    {
        inputFile = CSVDir.text;
        
        pointList = CSVReader.ReadCSVFile(inputFile);

        for (int i = dataPoints.Count - 1; i > -1; --i)
        {
            Destroy(dataPoints[i]);
            dataPoints.RemoveAt(i);
        }

        PlotDataPoint();
    }
}
