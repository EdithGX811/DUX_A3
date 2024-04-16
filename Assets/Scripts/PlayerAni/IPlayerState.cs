﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState 
{
    void OnEnter();
    void OnExit();
    void OnUpdate();
}