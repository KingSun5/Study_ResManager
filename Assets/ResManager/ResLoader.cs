using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;


/// <summary>
/// time:2019/6/30
/// author:Sun
/// des:资源的加载容器
///
/// github:https://github.com/KingSun5
/// csdn:https://blog.csdn.net/Mr_Sun88
/// </summary>
public class ResLoader
{

	/// <summary>
	/// 公共容器 储存所有动态资源
	/// </summary>
	public static List<Res> PublicLoadingResList = new List<Res>();

	/// <summary>
	/// 自身容器 
	/// </summary>
	public List<Res> SelfLoadingResList = new List<Res>();
	
	/// <summary>
	/// Resources单个资源的同步加载
	/// </summary>
	/// <param name="assetPath"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T AssetLoad<T>(string assetPath) where T : Object
	{
		//先从当前容器中查找
		var loadAsset = SelfLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			return loadAsset.Asset as T;
		}
		//在从公共容器中查找
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			return loadAsset.Asset as T;
		}
		//资源列表中不存在则重写加载
		var assetIns = Resources.Load<T>(assetPath);
		if (assetIns==null)
		{
			Debug.LogError(assetPath+" is Null !");
			return null;
		}
		//加载后添加到资源列表
		loadAsset = new Res(assetIns,assetPath,ResType.ResSingle);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		//返回目标
		return assetIns;
	}

	/// <summary>
	/// Resources目录下全部资源加载
	/// </summary>
	/// <param name="folderPaht"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T[] AssetsLoad<T>(string folderPaht) where T : Object
	{
		//先从当前容器中查找
		var loadAsset = SelfLoadingResList.Find(asset => asset.AssetKey == folderPaht);
		if (loadAsset != null)
		{
			return loadAsset.Asset as T[];
		}
		//在从公共容器中查找
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == folderPaht);
		if (loadAsset != null)
		{
			return loadAsset.Asset as T[];
		}
		//资源列表中不存在则重写加载
		T[] assetIns = Resources.LoadAll<T>(folderPaht);
		if (assetIns==null)
		{
			Debug.LogError(folderPaht+" is Null !");
			return null;
		}
		
		//加载后添加到资源列表
		loadAsset = new Res(assetIns,folderPaht,ResType.ResGroup);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		return assetIns;
	}

	/// <summary>
	/// 异步加载Resources下的单个资源
	/// </summary>
	/// <param name="assetPath"></param>
	/// <param name="callback"></param> 异步加载的回调
	/// <returns></returns>
	public IEnumerator AssetLoadAsync<T>(string assetPath,Action<T> callback) where T : Object
	{
		//先从当前容器中查找
		var loadAsset = SelfLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as T);
			yield break;
		}
		
		//在从公共容器中查找
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as T);
			yield break;
		}
		
		ResourceRequest request = Resources.LoadAsync(assetPath);
		yield return request;
		//加载后添加到资源列表
		loadAsset = new Res(request.asset,assetPath,ResType.ResSingle);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		callback(request.asset as T);
	}
	
	/// <summary>
	/// 直接加载AB包
	/// </summary>
	/// <param name="assetPath"></param>
	/// <returns></returns>
	public AssetBundle ABLoadFromMemory(string assetPath)
	{
		//先从自身容器查找
		var loadAsset = SelfLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			return loadAsset.Asset as AssetBundle;
		}
		//再从公共容器查找		
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			return loadAsset.Asset as AssetBundle;
		}
		
		AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(assetPath));
		loadAsset = new Res(ab,assetPath,ResType.AssetBundle);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		return ab;
	}

	
	/// <summary>
	///  LoadFromMemoryAsync 异步加载AB包
	/// </summary>
	/// <param name="assetPath"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator ABLoadFromMemoryAsync(string assetPath,Action<AssetBundle> callback) 
	{
		//先从自身容器查找
		var loadAsset = SelfLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as AssetBundle);
			yield break;
		}
		//再从公共容器查找
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as AssetBundle);
			yield break;
		}
		
		//获取AB包资源
		AssetBundleCreateRequest request = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(assetPath));
		yield return request;
		loadAsset = new Res(request.assetBundle,assetPath,ResType.AssetBundle);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		callback(request.assetBundle);
	}
	
	/// <summary>
	/// LoadFromFile直接加载本地AB包
	/// </summary>
	/// <param name="assetPath"></param>
	/// <returns></returns>
	public AssetBundle ABLoadFromFile(string assetPath)
	{
		//先从自身容器查找
		var loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			return loadAsset.Asset as AssetBundle;
		}
		//再从公共容器查找
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			return loadAsset.Asset as AssetBundle;
		}
		
		AssetBundle ab = AssetBundle.LoadFromFile(assetPath);
		loadAsset = new Res(ab,assetPath,ResType.AssetBundle);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		return ab;
	}

	/// <summary>
	/// 从本地异步加载AB资源
	/// </summary>
	/// <param name="assetPath"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator ABLoadFromFileAsync(string assetPath,Action<AssetBundle> callback)
	{
		//先从自身容器查找
		var loadAsset = SelfLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as AssetBundle);
			yield break;
		}
		//再从公共容器查找
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as AssetBundle);
			yield break;
		}
		
		//获取AB包资源
		AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetPath);
		yield return request;
		loadAsset = new Res(request.assetBundle,assetPath,ResType.AssetBundle);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		callback(request.assetBundle);
	}


	/// <summary>
	/// WWW放置加载AB包
	/// </summary>
	/// <param name="assetPath"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator ABLoadWWW(string assetPath,Action<AssetBundle> callback)
	{
		//先从自身容器查找
		var loadAsset = SelfLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as AssetBundle);
			yield break;
		}
		//再从公共容器查找
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as AssetBundle);
			yield break;
		}
		
		//获取AB包资源
		WWW www = WWW.LoadFromCacheOrDownload(assetPath,1);
		yield return www.assetBundle;
		loadAsset = new Res(www.assetBundle,assetPath,ResType.AssetBundle);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		callback(www.assetBundle);
	}

	/// <summary>
	/// WebRequest请求AB包
	/// </summary>
	/// <param name="assetPath"></param>
	/// <param name="callback"></param>
	/// <returns></returns>
	public IEnumerator ABLoadWebRequest(string assetPath,Action<AssetBundle> callback)
	{
		//先从自身容器查询
		var loadAsset = SelfLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as AssetBundle);
			yield break;
		}
		
		//再从公共容器查询
		loadAsset = PublicLoadingResList.Find(asset => asset.AssetKey == assetPath);
		if (loadAsset != null)
		{
			yield return loadAsset;
			callback(loadAsset.Asset as AssetBundle);
			yield break;
		}
		
		//获取AB包资源
		UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(assetPath);
		yield return request.Send();
		AssetBundle ab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
		loadAsset = new Res(ab,assetPath,ResType.AssetBundle);
		PublicLoadingResList.Add(loadAsset);
		SelfLoadingResList.Add(loadAsset);
		//增添该资源的引用次数
		loadAsset.Reference();
		callback(ab);
	}
	
	/// <summary>
	/// 卸载当前容器资源
	/// </summary>
	public void UnloadAll()
	{
		foreach (var asset in SelfLoadingResList)
		{
			asset.Release();
		}

		SelfLoadingResList.Clear();
		SelfLoadingResList = null;
	}
	
	
}
