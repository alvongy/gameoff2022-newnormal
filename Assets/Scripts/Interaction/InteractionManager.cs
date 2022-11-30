using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using YK.Game.Events;
//public enum InteractionType { None = 0, PickUp, Cook, Talk };

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader = default;

    //Events for the different interaction types
    [Header("Broadcasting on")]
    [SerializeField] private ItemEventChannelSO _onObjectPickUp = default;
    //[SerializeField] private VoidEventChannelSO _onCookingStart = default;
    //[SerializeField] private DialogueActorChannelSO _startTalking = default;
    [SerializeField] private InteractionUIEventChannelSO _toggleInteractionUI = default;

    [Header("Listening to")]
    [SerializeField] private VoidEventChannelSO _onInteractionEnded = default;
    //[SerializeField] private PlayableDirectorChannelSO _onCutsceneStart = default;

    [ReadOnlyInspector] public InteractionType currentInteractionType; //This is checked/consumed by conditions in the StateMachine

    private LinkedList<Interaction> _potentialInteractions = new LinkedList<Interaction>(); //To store the objects we the player could potentially interact with

    private void OnEnable()
    {
        _inputReader.InteractEvent += OnInteractionButtonPress;
        _onInteractionEnded.OnEventRaised += OnInteractionEnd;
        //_onCutsceneStart.OnEventRaised += ResetPotentialInteractions;
    }

    private void OnDisable()
    {
        _inputReader.InteractEvent -= OnInteractionButtonPress;
        _onInteractionEnded.OnEventRaised -= OnInteractionEnd;
        //	_onCutsceneStart.OnEventRaised -= ResetPotentialInteractions;
    }

    // Called mid-way through the AnimationClip of collecting
    private void Collect()
    {
        GameObject itemObject = _potentialInteractions.First.Value.interactableObject;
        _potentialInteractions.RemoveFirst();

        if (_onObjectPickUp != null)
        {
            ItemSO currentItem = itemObject.GetComponent<CollectableItem>().GetItem();
            _onObjectPickUp.RaiseEvent(currentItem);
        }

           //ObjectPool.Destroy(itemObject); //TODO: maybe move this destruction in a more general manger, to implement a removal SFX
           //Destroy(itemObject); 



            RequestUpdateUI(false);
    }

    private void OnInteractionButtonPress()
    {
        if (_potentialInteractions.Count == 0)
            return;

        currentInteractionType = _potentialInteractions.First.Value.type;

        switch (_potentialInteractions.First.Value.type)
        {
            case InteractionType.OPERATE:
                _potentialInteractions.First.Value.interactableObject.GetComponent<InteractItem>().Interaction();
                break;

            case InteractionType.TALK:
                _potentialInteractions.First.Value.interactableObject.GetComponent<InteractItem>().Interaction();
                break;

                //No need to do anything for Pickup type, the StateMachine will transition to the state
                //and then the AnimationClip will call Collect()
        }
    }

    //Called by the Event on the trigger collider on the child GO called "InteractionDetector"
    public void OnTriggerChangeDetected(bool entered, GameObject obj)
    {
        if (entered)
            AddPotentialInteraction(obj);
        else
            RemovePotentialInteraction(obj);
    }

    private void AddPotentialInteraction(GameObject obj)
    {
        Interaction newPotentialInteraction = new Interaction(InteractionType.NONE, obj);
        
        if (obj.CompareTag("Pickable"))
        {
            newPotentialInteraction.type = InteractionType.USE;
           	_potentialInteractions.AddFirst(newPotentialInteraction);
            Collect();
        }

        else if (obj.CompareTag("Terminal"))
        {
            newPotentialInteraction.type = InteractionType.OPERATE;
            _potentialInteractions.AddFirst(newPotentialInteraction);
            RequestUpdateUI(true);
        }
        else if (obj.CompareTag("Operable"))
        {
            newPotentialInteraction.type = InteractionType.TALK;
            _potentialInteractions.AddFirst(newPotentialInteraction);
            RequestUpdateUI(true);

            //if (!obj.GetComponent<MFTreasureBoxItem>().IsOpen()) 
            //{
            //    newPotentialInteraction.type = InteractionType.TALK;
            //    _potentialInteractions.AddFirst(newPotentialInteraction);
            //    RequestUpdateUI(true);
            //}

        }
        //else if (newPotentialInteraction.type != InteractionType.NONE)
        //{
        //	_potentialInteractions.AddFirst(newPotentialInteraction);
        //	RequestUpdateUI(true);
        //}
        //else
        //{
        //    RequestUpdateUI(false);
        //}
    }

    private void RemovePotentialInteraction(GameObject obj)
    {

        LinkedListNode<Interaction> currentNode = _potentialInteractions.First;
        while (currentNode != null)
        {
            if (currentNode.Value.interactableObject == obj)
            {
                _potentialInteractions.Remove(currentNode);
                break;
            }
            currentNode = currentNode.Next;
        }

        RequestUpdateUI(_potentialInteractions.Count > 0);
    }

    private void RequestUpdateUI(bool visible)
    {
        if (visible)
            _toggleInteractionUI.RaiseEvent(true, _potentialInteractions.First.Value.type);
        else

            _toggleInteractionUI.RaiseEvent(false, InteractionType.NONE);
    }

    private void OnInteractionEnd()
    {
        switch (currentInteractionType)
        {
            //case InteractionType.Cook:
            //case InteractionType.Talk:
            //	//We show the UI after cooking or talking, in case player wants to interact again
            //	RequestUpdateUI(true);
            //	break;
        }

        _inputReader.EnableGameplayInput();
    }

    private void ResetPotentialInteractions(PlayableDirector _playableDirector)
    {
        _potentialInteractions.Clear();
        RequestUpdateUI(_potentialInteractions.Count > 0);
    }
}
