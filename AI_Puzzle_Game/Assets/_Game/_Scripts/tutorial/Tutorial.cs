using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : BaseBehaviour
{
    protected override void OnVictoryStart()
    {
        ES3.Save<bool>("first_play", false);
    }
}
