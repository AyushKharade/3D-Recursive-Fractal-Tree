using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    Transform camParent;
    // Start is called before the first frame update
    void Start()
    {
        camParent = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        CamControls();
    }

    void CamControls()
    {
        if(rotateCam)
            camParent.Rotate(new Vector3(0, 0.5f, 0));

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            transform.Translate(transform.up * 4f * Time.deltaTime);
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            transform.Translate(transform.up * -4f * Time.deltaTime);
    }

    bool rotateCam = true;
    public void ToggleCamRotation()
    {
        rotateCam = !rotateCam;
    }

    public void ZoomIn()
    {
        if (GetComponent<Camera>().fieldOfView > 10)
            GetComponent<Camera>().fieldOfView -= 10;
    }
    public void ZoomOut()
    {
        if(GetComponent<Camera>().fieldOfView<90)
            GetComponent<Camera>().fieldOfView += 10;   
    }
    //public void ZoomOut() { transform.Translate(transform.forward * -5f); }
}
