using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetUI : MonoBehaviour {

	public void Load(int preset)
    {
        State.Instance.Current = ((Preset)preset).Get();
    }

}
