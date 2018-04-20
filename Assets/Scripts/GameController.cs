using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static event Action OnRun;
    public static event Action OnLevel2;
    public Material[] Mat;
    [HideInInspector] public static List<MoveChunk> listChunks = new List<MoveChunk>();
    [HideInInspector] public static GameController instance;
    [HideInInspector] public enum State { Run, Idle, Level2 }
    [HideInInspector] public static State St { get;private set; }
    [HideInInspector] public int _Count;
    private bool _SecondPress;
    private bool _LeftShpereOn;
    private bool _RightShpereOn;
    [SerializeField] private GameObject _Sph_L;
    [SerializeField] private GameObject _Sph_R;
    [SerializeField]private Material _Sph_Mat_Off;
    [SerializeField]private Material _Sph_Mat_On;

    private void Awake()
    {
        instance = this;
        listChunks.Clear();
    }
    private void Reset()
    {
        _Count = 0;
        St = State.Idle;
    }
    private void OnEnable()
    {
        HideZoneScript.OnReachEnd += BuildChunk;
        PrimitiveScript.OnCollisionBarrier += PrimitiveScript_OnCollisionBarrier;
        PrimitiveScript.OnCollisionBunus += PrimitiveScript_OnCollisionBunus;
        UI_time.OnTimeOut += UI_time_OnTimeOut;
        InputBase.OnTwoHandsUp += InputBase_OnTwoHandsUp;
        InputBase.OnTwoHandsDown += InputBase_OnTwoHandsDown;
        InputBase.OnLeftHandUp += InputBase_OnLeftHandUp;
        InputBase.OnRightHandUp += InputBase_OnRightHandUp;
    }


    private void OnDisable()
    {
        HideZoneScript.OnReachEnd -= BuildChunk;
        PrimitiveScript.OnCollisionBarrier -= PrimitiveScript_OnCollisionBarrier;
        PrimitiveScript.OnCollisionBunus -= PrimitiveScript_OnCollisionBunus;
        UI_time.OnTimeOut -= UI_time_OnTimeOut;
        InputBase.OnTwoHandsUp -= InputBase_OnTwoHandsUp;
        InputBase.OnTwoHandsDown -= InputBase_OnTwoHandsDown;
        InputBase.OnLeftHandUp -= InputBase_OnLeftHandUp;
        InputBase.OnRightHandUp += InputBase_OnRightHandUp;
    }
    void Start ()
    {
        St = State.Idle;
        _SecondPress = false;
    }

    private void UI_time_OnTimeOut()
    {
        Level2();
    }
    [ContextMenu("+++Level2")]
    private void Level2()
    {
        _Count = 0;
        St = State.Level2;
        if (OnLevel2 != null)
            OnLevel2();
        _LeftShpereOn = false;
        _RightShpereOn = false;
        if(!_Sph_L.activeSelf)
            _Sph_L.SetActive(true);
        _Sph_L.GetComponent<Renderer>().material = _Sph_Mat_Off;
        if (!_Sph_R.activeSelf)
            _Sph_R.SetActive(true);
        _Sph_R.GetComponent<Renderer>().material = _Sph_Mat_Off;        
    }
    private void InputBase_OnTwoHandsUp()
    {
        SwitchShperes(2);
    }
    private void InputBase_OnRightHandUp()
    {
        SwitchShperes(1);
    }
    private void InputBase_OnLeftHandUp()
    {
        SwitchShperes(-1);
    }
    private void InputBase_OnTwoHandsDown()
    {
        SwitchShperes(0);
    }
    private void SwitchShperes(int v)
    {
        if(St == State.Level2)
        {
            switch (v)
            {
                case 2:
                    {
                        _Sph_L.GetComponent<Renderer>().material = _Sph_Mat_On;
                        _Sph_R.GetComponent<Renderer>().material = _Sph_Mat_On;
                        _LeftShpereOn = true;
                        _RightShpereOn = true;
                        break;
                    }
                case 1:
                    {
                        _Sph_L.GetComponent<Renderer>().material = _Sph_Mat_Off;
                        _Sph_R.GetComponent<Renderer>().material = _Sph_Mat_On;
                        _RightShpereOn = true;
                        break;
                    }
                case 0:
                    {
                        if (_LeftShpereOn && _RightShpereOn)
                        {
                            _Sph_L.SetActive(false);
                            _Sph_R.SetActive(false);
                            St = State.Run;
                            if (OnRun != null)
                            {
                                OnRun();
                            }
                        }
                        break;
                    }
                case -1:
                    {
                        _Sph_L.GetComponent<Renderer>().material = _Sph_Mat_On;
                        _Sph_R.GetComponent<Renderer>().material = _Sph_Mat_Off;
                        _LeftShpereOn = true;
                        break;
                    }
            }
        }
    }

    private void PrimitiveScript_OnCollisionBarrier(PrimitiveScript obj)
    {
        if (_Count > 0)
        {
            _Count--;
        }
    }

    private void PrimitiveScript_OnCollisionBunus(PrimitiveScript obj)
    {
        _Count++;
        if (_Count >= 10)
        {
            Level2();
        } 
    }

    public void BuildChunk()
    {
        MoveChunk go = GetInactiveChunk();
        go.gameObject.SetActive(true);
    }

    public void Run()
    {
        St = State.Run;
        if(OnRun != null)
        {
            OnRun();
        }
        MakePreposition();
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    private MoveChunk GetInactiveChunk()
    {
        int i = 0;
        for (;;)
        {
            i = UnityEngine.Random.Range(0, listChunks.Count);
            if(!listChunks[i]) continue;
            if (listChunks[i].gameObject.activeSelf)
                continue;
            return listChunks[i];
        }
    }

    
    private void MakePreposition()
    {
        if (_SecondPress) return;
        _SecondPress = true;

        MoveChunk g1,g2,g3,g4;
        g4 = GetInactiveChunk();
        g4.gameObject.SetActive(true);
        g4.transform.position = new Vector3(0, 0, 0);

        g3 = GetInactiveChunk();
        g3.gameObject.SetActive(true); 
        g3.transform.position = new Vector3(0, 0, 10f);

        g2 = GetInactiveChunk();
        g2.gameObject.SetActive(true); 
        g2.transform.position = new Vector3(0, 0, 20f);

        g1 = GetInactiveChunk();
        g1.gameObject.SetActive(true);
    }
}
