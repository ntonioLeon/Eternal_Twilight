using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayFabCustomException : Exception
{
    public PlayFabCustomException() : base() { }
    public PlayFabCustomException(string message) : base(message) { }
    public PlayFabCustomException(string message, Exception inner) : base(message, inner) { }
}
