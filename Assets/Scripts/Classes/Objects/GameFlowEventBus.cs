using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowEventBus : MonoBehaviour
{
    //ANIMATION_AMBIENT
    //ANIMATION_NEW-MONTH
    //ANIMATION_NEW-PROPOSAL
    //PROPOSAL_INITIALIZATION
    //PROPOSAL_ONGOING
    //PROPOSAL_FULL-DECISION
    //PROPOSAL_STATS-UPDATED
    private LinkedList<GameStateEnum> gameStateQueue;

    public GameFlowEventBus(LinkedList<GameStateEnum> savedGameStateQueue) {
        this.gameStateQueue = savedGameStateQueue;
    }

    public void Enqueue(GameStateEnum gameEvent) {
        gameStateQueue.AddLast(gameEvent);
    }

    public void Enqueue(GameStateEnum[] gameEvents) {
        for(int i = 0; i < gameEvents.Length; i++) {
            gameStateQueue.AddLast(gameEvents[i]);
        }
    }

    public void Dequeue() {
        gameStateQueue.RemoveFirst();
    }

    public void Interrupt(GameStateEnum gameEvent) {
        gameStateQueue.AddFirst(gameEvent);
    }

    public void Interrupt(GameStateEnum[] gameEvents) {
        for(int i = 0; i < gameEvents.Length; i++) {
            gameStateQueue.AddFirst(gameEvents[i]);
        }
    }

    public int GetBusSize() {
        return gameStateQueue.Count;
    }

    public GameStateEnum Head(){
        return gameStateQueue.First.Value;
    }

    //Used for debugging only
    public LinkedList<GameStateEnum> GetFlowEventBus() {
        return gameStateQueue;
    }
}
