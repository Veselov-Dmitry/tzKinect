using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;

public class InputKinect : InputBase
{


    [SerializeField] private GameObject m_BodySourceManager;

    private BodySourceManager _BodyManager;
    private Body[] _Bodies;
    private float _SholPos;
    private float _HandLPosY;
    private float _HandRPosY;
    private float _HeadPos;
    private float _SpinePosY;
    private object _HandLeftIsUp;
    private object _HandRightIsUp;
    private object _HandUnderHead;
    private object _TwoHandIsUp;
    private object _IdleWaiting;
    private object _JumpIsUp;
    private PointSpeed _HandLSpeed;
    private PointSpeed _HandRSpeed;
    private PointSpeed _SpineSpeed;
    private bool _IsTracked;
    private float _MinSpeedHand = 5f;
    private float _MinSpeedSpine = 1.5f;
    private float _SpinePosMem;
    private float _DirOnJump;
    private float _HandLPosX;
    private float _HandRPosX;
    private float _SpinePosX;
    private float _HandLenght = 0.5f;
    


    #region Remuve IT!!!!
    public UnityEngine.UI.Text txt1;
    public UnityEngine.UI.Text txt2;
    #endregion

    void Start ()
    {
        _BodyManager = m_BodySourceManager.GetComponent<BodySourceManager>();
        float time = Time.time;
        _HandLSpeed = new PointSpeed(3, time);
        _HandRSpeed = new PointSpeed(3, time);
        _SpineSpeed = new PointSpeed(3, time);
        _SpinePosMem = 0;
        //StartCoroutine(HandsUp());
    }

    //private IEnumerator HandsUp()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(0.3f);
    //        if (_Bodies == null)
    //        {
    //        }


    //    }
    //}

    void Update ()
    {
        if (_BodyManager == null) return;
        _Bodies = _BodyManager.GetData();
        if (_Bodies == null) return;
        txt2.text = "KINECT off"; txt2.color = Color.red;
        _SholPos = 0;
        _HandLPosY = 0;
        _HandRPosY = 0;
        _HeadPos = 0;
        _SpinePosY = 0;
        foreach (var body in _Bodies)
        {
            if (body == null) continue;

            _IsTracked = body.IsTracked;
            if (_IsTracked)
            {
                txt2.text = "KINECT IsTracked"; txt2.color = Color.green;
                if (body.Joints.ContainsKey(JointType.Neck))
                    _SholPos = body.Joints[JointType.Neck].Position.Y;
                if (body.Joints.ContainsKey(JointType.HandLeft))
                {
                    _HandLPosY = body.Joints[JointType.HandLeft].Position.Y;
                    _HandLPosX = body.Joints[JointType.HandLeft].Position.X;
                }
                if (body.Joints.ContainsKey(JointType.HandRight))
                {
                    _HandRPosY = body.Joints[JointType.HandRight].Position.Y;
                    _HandRPosX= body.Joints[JointType.HandRight].Position.X;
                }
                if (body.Joints.ContainsKey(JointType.Head))
                    _HeadPos = body.Joints[JointType.Head].Position.Y;
                if (body.Joints.ContainsKey(JointType.SpineMid))
                {
                    _SpinePosY = body.Joints[JointType.SpineMid].Position.Y;
                    _SpinePosX = body.Joints[JointType.SpineMid].Position.X;
                }


            }
        }

        txt1.text = "Hand Left Y:" + _HandLPosY.ToString("0.##") +
            "\n\rHand Left X:" + _HandLPosX.ToString("0.##") +
            "\n\rHand Right X :" + _HandRPosX.ToString("0.##") +
            "\n\rSpine      :" + _SpinePosX.ToString("0.##") +
            "\n\rHand Right:" + _HandRPosY.ToString("0.##") +
            "\n\rSpine     :" + _SpinePosY.ToString("0.##");
        UpdSpeeds();
        Jumping();
        CheckHandUp();
    }

    private void Jumping()
    {
        _DirOnJump = _SpinePosY - _SpinePosMem;
        _SpinePosMem = _SpinePosY;

        if (_SpineSpeed.Speed > _MinSpeedSpine && _DirOnJump > 0)
        {
            if(_JumpIsUp == null)
            {
                _JumpIsUp = new object();
                Jump();
                StartCoroutine(JumpIsUp());
            }
        }
    }

    private void UpdSpeeds()
    {
        float time = Time.time;
        if (_HandLPosY != 0) _HandLSpeed.AddPosition(_HandLPosY, time);
        if (_HandRPosY != 0) _HandRSpeed.AddPosition(_HandRPosY, time);
        if (_SpinePosY != 0) _SpineSpeed.AddPosition(_SpinePosY, time);

        #region Remuve IT!!!!
        //_HandLSpeed.AddPosition(test1.transform.position.y, time);
        txt1.text += "\n\rHand Left  Vel:" + (_HandLSpeed.Speed).ToString("0#.#")
        + "\n\rHand Right Vel:" + (_HandRSpeed.Speed).ToString("0#.#")
        + "\n\rSpine     Vel:" + (_SpineSpeed.Speed).ToString("0#.#");
        #endregion
    }

