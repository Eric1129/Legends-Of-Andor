using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Monster : Fightable, MoveStrategy
{
    private Node location;
    private int willpower;
    private int strength;
    private string monsterType;
    protected GameObject prefab;
    private int reward;
    private bool canMove;
    private bool herbGor;
    private bool skralTower;

    public Monster(Node startingPos, GameObject prefab)
    {
        location = startingPos;
        this.prefab = prefab;
        canMove = true;
        herbGor = false;
        skralTower = false;
    }
    private Monster() { }

    public void attack(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void setCantMove()
    {
        canMove = false;
    }

    public bool canMonsterMove()
    {
        return canMove;
    }

    public bool isMedicinalGor()
    {
        return herbGor;
    }

    public void setSkralTower()
    {
        skralTower = true;
    }

    public void setHerbGor()
    {
        herbGor = true;
    }
    public void move()
    {
        location = location.toCastleNode();

        if (location.getIndex() == 0)
        {
            GameController.instance.monsterAtCastle(this);
        }
    }


    public int getLocation()
    {
        return location.getIndex();
    }

    public void setLocationNode(Node x)
    {
        location = x;
    }

    public Node getLocationNode()
    {
        return location;
    }

    public GameObject getPrefab()
    {
        return prefab;
    }
    public int getWillpower()
    {
        return willpower;
    }
    public void setWillpower(int willpower)
    {
        this.willpower = willpower;
    }

    public void increaseWillpower(int amount)
    {
        this.willpower += amount;
    }
    public void decreaseWillpower(int amount)
    {
        this.willpower = Mathf.Max(0, this.willpower - amount);


    }

    public void recover()
    {
        if(monsterType == "Gor")
        {
            this.willpower = 4;
        }
        else if (monsterType == "Skral")
        {
            this.willpower = 5;
        }
        else
        {
            this.willpower = 7;
        }
    }

    public int getStrength()
    {
        return strength;
    }
    public void setStrength(int strength)
    {
        this.strength = strength;
    }

    public void setMonsterType(string monsterType)
    {
        this.monsterType = monsterType;
        
    }

    public string getMonsterType()
    {
        return this.monsterType;
    }

    public void setReward(int reward)
    {
        this.reward = reward;
    }

    public int getReward()
    {
        return this.reward;
    }

    public int getNumDice()
    {
        if(this.monsterType == "Gor" || this.monsterType == "Skral")
        {
            if (this.willpower < 7) return 2;
            else return 3;
        }
        else
        {
            if (this.willpower < 7) return 1;
            else return 2;
        }
    }

    public List<int> diceRoll()
    {
        System.Random random = new System.Random();
        List<int> dice = new List<int>();
        if (this.monsterType == "Gor" || this.monsterType == "Skral")
        {
            for(int i =0; i<getNumDice(); i++)
            {
                dice.Add(random.Next(1, 7));
            }
        }
        else
        {
            int[] blackDice = {6, 8, 10, 12, 6, 10};
            for (int i = 0; i < getNumDice(); i++)
            {
                dice.Add(blackDice[random.Next(1, 7)]);
            }
        }

        return dice;
    }    
}
