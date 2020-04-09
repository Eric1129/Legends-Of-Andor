using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Drawing;
using System;

public class BoardPosition : MonoBehaviour
{

    // could eventually change this to a more descriptive name
    // like: boardPosComponentHandler
    // this class just deals with lineRenderer, colliders etc

    private PolygonCollider2D polyCollider;
    private LineRenderer lineRenderer;

    private Vector2[] perimeter;
    private bool selected = false;

    public static BoardPosition currentlySelected;

    public int tileID = -1;

    // physical location, in local coordinates (where sphere ie player appears)
    private Vector3 middle;

    // note this will not be used, neighbours stored in Graph/BoardContents classes.
    private int[] neighborBoardNumbers;
    // ie: == [12, 2, 4]
    // then use GameObject.FindWithTag(neighbor[i].ToString()) to traverse



    private void setClassAttributes()
    {
        polyCollider = gameObject.GetComponent<PolygonCollider2D>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        perimeter = polyCollider.points;

        lineRenderer.positionCount = perimeter.Length;

        // worked before I imported System.Drawing, but need that import
        // for calcCentroid fxn
        // should all manually collect middles of tiles to avoid this
        lineRenderer.material.color = UnityEngine.Color.green;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }


    // called by MasterClass.
    public void init(int id)
    {
        setClassAttributes();
        setLineRendererAttributes();

        setPerimeterLinePositions();

        PointF mid = calculateCentroid();
        middle = new Vector3(mid.X, mid.Y, -1);

        tileID = id;
    }


    public Vector3 getMiddle()
    {
        // eventually we will manually record the middle of each
        // boardPosition so that it looks nice
        // -- calculating centroid or other returning polyCollider.bounds.center;
        // doesn't work too well sometimes
        if (gameObject.tag.Equals("9"))
            return new Vector3(-34.0f, 29.4f, 0.0f);
        return middle;
        // return polyCollider.bounds.center;
    }


    void OnMouseDown()
    {
        // Remove last selected
        if(BoardPosition.currentlySelected == null)
        {
            BoardPosition.currentlySelected = this;
        }
        else if(!this.name.Equals(BoardPosition.currentlySelected.name))
        {
            BoardPosition.currentlySelected.lineRenderer.enabled = false;
            BoardPosition.currentlySelected = this;
        }


        if(lineRenderer.enabled == false)
            lineRenderer.enabled = true;
        selected = true;

        GameController.instance.tileClick(this);
    }

    void OnMouseOver()
    {
        // adjustTransperency();
        lineRenderer.enabled = true;
    }


    void OnMouseExit()
    {
        // turn off border highlight if mouse isn't over this boardPosition
        if (BoardPosition.currentlySelected == null || !this.name.Equals(BoardPosition.currentlySelected.name))
        {
            this.lineRenderer.enabled = false;
        }


        if (!selected)
            lineRenderer.enabled = false;
    }


    public void hideBorderHighlight()
    {
        selected = false;
        lineRenderer.enabled = false;
    }


    private void setLineRendererAttributes()
    {
        lineRenderer.startWidth = 0.3f;
        lineRenderer.endWidth = 0.3f;
        lineRenderer.enabled = false;

    }


    void setPerimeterLinePositions()
    {
        for (int i = 0; i < perimeter.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(perimeter[i].x, perimeter[i].y, -0.03f));
        }
        lineRenderer.loop = true;

        // now make em smooth
        lineRenderer.numCapVertices = perimeter.Length;
    }


// from https://stackoverflow.com/questions/9815699/how-to-calculate-centroid
// shouldn't use this, looks shitty at times
// -- need to manually record where we want th middle of each boardPosition to be.
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
