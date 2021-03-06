﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MyJoyCon : MonoBehaviour
{
    float timer;
    int deviceNum = 0;

    private static readonly Joycon.Button[] m_buttons =
         Enum.GetValues(typeof(Joycon.Button)) as Joycon.Button[];

    private List<Joycon> m_joycons;
    public Joycon m_joyconL;
    public Joycon m_joyconR;
    private Joycon.Button? m_pressedButtonL;
    private Joycon.Button? m_pressedButtonR;

    public struct JoyConDec
    {
        public bool isLeft;
        public Joycon.Button? button;
        public float[] stick;
        public Vector3 gyro;
        public Vector3 accel;
        public Quaternion orientation;

        public float shuffleGage;

    }
    public static JoyConDec joyconDec = new JoyConDec();

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        m_joycons = JoyconManager.Instance.j;

        if (m_joycons == null || m_joycons.Count <= 0) return;

        m_joyconL = m_joycons.Find(c => c.isLeft);
        m_joyconR = m_joycons.Find(c => !c.isLeft);

        if (m_joycons[deviceNum].isLeft)
        {
            deviceNum = 0;
        }
        else
        {
            deviceNum = 1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        m_pressedButtonL = null;
        m_pressedButtonR = null;

        timer += 0.1f * Time.deltaTime;

        if (m_joyconL != null)
        {
            foreach (var button in m_buttons)
            {
                if (m_joyconL.GetButton(button))
                {
                    m_pressedButtonL = button;
                }
                if (m_joyconR.GetButton(button))
                {
                    m_pressedButtonR = button;
                }
            }

            SetJoyCon();
            JoyConAction();

        }

        //Debug.Log(joyconDec.button);
    }

    void SetJoyCon()
    {
        joyconDec.isLeft = m_joycons[deviceNum].isLeft;
        joyconDec.button = joyconDec.isLeft ? m_pressedButtonL : m_pressedButtonR;
        joyconDec.stick = m_joycons[deviceNum].GetStick();
        joyconDec.gyro = m_joycons[deviceNum].GetGyro();
        joyconDec.accel = m_joycons[deviceNum].GetAccel();
        joyconDec.orientation = m_joycons[deviceNum].GetVector();
    }

    void JoyConAction()
    {
        joyconDec.shuffleGage += Mathf.Abs(m_joycons[deviceNum].GetGyro().x + m_joycons[deviceNum].GetGyro().y + m_joycons[deviceNum].GetGyro().z);
        if (timer > 0.01f)
        {
            joyconDec.shuffleGage = 0;
            timer = 0;
        }
    }

    public bool GetAnyButtonDown()
    {
        if (m_joyconL == null) return false;

        if (m_joyconL.GetButtonDown(Joycon.Button.SHOULDER_1) || m_joyconL.GetButtonDown(Joycon.Button.SHOULDER_2)
            || m_joyconL.GetButtonDown(Joycon.Button.DPAD_UP) || m_joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN)
            || m_joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT) || m_joyconL.GetButtonDown(Joycon.Button.DPAD_RIGHT))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
