using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGUFW;

public class MessagerTest : MonoBehaviour
{
    OrderedMessenger<int> messenger;

    // Start is called before the first frame update
    void Start()
    {
        messenger = new OrderedMessenger<int>();

        messenger.Broadcast(666);
        messenger.Remove<int>(666,test2);

        messenger.Add<int>(666,test1_1);
        messenger.Add<int>(666,test2);
        messenger.Broadcast(66);
        messenger.Remove(666,test1);
        messenger.Broadcast(666,4);
    }

    void test1()
    {
        Debug.Log("test1");
    }

    void test1_1(int i)
    {
        Debug.Log("test1_1 "+i);
        // messenger.Abort(666);
    }

    void test2(int i)
    {
        Debug.Log("test2 "+i);
    }

    void test3(string v)
    {
        Debug.Log("test3 "+v);
    }


}
