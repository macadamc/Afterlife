using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShadyPixel.Singleton;

public class FlockingAgentManager : Singleton<FlockingAgentManager>
{
    public List<FlockingAgent> agents;
    public List<Transform> enemies;

    public void AddAgent(FlockingAgent agent)
    {
        if (!agents.Contains(agent))
            agents.Add(agent);
    }

    public void RemoveAgent(FlockingAgent agent)
    {
        if (agents.Contains(agent))
            agents.Remove(agent);
    }

    public void AddEnemy(Transform enemyTransform)
    {
        if (!enemies.Contains(enemyTransform))
            enemies.Add(enemyTransform);
    }

    public void RemoveEnemy(Transform enemyTransform)
    {
        if (enemies.Contains(enemyTransform))
            enemies.Remove(enemyTransform);
    }

    private void OnEnable()
    {
        agents = new List<FlockingAgent>();
        enemies = new List<Transform>();
        Initialize(this);
    } 

    public List<FlockingAgent> GetNeighbors(FlockingAgent agent, float radius)
    {
        List<FlockingAgent> neighborsFound = new List<FlockingAgent>();

        foreach(var otherAgent in agents)
        {
            if (otherAgent == agent)
                continue;

            if(Vector2.Distance(agent.transform.position, otherAgent.transform.position) <= radius)
            {
                neighborsFound.Add(otherAgent);
            }
        }

        return neighborsFound;
    }

    public List<Transform> GetEnemies(FlockingAgent agent, float radius)
    {
        List<Transform> enemiesFound = new List<Transform>();

        foreach (var enemy in enemies)
        {
            if (enemy == agent)
                continue;

            if (Vector2.Distance(agent.transform.position, enemy.transform.position) <= radius)
            {
                enemiesFound.Add(enemy);
            }
        }

        return enemiesFound;
    }


}
