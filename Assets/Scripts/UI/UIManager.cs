using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;
    //用于存储显示的面板
    private Dictionary<string,BasePanel> panelDic = new Dictionary<string,BasePanel>();
    //场景中的canvas对象用于设置面板父对象
    private Transform canvasTrans;

    private UIManager()
    {
        //得到场景中的canvas对象
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvasTrans = canvas.transform;
        //在过场景的时候不销毁这个对象
        GameObject.DontDestroyOnLoad(canvas);
    }
        


    //显示面板
    public T ShowPane<T>() where T: BasePanel
    {
        string panelName = typeof(T).Name;
        //判断字典中是否有这个面板
        if(panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //根据面板名字动态加载面板
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        //把面板放到canvas下面
        panelObj.transform.SetParent(canvasTrans, false);

        //把面板保存起来
        T panel = panelObj.GetComponent<T>();
        //把面板保存在字典中
        panelDic.Add(panelName, panel);
        //调用自己的显示逻辑
        panel.ShowMe();

        return panel;
    }

    //隐藏面板
     public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //根据泛型得名字
        string panelName = typeof(T).Name;
        //判断当前的面板有没有你要隐藏的
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                //让面板淡出完毕再删除面板
                panelDic[panelName].HideMe(() =>
                {
                    //删除对象
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除字典里面的存储的面板脚本
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //删除对象
                GameObject.Destroy(panelDic[panelName].gameObject);
                //删除字典里面的存储的面板脚本
                panelDic.Remove(panelName);
            }
        }
    }
    //得到面板
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof (T).Name;
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        return null;
    }
}
