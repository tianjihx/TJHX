using UnityEngine;
using System.Collections;

public class BattleController : Singleton<BattleController>
{
    [SerializeField] private BattleCamera m_BattleCamera;
    public BattleCamera Cam
    {
        get
        {
            return m_BattleCamera;
        }
    }

}
