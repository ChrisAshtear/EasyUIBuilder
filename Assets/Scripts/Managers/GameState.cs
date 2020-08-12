/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectR.SaveGame;
using ProjectR.Actors.Heroes;

namespace ProjectR
{
    [Serializable]
    public class GameState
    {

        public Company company;
        public List<HeroStats> heroes;

        public void packHeroes()
        {
            heroes = new List<HeroStats>();
            foreach (Hero h in company.Heroes)
            {
                HeroStats stats = h.returnStats();
                heroes.Add(stats);
            }
        }
       

    }
}
*/