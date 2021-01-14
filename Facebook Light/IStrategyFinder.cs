using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FacebookWrapper.ObjectModel;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
    public interface IStrategyFinder
    {
        void FindBestHour(User i_LoggedInUser);
        int Time { get; set;}

        string BestPost { get; set;}
    }
}
