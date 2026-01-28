using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class RemoteAddressablesBoot : MonoBehaviour
{
    public string keyToLoad = "MinhaPrefabAddress";

    IEnumerator Start()
    {
        // 1) Init
        yield return Addressables.InitializeAsync();

        // 2) Atualiza catálogos (se houver)
        var check = Addressables.CheckForCatalogUpdates();
        yield return check;
        if (check.Status == AsyncOperationStatus.Succeeded && check.Result != null && check.Result.Count > 0)
        {
            var update = Addressables.UpdateCatalogs(check.Result);
            yield return update;
        }

        // 3) Baixa dependências do que você vai usar
        var download = Addressables.DownloadDependenciesAsync(keyToLoad);
        yield return download;

        if (download.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Falha ao baixar dependências: " + download.OperationException);
            yield break;
        }

        // 4) Carrega o asset
        var load = Addressables.LoadAssetAsync<GameObject>(keyToLoad);
        yield return load;

        if (load.Status == AsyncOperationStatus.Succeeded)
        {
            Instantiate(load.Result);
        }
        else
        {
            Debug.LogError("Falha ao carregar asset: " + load.OperationException);
        }
    }
}