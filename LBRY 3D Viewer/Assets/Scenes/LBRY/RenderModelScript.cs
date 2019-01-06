using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RenderModelScript : MonoBehaviour {

    void Start() {
      StartCoroutine(GetAssetBundle());
    }

    IEnumerator GetAssetBundle() {
      UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("./lbry.assetbundle");
      yield return www.SendWebRequest();

      if(www.isNetworkError || www.isHttpError) {
        Debug.Log(www.error);
      } else {
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

        var assetNames = bundle.GetAllAssetNames();
        Debug.Log(assetNames[0]);
        Instantiate(bundle.LoadAsset(assetNames[0]));

        bundle.Unload(false);
      }
    }

    // Update is called once per frame
    void Update() {

    }

}