    // если рука поднялась выше торса вызываем событие влево
    private void CheckHandUp()
    {
        if (_HandLPosY < _HeadPos && _HandRPosY < _HeadPos && (_HandLPosX <- _HandLenght || _HandRPosX > _HandLenght))
        {// руки ниже головы
            if (_HandLPosY > _SpinePosY && _HandRPosY < _SpinePosY)
            {// левая вверху правая внизу
                if (_HandLeftIsUp == null && _HandLSpeed.Speed < _MinSpeedHand && _HandLPosX < -_HandLenght)
                {
                    _HandLeftIsUp = new object();
                    LeftHandUp();
                    StartCoroutine(HandLeftUpping());
                }
            }
            else if (_HandRPosY > _SpinePosY && _HandLPosY < _SpinePosY && _HandRPosX > _HandLenght)
            {// правая вверху левая внизу
                if (_HandRightIsUp == null && _HandRSpeed.Speed < _MinSpeedHand)
                {
                    _HandRightIsUp = new object();
                    RightHandUp();
                    StartCoroutine(HandRightUpping());
                }
            }else if(_HandRPosY > _SpinePosY && _HandLPosY > _SpinePosY && (_HandLPosX < -_HandLenght || _HandRPosX > _HandLenght))
            {//обе руки выше спины
                if (_TwoHandIsUp == null )
                {
                    _TwoHandIsUp = new object();
                    TwoHandsUp();
                    StartCoroutine(TwoHandsUpping());
                }
            }
            else if ((_HandRPosY > _SpinePosY || _HandLPosY > _SpinePosY) &&
               _HandLSpeed.Speed < _MinSpeedHand && _HandRSpeed.Speed < _MinSpeedHand)
            {//любая из рук поднята над поясом
                //if (_IdleWaiting == null)
                //{
                //    _IdleWaiting = new object();
                //    StartCoroutine(IdleWaiting());
                //}
            }

        }else if(_HandLPosY > _HeadPos || _HandRPosY > _HeadPos)
        {// любая рука выше головы
            if (_HandUnderHead == null&& _HandRightIsUp == null&& _HandRightIsUp == null)
            {
                _HandUnderHead = new object();
                HandUp();
                StartCoroutine(HandUnderHead());
            }
        }
    }


    private IEnumerator JumpIsUp()
    {
        while (true)
        {
            if (_SpinePosY != 0)
            {
                if (_DirOnJump < 0)
                {
                    _JumpIsUp = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    // корутина обнуляет объект-блокировщик если обе руки опущены ниже пояса и запускает событие две руки опущены
    private IEnumerator IdleWaiting()
    {
        while (true)
        {
            if (_HandLPosY != 0 &&
                _HandRPosY != 0 &&
                _SpinePosY != 0)
            {
                if (_HandLPosY < _SpinePosY &&
                    _HandRPosY < _SpinePosY)
                {
                    TwoHandsDown();
                    _IdleWaiting = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    // корутина обнуляет объект-блокировщик если обе руки опущены ниже пояса и запускает событие две руки опущены
    private IEnumerator TwoHandsUpping()
    {
        while (true)
        {
            if (_HandLPosY != 0 &&
                _HandRPosY != 0 &&
                _SpinePosY != 0)
            {
                if (_HandLPosY < _SpinePosY &&
                    _HandRPosY < _SpinePosY)
                {
                    TwoHandsDown();
                    _TwoHandIsUp = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    // корутина обнуляет объект-блокировщик если обе руки ниже головы
    private IEnumerator HandUnderHead()
    {
        while (true)
        {
            if (_HandLPosY != 0 &&
                _HandRPosY != 0 &&
                _HeadPos != 0)
            {
                if (_HandLPosY < _HandRPosY &&
                    _HandRPosY < _HeadPos)
                {

                    _HandUnderHead = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    // корутина обнуляет объект-блокировщик если рука опутилась ниже торса или выше головы
    private IEnumerator HandLeftUpping()
    {
        while (true)
        {
            if (_HandLPosY != 0 &&
                _SpinePosY != 0 &&
                _HeadPos != 0)
            {
                if (_HandLPosY < _SpinePosY ||
                    _HandLPosY > _HeadPos)
                {
                    TwoHandsDown();
                    _HandLeftIsUp = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    // корутина обнуляет объект-блокировщик если рука опутилась ниже торса
    private IEnumerator HandRightUpping()
    {
        while (true)
        {
            if (_HandRPosY != 0 &&
                _SpinePosY != 0 &&
                _HeadPos != 0)
            {
                if (_HandRPosY < _SpinePosY ||
                    _HandLPosY > _HeadPos)
                {
                    TwoHandsDown();
                    _HandRightIsUp = null;
                    yield break;
                }
            }
            yield return null;
        }
    }

    
}
