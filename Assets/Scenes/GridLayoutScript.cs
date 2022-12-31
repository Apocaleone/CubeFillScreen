using UnityEngine;

public class GridLayoutScript : MonoBehaviour
{
    [SerializeField]
    private GameObject Cube;
    [SerializeField]
    private Vector3 InitPosition;
    [SerializeField]
    private Vector2Int GridDimensions;
    [SerializeField]
    private Vector2 CubeDimensions;
   
    private float mDoubleTapWindow = 0.3f;
    private bool mIsSecondTap = false;
    private float mTimer = 0;
    private Material mCubeMaterial;
    private Camera mMainCamera;
    private float mXGap;
    private float mYGap;
    // Start is called before the first frame update
    private void Awake()
    {
        mMainCamera = Camera.main;
        Cube.transform.position = InitPosition;
        float widthViewPort = (mMainCamera.orthographicSize * mMainCamera.aspect);
        CubeDimensions = Cube.GetComponent<MeshRenderer>().bounds.size;
        InitPosition = new Vector3((- widthViewPort) + (CubeDimensions.x / 2), mMainCamera.orthographicSize + (CubeDimensions.y / 2), 0);
        GridDimensions.x = Mathf.FloorToInt(widthViewPort * 2/CubeDimensions.x);
        GridDimensions.y = Mathf.FloorToInt(mMainCamera.orthographicSize * 2/CubeDimensions.y);
        float widthDiff = widthViewPort * 2 - (GridDimensions.x * CubeDimensions.x);//((widthViewPort * 2)/CubeDimensions.x) - GridDimensions.x;
        float heightDiff = mMainCamera.orthographicSize * 2 - (GridDimensions.y * CubeDimensions.y);//(mMainCamera.orthographicSize * 2/CubeDimensions.y) - GridDimensions.y;
        mXGap = (widthDiff / (GridDimensions.x-1));
        mYGap = (heightDiff / (GridDimensions.y-1));
        LayItOut();
    }

  

    private void LayItOut()
    {
        float xSpacing = CubeDimensions.x+mXGap;
        float ySpacing = CubeDimensions.y+mYGap;
        for (int i = 0; i < GridDimensions.x; i++)
        {
            for (int j = 0; j < GridDimensions.y; j++)
            {
                GameObject cube = Instantiate(Cube);
                cube.SetActive(true);
                Material cubeMaterial = cube.GetComponent<MeshRenderer>().material;
                cubeMaterial.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                cube.transform.position = new Vector3(InitPosition.x + (i * xSpacing), InitPosition.y - (j * ySpacing), 0);
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (!mIsSecondTap && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                mCubeMaterial = hit.collider.GetComponent<MeshRenderer>().material;
                mIsSecondTap = true;
                mTimer = 0;
            }
        }
        else if (mIsSecondTap)
        {
            mTimer += Time.deltaTime;
            if (mTimer < mDoubleTapWindow && Input.GetMouseButtonDown(0))
            {
                mCubeMaterial.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                mIsSecondTap = false;
            }
            else if (mTimer > mDoubleTapWindow)
            {
                mIsSecondTap = false;

            }
        }
    }
}
