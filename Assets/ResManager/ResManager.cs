

/// <summary>
/// 资源管理属性方法
/// </summary>
public interface IResManager
{
	ResLoader GetResLoader();//获取资源加载器实例
}


/// <summary>
/// time:2019/6/30
/// author:Sun
/// des:资源的加载和管理
///
/// github:https://github.com/KingSun5
/// csdn:https://blog.csdn.net/Mr_Sun88
public class ResManager:IResManager
{
	/// <summary>
	/// 单例
	/// </summary>
	private static ResManager _instance;
	public static ResManager Instance
	{
		get
		{
			if (_instance==null)
			{
				_instance = new ResManager();
			}
			return _instance;
		}
	}

	/// <summary>
	/// 获取一个资源加载器
	/// </summary>
	/// <returns></returns>
	public ResLoader GetResLoader()
	{
		return new ResLoader();
	}
}
