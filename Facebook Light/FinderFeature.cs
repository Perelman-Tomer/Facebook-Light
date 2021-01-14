using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
    public class FinderFeature
    {
        public IStrategyFinder IStrategyFinder { get; set; }

        public FinderFeature(IStrategyFinder i_StrategyFinder)
        {
            IStrategyFinder = i_StrategyFinder;
        }

        public void FindFinder(User i_LoggedInUser)
        {
            IStrategyFinder.FindBestHour(i_LoggedInUser);
        }
    }
}
