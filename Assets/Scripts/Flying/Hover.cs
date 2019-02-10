using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    FakeZAxis m_FakeZAxis;

    public GameObject Hurtbox;

    private void Awake()
    {
        m_FakeZAxis = GetComponent<FakeZAxis>();
    }

    private void FixedUpdate()
    {
        if (Hurtbox == null)
            return;

        if (Hurtbox?.activeSelf != !m_FakeZAxis.InAir)
            Hurtbox.SetActive(!m_FakeZAxis.InAir);
    }
}
