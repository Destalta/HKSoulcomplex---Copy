using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using WeaverCore.Assets.Components;

public class ConversationDefault : Conversation
{
    private WeaverNPC npc;
    private bool inRange;
    public string[] Messages;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;
    public UnityEvent OnTalk;

    private void Start()
    {
        npc = GetComponent<WeaverNPC>();
    }

    private void Update()
    {
        if (npc.PlayerInRange)
        {
            if (inRange == false)
            {
                inRange = true;
                OnEnter.Invoke();
            }
        }
        else
        {
            if (inRange)
            {
                inRange = false;
                OnExit.Invoke();
            }
        }
    }

    protected override IEnumerator DoConversation()
    {
        ShowConversationBox();
        OnTalk.Invoke();
        yield return Speak(Messages);
        HideConversationBox();
        yield return null;
    }
}
