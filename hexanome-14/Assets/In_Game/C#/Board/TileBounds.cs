using UnityEngine;
using System.Collections;
using System;

public class TileBounds
{
    private Vector2[] perimeter;
    public TileBounds(PolygonCollider2D polyCollider)
    {
        perimeter = polyCollider.points;
    }
    public Vector3 getSize()
    {
        float minX = perimeter[0].x;
        float maxX = perimeter[0].x;

        float minY = perimeter[0].y;
        float maxY = perimeter[0].y;

        for(int i = 1; i< perimeter.Length; i++)
        {
            if (perimeter[i].x < minX)
            {
                minX = perimeter[i].x;
            }
            if (perimeter[i].x > maxX)
            {
                maxX = perimeter[i].x;
            }
            if (perimeter[i].y < minY)
            {
                minY = perimeter[i].y;
            }
            if (perimeter[i].y > maxY)
            {
                maxY = perimeter[i].y;
            }
        }

        return new Vector3(maxX - minX, maxY - minY, 1);
    }
    public Bounds createBounds()
    {
        return new Bounds(calculateCentroid(), getSize());
    }

    public Vector3 calculateCentroid()
    {
        float accumulatedArea = 0.0f;
        float centerX = 0.0f;
        float centerY = 0.0f;

        for (int i = 0, j = perimeter.Length - 1; i < perimeter.Length; j = i++)
        {
            float temp = perimeter[i].x * perimeter[j].y - perimeter[j].x * perimeter[i].y;
            accumulatedArea += temp;
            centerX += (perimeter[i].x + perimeter[j].x) * temp;
            centerY += (perimeter[i].y + perimeter[j].y) * temp;
        }

        if (Math.Abs(accumulatedArea) < 1E-7f)
            return new Vector3();  // Avoid division by zero

        accumulatedArea *= 3f;
        return new Vector3(centerX / accumulatedArea, centerY / accumulatedArea);
    }
}
