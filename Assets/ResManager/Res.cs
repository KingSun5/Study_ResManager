using UnityEngine;

public enum ResType
{
	Null,//空
	ResSingle,//Res单个资源
	ResGroup,//Res组资源
	AssetBundle,//AB包资源
}



/// <summary>
/// time:2019/6/30
/// author:Sun
/// des:资源容器
///
/// github:https://github.com/KingSun5
/// csdn:https://blog.csdn.net/Mr_Sun88
/// </summary>
public class Res
{

	/// <summary>
	/// 资源类型标记
	/// </summary>
	private ResType _assetType;

	/// <summary>
	/// 资源名字
	/// </summary>
	public string AssetKey;

	/// <summary>
	/// 单体资源构造函数
	/// </summary>
	/// <param name="asset"></param>
	/// <param name="path"></param>
	/// <param name="type"></param>
	public Res(Object asset,string path,ResType type)
	{
		Asset = asset;
		AssetKey = path;
		_assetType = type;
	}

	/// <summary>
	/// 资源组构造函数
	/// </summary>
	/// <param name="assets"></param>
	/// <param name="path"></param>
	/// <param name="type"></param>
	public Res(Object[] assets,string path,ResType type)
	{
		Assets = assets;
		AssetKey = path;
		_assetType = type;
	}

	/// <summary>
	/// 单体资源
	/// </summary>
	public Object Asset;

	/// <summary>
	/// 资源组
	/// </summary>
	public Object[] Assets;

	/// <summary>
	/// 资源的引用次数
	/// </summary>
	private int _referenceCount = 0;

	/// <summary>
	/// 引用
	/// </summary>
	public void Reference()
	{
		_referenceCount++;
	}
	
	/// <summary>
	/// 释放
	/// </summary>
	public void Release()
	{
		//引用数--
		_referenceCount--;

		//引用数小于等于0 则将资源从内存中释放
		if (_referenceCount <= 0)
		{
			switch (_assetType)
			{
					case ResType.ResSingle:
						Resources.UnloadAsset(Asset);
						break;
					case ResType.ResGroup:
						foreach (var asset in Assets)
						{
							Resources.UnloadAsset(asset);
						}
						break;
					case ResType.AssetBundle:
						var ab = Asset as AssetBundle;
						if (ab != null) ab.Unload(false);
						break;
					default:
						Debug.LogError("This ResType is Error or Null");
						break;
			}

			if (ResLoader.PublicLoadingResList.Contains(this))
			{
				ResLoader.PublicLoadingResList.Remove(this);
			}
		}
	}

	/// <summary>
	/// 强制释放，不考虑引用
	/// </summary>
	public void Destroy()
	{
		switch (_assetType)
		{
			case ResType.ResSingle:
				Resources.UnloadAsset(Asset);
				break;
			case ResType.ResGroup:
				foreach (var asset in Assets)
				{
					Resources.UnloadAsset(asset);
				}
				break;
			case ResType.AssetBundle:
				var ab = Asset as AssetBundle;
				if (ab != null) ab.Unload(true);
				break;
			default:
				Debug.LogError("This ResType is Error or Null");
				break;
		}
	}
}