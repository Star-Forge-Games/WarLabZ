using UnityEngine;
using YG;

public class PlatformSprite : MonoBehaviour
{

    [SerializeField] ImageLoadYG currencyImageLoad;

    private void Start()
    {
        if (currencyImageLoad)
            currencyImageLoad.Load(YG2.purchases[0].currencyImageURL);
    }
}
