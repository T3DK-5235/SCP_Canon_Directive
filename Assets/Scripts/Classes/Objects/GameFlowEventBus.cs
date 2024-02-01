using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlowEventBus : MonoBehaviour
{
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
}
