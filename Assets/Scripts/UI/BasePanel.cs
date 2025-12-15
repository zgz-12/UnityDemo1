using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public  abstract class BasePanel : MonoBehaviour
{
    //专门用于控制面板透明度的组件
    private CanvasGroup canvasGroup;
    //淡入淡出的速度
    private float alphaSpeed = 10;
    //当前是需要隐藏还是显示
    private bool isShow = false; 
    //当隐藏完毕之后要做的事情
    private UnityAction hideCallBake = null;
   protected virtual void Awake()
    {
        //一开始去获取面板上挂载的组件
        canvasGroup = this.GetComponent<CanvasGroup>();
        //如果没有挂载就添加这个组件
        if(canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();

    }
    protected virtual void Start()
    {
        Init();
    }
    /// <summary>
    /// 注册控件事件的方法
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// 显示的时候做的逻辑
    /// </summary>
    public virtual void ShowMe()
    {
        canvasGroup.alpha = 0;
        isShow = true;
    }

    /// <summary>
    /// 隐藏的时候做的事情
    /// </summary>
    public virtual void HideMe( UnityAction callBake)
    {
        canvasGroup.alpha = 1;
        isShow = false;

        hideCallBake = callBake;
    }
    void Update()
    {
        //当处于显示状态时 透明度不为1，就会不停加到1 到1之后就停止变化
        //淡入
        if(isShow&&canvasGroup.alpha!=1) 
        {
            canvasGroup.alpha += alphaSpeed * Time.deltaTime;
            if(canvasGroup.alpha>=1)
                canvasGroup.alpha = 1;
        }
        //淡出
        else if (!isShow&&canvasGroup.alpha!=0)
        {
            canvasGroup.alpha -= alphaSpeed*Time.deltaTime;
            if (canvasGroup.alpha<=0)
                canvasGroup.alpha = 0;

            hideCallBake?.Invoke();
        }
    }
}
