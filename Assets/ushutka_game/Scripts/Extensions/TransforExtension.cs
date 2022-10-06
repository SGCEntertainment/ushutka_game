using UnityEngine;

public static class TransforExtension
{
    const float maxX = 25;
    const float maxY = 25;

    public static (Vector2, Quaternion) GetPositionAndRotaion(this Transform _)
    {
        float x = Random.Range(-maxX, maxX);
        float y = Random.Range(-maxY, maxY);

        Vector2 position = new Vector2(x, y);
        Quaternion rotation = Quaternion.Euler(Vector3.forward * Random.Range(0, 360.0f));
        return (position, rotation);
    }
}