
/*using Combats;
using ProjectR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using ProjectR.SaveGame;
using ProjectR.Actors.Heroes;
public class GameInit : MonoBehaviour
{
    [SerializeField] private Company company;
    [SerializeField] private Combat combat;
    [SerializeField] private MapUI map;

    protected void Start()
    {
        company = new Company();
        MapManagers.GenerateMap(7, 3);
        map = MapUI.Instantiate();

        Thief thief = Thief.Instantiate();
        thief.gameObject.SetActive(false);
        company.Hire(thief);

    }

    public GameState getState()
    {
        GameState state = new GameState();
        state.company = company;
        return state;
    }

    public void setState(GameState state)
    {
        this.company.Gold = state.company.Gold;
        this.company.FireAll();

        
        foreach(HeroStats h in state.heroes)
        {
            
            this.company.Hire(initHero(h));
        }
    }

    public Hero initHero(HeroStats stats)
    {
        Thief thf = Thief.Instantiate();
        thf.gameObject.SetActive(false);
        //init stats.
        thf.loadStats(stats);

        return thf;
    }
}*/
