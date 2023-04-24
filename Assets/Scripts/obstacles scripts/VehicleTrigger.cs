using UnityEngine;

public class VehicleTrigger : MonoBehaviour
{
    [SerializeField] private float rot;
    [SerializeField] private bool IsDestroy;
    [SerializeField] private float rotStart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            if (IsDestroy)
            {
                Vehicle v = other.GetComponent<Vehicle>();
                GameObject[] cars = v.GetCars();
                int i = Random.Range(0, cars.Length - 1);
                GameObject obj = Instantiate(cars[i]);
                obj.transform.position = v.GetStart();
                obj.transform.eulerAngles = new Vector3(0f, rotStart, 0f);
                float[] fs = v.GetSpeed();
                obj.GetComponent<Vehicle>().SetSpeed(fs[0], fs[1]);
                Destroy(other.gameObject);
            }
            else
            {
                other.GetComponent<Vehicle>().ChangeEuler(rot);
            }
        }
    }
}
