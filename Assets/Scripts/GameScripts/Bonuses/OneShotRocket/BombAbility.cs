using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class BombAbility : MonoBehaviour
{

    [SerializeField] Bomb bombPrefab;
    [SerializeField] Animator bomber;
    [SerializeField] Button button;

    private void Start()
    {
        if (YG2.saves.supplies[1] == 0) button.interactable = false;
    }

    public void Fire()
    {
        var temp = YG2.saves.supplies;
        temp[1]--;
        YG2.saves.supplies = temp;
        YG2.SaveProgress();
        bomber.Play("Fire");
        StartCoroutine(Drop());
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(1f);
        Bomb b1 = Instantiate(bombPrefab, new Vector3(0, 10, 10), Quaternion.identity);
        b1.id = 0;
        yield return new WaitForSeconds(0.2f);
        Bomb b2 = Instantiate(bombPrefab, new Vector3(0, 10, 20), Quaternion.identity);
        b2.id = 1;
        yield return new WaitForSeconds(0.2f);
        Bomb b3 = Instantiate(bombPrefab, new Vector3(0, 10, 30), Quaternion.identity);
        b3.id = 2;
        yield return new WaitForSeconds(0.2f);
        Bomb b4 = Instantiate(bombPrefab, new Vector3(0, 10, 40), Quaternion.identity);
        b4.id = 3;
        yield return new WaitForSeconds(0.2f);
        Bomb b5 = Instantiate(bombPrefab, new Vector3(0, 10, 50), Quaternion.identity);
        b5.id = 4;
        yield return new WaitForSeconds(0.2f);
        if (YG2.saves.supplies[1] != 0) button.interactable = true;
    }
}
