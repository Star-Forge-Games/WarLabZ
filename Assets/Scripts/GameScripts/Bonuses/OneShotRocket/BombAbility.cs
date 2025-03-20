using System.Collections;
using UnityEngine;

public class BombAbility : MonoBehaviour
{

    [SerializeField] GameObject bombPrefab;
    [SerializeField] Animator bomber;

    private void Start()
    {
        //if (!supplies exist) button.interactible = false;//
    }

    public void Fire()
    {
        // button.interactible = false;
        // supplies--;
        bomber.Play("Fire");
        StartCoroutine(Drop());
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(bombPrefab, new Vector3(0, 10, 10), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(bombPrefab, new Vector3(0, 10, 20), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(bombPrefab, new Vector3(0, 10, 30), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(bombPrefab, new Vector3(0, 10, 40), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Instantiate(bombPrefab, new Vector3(0, 10, 50), Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        // button.interactible = true;
    }
}
