using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public interface IPoolObj {
    void Init();
    void Sleep();
}

// TODO Sleepが途中のオブジェクトの扱いをどうすると良いか謎だったので一旦保留
public abstract class Pooler<T> : MonoBehaviour where T : IPoolObj {
    [SerializeField] private GameObject mOriginal = default;
    private List<T> mObjPool = new List<T>();
    private List<T> mObjPoolUsing = new List<T>();
    private int removeOldThreshold = 200;

    public T CreateAndRemoveOld() {
        if (mObjPoolUsing.Count >= removeOldThreshold) {
            PoolOld();
        }
        return Create();
    }

    public T Create() {
        T obj;
        if (mObjPool.Count > 0) {
            obj = Pop();
        } else {
            var go = Instantiate<GameObject>(mOriginal);
            obj = go.GetComponent<T>();
        }
        obj.Init();
        return obj;
    }

    private T Pop() {
        var obj = mObjPool.First();
        mObjPool.Remove(obj);
        mObjPoolUsing.Add(obj);
        return obj;
    }

    public void PoolOld() {
        var old = mObjPoolUsing.First();
        Pool(old);
    }

    public void Pool(T obj) {
        obj.Sleep();
        mObjPoolUsing.Remove(obj);
        mObjPool.Add(obj);
    }

    public void Clear() {
        mObjPool.Clear();
    }
}
