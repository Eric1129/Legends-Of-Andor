using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System;

public class BoardPosition : MonoBehaviour
{
    private Vector2[] perimeter;
    // this is NOT physical location
    private int gameLocation;
    private bool currentlySelected = false;
    private LineRenderer lineRenderer;
    private PolygonCollider2D polyCollider;
    private float alpha = 0.2f;
    // physical location, in local coordinates
    private Vector3 middle;

    private int[] neighborBoardNumbers;
    // ie: == [12, 2, 4]
    // then use GameObject.FindWithTag(neighbor[i].ToString()) to traverse


    public BoardPosition(int boardNumber)
    {
        gameLocation = boardNumber;
    }


    public BoardPosition(int boardNumber, Vector2[] boundaryVertices)
    {
        gameLocation = boardNumber;
        perimeter = boundaryVertices;
    }


    public void init()
    {
        polyCollider = gameObject.GetComponent<PolygonCollider2D>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        perimeter = polyCollider.points;

        lineRenderer.positionCount = perimeter.Length;
        drawLine(false);

        // worked before I imported System.Drawing, but need that import
        // for calcCentroid fxn
        // should all manually collect middles of tiles to avoid this
        lineRenderer.material.color = UnityEngine.Color.green;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        PointF mid = calculateCentroid();
        middle = new Vector3(mid.X, mid.Y, transform.position.z);
    }


    public Vector3 getMiddle()
    {
        if (gameObject.tag.Equals("9"))
            return new Vector3(-34.0f, 29.4f, 0.0f);
        return middle;
        // return polyCollider.bounds.center;
    }


    void OnMouseDown()
    {
        masterClass master = GetComponentInParent<masterClass>();
        if (master.isCurrentlySelected(gameObject.tag))
            return;

        master.requestHighlight(gameObject.tag);
        // Todo: make this better for turns, need to pass who clicked somehow
        // should just only have highlight stuff here and no game logic
        // could add a million colliders to each player
        // or a button down check for each player each frame
        // followed by code to figure out which spot they clicked on
        master.setNewHeroLocation(getMiddle(), gameObject.tag);

        if(lineRenderer.enabled == false)
            lineRenderer.enabled = true;
        currentlySelected = true;
        // adjustTransperency();
    }


    // doesn't work although this is how everyone says to do :(
    // void adjustTransperency()
    // {
    //     UnityEngine.Color col = lineRenderer.material.color;
    //     if (!currentlySelected)
    //     {
    //         col.a = alpha;
    //         lineRenderer.material.color = col;
    //     }
    //     else
    //     {
    //         col.a = 1.0f;
    //         lineRenderer.material.color = col;
    //     }
    // }


    void OnMouseOver()
    {
        // adjustTransperency();
        lineRenderer.enabled = true;
    }


    void OnMouseExit()
    {
        // turn off border highlight if mouse isn't over this boardPosition
        if (!currentlySelected)
            lineRenderer.enabled = false;
    }


    public void hideBorderHighlight()
    {
        currentlySelected = false;
        lineRenderer.enabled = false;
    }


    void drawLine(bool shouldEnable)
    {
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        for (int i = 0; i < perimeter.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(perimeter[i].x, perimeter[i].y, -0.03f));
        }
        lineRenderer.loop = true;
        lineRenderer.enabled = shouldEnable;
        // now make em smooth
        lineRenderer.numCapVertices = perimeter.Length;
        // lineRenderer.numCornerVertices = perimeter.Length;
    }


// from https://stackoverflow.com/questions/9815699/how-to-calculate-centroid
 public PointF calculateCentroid()
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
        return PointF.Empty;  // Avoid division by zero

     accumulatedArea *= 3f;
     return new PointF(centerX / accumulatedArea, centerY / accumulatedArea);
  }
}
