using UnityEngine;

public class CrosshairModifier : MonoBehaviour
{
    [SerializeField] private GameObject Camera;
    [SerializeField] private GameObject Crosshair;

    private bool CheckEnemy()
    {
        //raycast from the camera to the center of the screen
        Ray ray = Camera.GetComponent<Camera>().ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }

    private void ChangeCrosshairColor(bool isEnemy)
    {
        Color color = isEnemy ? Color.red : Color.white;
        Crosshair.GetComponent<UnityEngine.UI.Image>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        bool isEnemy = CheckEnemy();
        ChangeCrosshairColor(isEnemy);
    }
}
