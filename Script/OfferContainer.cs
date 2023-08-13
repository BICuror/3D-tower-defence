using UnityEngine;

public class OfferContainer : MonoBehaviour
{
    [SerializeField] private GameObject _buildingPlaceHolder;

    private GameObject _instantiatedBuildingPlaceHolder;

    private DraggableObject _draggableObject;
    
    private Vector3 _finalPosition;

    public void Initialize(DraggableObject draggableObject, Vector3 spawnPosition)
    {
        _instantiatedBuildingPlaceHolder = Instantiate(_buildingPlaceHolder, spawnPosition, Quaternion.identity);

        _draggableObject = draggableObject;

        _draggableObject.PickUp();

        _finalPosition = spawnPosition;

        GetComponent<Rigidbody>().velocity = CalculateVelocity(spawnPosition);
    }

    private void OnCollisionEnter(Collision other) 
    {    
        if (other.gameObject == _instantiatedBuildingPlaceHolder)
        {
            Destroy(_instantiatedBuildingPlaceHolder);
        }
        else if (other.gameObject.CompareTag("Terrain"))
        {
            if (transform.childCount > 0)
            {
                GameObject mainObject = gameObject.transform.GetChild(0).gameObject;

                mainObject.transform.SetParent(null);

                mainObject.transform.rotation = Quaternion.identity;
    
                mainObject.transform.position = _finalPosition;
                
                _draggableObject.Place();  
            }

            Destroy(gameObject);

            Destroy(_instantiatedBuildingPlaceHolder);                     
        }
    }

    private Vector3 CalculateVelocity(Vector3 destination)
    {     
        Vector3 direction = destination - transform.position - new Vector3(0f, 1f, 0f); // get Target Direction
        float height = direction.y; // get height difference
        direction.y = 0; // retain only the horizontal difference
        float distance = direction.magnitude; // get horizontal direction
        float radians = 70f * Mathf.Deg2Rad; // Convert angle to radians
        direction.y = distance * Mathf.Tan(radians); // set dir to the elevation angle.
        distance += height / Mathf.Tan(radians); // Correction for small height differences

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * radians));

        return velocity * direction.normalized;
    }
}
