using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Ayush Kharade

public class Driver : MonoBehaviour
{
    // variables
    [Header("Variables")]
    //public int depth;
    public GameObject nodePrefab;          // 3D model of the segment to be used, has a collider on it.

    bool treeInitialized;                  // boolean to check if a tree has been created, if yes, inputs to create dynamic branches is enabled.

    // UI Variables
    public InputField depthInput;
    public Dropdown depthInputDropDown;
    public Text startButton;
    public Text dynamicBranchUI;
   
    void Start()
    {

        dynamicBranchUI.enabled = false;
    }

    private void Update()
    {

        if (treeInitialized)                      // Check every frame, once tree is created, accept any clicking inputs.
        {
            MouseInput();
        }
        
    }

    /// <summary>
    /// Main function to create the recursive tree.
    /// </summary>
    /// <param name="depth"> How many times the tree branches</param>
    /// <param name="position"> Position of the segment in 3D Space</param>
    /// <param name="angle"> angle at which the segment rotates on z axis</param>
    /// <param name="scalingFactor"> length off the segment</param>
    /// <param name="rootNode"> boolean if its a root node or not</param>
    /// <param name="parent">transform reference to its parent (Not used at the moment)</param>
    public void Recursive3D_Tree(int depth, Vector3 position, float angle, float scalingFactor, bool rootNode, Transform parent)
    {
        GameObject node = Instantiate(nodePrefab, Vector3.zero, Quaternion.identity);             // create 3D object.
        Transform childNodeTransform = node.transform.GetChild(0);                                // save reference to its child.

        node.transform.position = position;
        childNodeTransform.rotation = Quaternion.Euler(childNodeTransform.rotation.x, childNodeTransform.rotation.y, parent.rotation.z + angle);
        childNodeTransform.localScale= new Vector3(depth/2f,depth * scalingFactor,depth/2f);

        node.transform.parent = transform;
        // save info on the script
        childNodeTransform.GetComponent<NodeInfo>().SetData(scalingFactor,depth);

        
        node.transform.Rotate(new Vector3(0, Random.Range(0, 120f), 0));      // instead of random can have a pattern
  

        // set position and translate pivot point
        childNodeTransform.Translate(node.transform.up.normalized * (depth*scalingFactor));

        // recurse
        float placementMultiplier = scalingFactor * 0.9f;
        if (depth - 1 > 0)
        {

            for (int i = -1; i < depth - 1; i++)
            {

                childNodeTransform.Translate(node.transform.up.normalized * (i*placementMultiplier+0));
                Recursive3D_Tree(depth - 1, childNodeTransform.position, 60, scalingFactor*0.6f,false, childNodeTransform);
                childNodeTransform.Translate(node.transform.up.normalized * (-i * placementMultiplier - 0));

                childNodeTransform.Translate(node.transform.up.normalized * (i * placementMultiplier + 0));
                Recursive3D_Tree(depth - 1, childNodeTransform.position, -60, scalingFactor * 0.6f, false, childNodeTransform);
                childNodeTransform.Translate(node.transform.up.normalized * (-i * placementMultiplier - 0));

            }
        }
        else 
        {
            // do nothing, no leaf node here    
        }
    }

    /// <summary>
    /// Function that startst the tree based on input.
    /// </summary>
    public void StartTree()
    {
        if (!treeInitialized)
        {
            int input = depthInputDropDown.value+1;

            Recursive3D_Tree(input, new Vector3(0, 0, 0), Random.Range(-20f,20f), 5, true, transform);
            treeInitialized = true;
            dynamicBranchUI.enabled = true;

            startButton.text = "Reset Scene";
        }
        else
        {
            treeInitialized = false;
            dynamicBranchUI.enabled = false;
            startButton.text = "Start Tree";
            depthInput.text = "";

            // delete all objects
            foreach (Transform t in transform)
                Destroy(t.gameObject);
        }
    }

    void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.transform != null)
                {
                    GenerateAnotherSegment(hit);
                }
            }
        }
    }


    void GenerateAnotherSegment(RaycastHit hit)
    {
        // Generate another node here
        float r = Random.Range(0, 100);
        int i = 1;
        if (r > 50) i = -1;

        float angle = hit.transform.rotation.eulerAngles.z + Random.Range(30f,60f)*i;
        Recursive3D_Tree(1, hit.point, angle, hit.transform.GetComponent<NodeInfo>().scalingFactor*0.6f, false, transform);
    }


}
