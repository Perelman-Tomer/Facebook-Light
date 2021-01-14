using FacebookWrapper.ObjectModel;

namespace C20_Ex20_Tomer_204389605_Binyamin_308051903
{
    public interface IFinder
    {
        void FindBestHour(User i_LoggedInUser);
        int Time { get; set; }
    }
}
