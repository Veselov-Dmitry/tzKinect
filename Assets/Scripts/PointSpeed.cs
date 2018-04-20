using System;
using System.Collections.Generic;
using System.Text;

public class PointSpeed
{
    private float _Speed;
    private float _LastTime;
    private Queue<float> _Speeds;
    private Queue<float> _DeltaPath;
    private Queue<float> _DeltaTime;

    public float Speed { get { return GetSpeed(); } }

    public int Accuracy { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Accuracy">количество запиминаемых значений скорости для средного</param>
    public PointSpeed(int Accuracy,float time)
    {
        _DeltaPath = new Queue<float>();
        _DeltaTime = new Queue<float>();
        _Speeds = new Queue<float>();
        _DeltaPath.Enqueue(0);
        _DeltaTime.Enqueue(0);
        _Speeds.Enqueue(0);

        this.Accuracy = Accuracy;
        _LastTime = time;
    }

    private float GetSpeed()
    {
        float AvrSpeed = 0;
        foreach (float item in _Speeds)
        {
            AvrSpeed += item;
        }
        int cnt = _Speeds.Count;
        return (cnt != 0) ? (AvrSpeed / cnt) : 0;
    }
    public string GetDubug()
    {
        StringBuilder sb = new StringBuilder();
        foreach (float item in _Speeds)
        {
            sb.Append(item + " \n\r");
        }
        sb.Append(_Speeds.Count + " \n\r");

        return sb.ToString();
    }

    private void UpdValue(float value, Queue<float> q)
    {
        q.Enqueue(value);
        if(q.Count >= Accuracy)
            q.Dequeue();
    }

    public void AddPosition(float y, float time)
    {
        float newPointTime = time;
        UpdValue(newPointTime - _LastTime, _DeltaTime);
        _LastTime = newPointTime;

        float deltay = Math.Abs(_DeltaPath.Peek() - y);
        UpdValue(y, _DeltaPath);

        UpdValue( (deltay / _DeltaTime.Peek() ), _Speeds);
    }
}
