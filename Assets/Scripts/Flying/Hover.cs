using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    FakeZAxis m_FakeZAxis;
    Collider2D m_Collider;

    public GameObject Hurtbox;

    private void Awake()
    {
        m_FakeZAxis = GetComponent<FakeZAxis>();
        m_Collider = GetComponent<Collider2D>();
    }

    private void FixedUpdate()
    {
        if (Hurtbox?.activeSelf != !m_FakeZAxis.InAir)
            Hurtbox.SetActive(!m_FakeZAxis.InAir);
    }
}
