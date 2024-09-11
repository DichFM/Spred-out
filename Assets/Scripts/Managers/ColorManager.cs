using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;
    public Color[] Colors;
    public Color DefaultColor;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }


    public Color GetRandomColor()
    {
        return Colors[Random.Range(0, Colors.Length)];
    }
}

