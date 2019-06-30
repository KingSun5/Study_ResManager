# Study_ResManager
Unity3D资源的加载和管理

使用方法：

		//获取资源加载器
		ResLoader SResLoader = ResManager.Instance.GetResLoader();

		#region ------------------加载Resources目录下资源------------------

		var cube = SResLoader.AssetLoad<GameObject>("Prefab/Cube");
		Instantiate(cube);

		#endregion
		
		#region -----------------加载Resources目录下资源组-----------------
		
		GameObject[] assets = SResLoader.AssetsLoad<GameObject>("Prefab");
		for (int i = 0; i < assets.Length; i++)
		{
			Instantiate(assets[i]);
		}	
		
		#endregion

		#region ----------------异步加载Resources目录下资源----------------
		
		StartCoroutine(SResLoader.AssetLoadAsync<GameObject>("Prefab/Cube", (asset) =>
		{
			Instantiate(asset);
		}));

		#endregion

		#region -----------LoadFromMemoryAsync方法异步加载AB资源-----------

		var path = Application.dataPath + "/AssetBundles/cube.unity3d.assetbundle";
		StartCoroutine(SResLoader.ABLoadFromMemoryAsync(path,assetbudnle =>
			{
				Object cube = assetbudnle.LoadAsset("Cube.prefab");
				Instantiate(cube);
			}));
		
		#endregion
		
		#region ---------------LoadFromMemory方法加载AB资源----------------
		
		var path = Application.dataPath + "/AssetBundles/cube.unity3d.assetbundle";
		AssetBundle ab = SResLoader.ABLoadFromMemory(path);
		Object cube = ab.LoadAsset("Cube.prefab");
		Instantiate(cube);

		#endregion

		#region -----------LoadFromMemoryAsync方法异步加载AB资源-----------

		var path = Application.dataPath + "/AssetBundles/cube.unity3d.assetbundle";
		StartCoroutine(SResLoader.ABLoadFromMemoryAsync(path,assetbudnle =>
			{
				Object cube = assetbudnle.LoadAsset("Cube.prefab");
				Instantiate(cube);
			}));
		
		#endregion

		#region ------------LoadFromFileAsync方法异步加载AB资源------------
		
		var path = Application.dataPath + "/AssetBundles/cube.unity3d.assetbundle";
		StartCoroutine(SResLoader.ABLoadFromFileAsync(path, ab =>
		{
			Object cube = ab.LoadAsset("Cube.prefab");
			Instantiate(cube);
		}));
		
		#endregion

		#region ---------------LoadFromFile方法异步加载AB资源--------------
		
		var path = Application.dataPath + "/AssetBundles/cube.unity3d.assetbundle";
		AssetBundle ab = SResLoader.ABLoadFromFile(path);
		Object cube = ab.LoadAsset("Cube.prefab");
		Instantiate(cube);
		
		#endregion
		
		#region --------------------WWW方法异步加载AB资源------------------
		
		var path = Application.dataPath + "/AssetBundles/cube.unity3d.assetbundle";
		StartCoroutine(SResLoader.ABLoadWWW(path, ab =>
		{
			Object cube = ab.LoadAsset("Cube.prefab");
			Instantiate(cube);
		}));
		
		#endregion
		
		#region -----------------WebRequest方法异步加载AB资源--------------
		
		var path = Application.dataPath + "/AssetBundles/cube.unity3d.assetbundle";
		StartCoroutine(SResLoader.ABLoadWebRequest(path, ab =>
		{
			Object cube = ab.LoadAsset("Cube.prefab");
			Instantiate(cube);
		}));
