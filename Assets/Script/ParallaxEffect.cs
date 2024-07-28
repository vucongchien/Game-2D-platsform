using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxeffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;


    //Vị trí bắt đầu của đối tượng trò chơi parallax
    Vector2 startingPosition;

    // Giá trị Z bắt đầu của đối tượng trò chơi parallax
    float startingZ;

    // Khoảng cách mà camera đã di chuyển từ vị trí bắt đầu của đối tượng parallax
    Vector2 camMoveSinceStart => (Vector2)cam.transform.position-startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    // Nếu đối tượng ở phía trước mục tiêu, sử dụng mặt phẳng clip gần. Nếu ở phía sau mục tiêu,  sử dụng farClipPlane
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    // Đối tượng càng xa người chơi thì đối tượng ParallaxEffect sẽ di chuyển càng nhanh. Kéo giá trị Z của nó đến gần mục tiêu hơn để khiến nó di chuyển chậm hơn.
    float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;
    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;

    }

    // Update is called once per frame
    void Update()
    {
        // When the target moves, move the parallax object the same distance times a multiplier
        Vector2 newPosition = startingPosition + camMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
