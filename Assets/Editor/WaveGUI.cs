using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(Wave))]
public class WaveGUI : Editor
{

    public VisualTreeAsset m_InspectorXML;

    /*public override VisualElement CreateInspectorGUI()
    {
        VisualElement myInspector = new VisualElement();
        m_InspectorXML.CloneTree(myInspector);
        return myInspector;
    }*/
}
