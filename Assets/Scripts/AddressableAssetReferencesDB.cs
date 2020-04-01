using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;


[CreateAssetMenu(fileName = "AddressableAssetReferenceStorageBase", menuName = "ScriptableObjects/AddressableAssetReferenceStorageBase", order = 1)]
public class AddressableAssetReferencesDB : ScriptableObject
{
    [SerializeField]
    private AssetReferenceStorage[] AssetReferences = default;

    public AssetReference FindElementWithId(int id)
    {
        Debug.Log(AssetReferences.Length);
        Debug.Log(AssetReferences[0].assetReference == null);
        Debug.Log(AssetReferences[1].assetReference == null);
        return AssetReferences.First(x => x.id == id).assetReference;
    }
}

[System.Serializable]
public class AssetReferenceStorage
{
    public int id = -1;
    public AssetReference assetReference;
}

