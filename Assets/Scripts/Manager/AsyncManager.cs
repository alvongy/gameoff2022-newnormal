using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AsyncManager : Singleton<AsyncManager>
{
    public CancellationTokenSource disableCancellation = new CancellationTokenSource();
    public CancellationTokenSource destroyCancellation = new CancellationTokenSource();

    public AsyncManager()
    {
        if (disableCancellation != null)
        {
            disableCancellation.Dispose();
        }
        disableCancellation = new CancellationTokenSource();
    }

    public void OnDisable()
    {
        disableCancellation.Cancel();
    }

    public void OnDestroy()
    {
        destroyCancellation.Cancel();
        destroyCancellation.Dispose();
        destroyCancellation = new CancellationTokenSource();
    }
}
