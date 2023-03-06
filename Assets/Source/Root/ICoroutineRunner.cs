using UnityEngine;
using System.Collections;

public interface ICoroutineRunner
{
    public Coroutine StartCoroutine(IEnumerator coroutine);
}
