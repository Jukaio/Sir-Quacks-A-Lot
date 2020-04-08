using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marinasFileDump : MonoBehaviour
{
    public float speed;

    // http://canvas.projekti.info/ebooks/3D%20Math%20Primer%20for%20Graphics%20and%20Game%20Development%20(2nd%20Ed)(gnv64).pdf Chapter 1 and 2;
    private void Update()
    {
        Vector3 temp = Vector3.zero; // Your bloody C# course

        bool moveLeft;
        moveLeft = Input.GetKey("a");
        if (moveLeft == true)
        {
            temp += Vector3.left; // The book: Vector addition 
        }

        bool moveRight;
        moveRight = Input.GetKey("d");
        if (moveRight == true)
        {
            temp += Vector3.right; // The book: Vector addition 
        }

        bool moveUp;
        moveUp = Input.GetKey("w");
        if (moveUp == true)
        {
            temp += Vector3.up; // The book: Vector addition 
        }

        bool moveDown;
        moveDown = Input.GetKey("s");
        if (moveDown == true)
        {
            temp += Vector3.down; // The book: Vector addition 
        }
        temp.Normalize(); // The book: Ctrl + F and search "Normalize"

        gameObject.transform.position += temp * speed * Time.deltaTime; // The book: Vector multiplication with scalar
    }
}
