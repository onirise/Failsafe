using System;
using System.Collections;
using UnityEngine;

namespace Failsafe.Items
{
    public interface ILimitedEffect
    {
        public IEnumerator EndEffect(Action callback);

    }
}