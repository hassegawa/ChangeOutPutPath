namespace ChangeOutPutPath
{
    class Program
    {

        static void Main(string[] args)
        {
            var core = new Core.Core();
            core.changeCsproft(args[0], args[1]);
        }

    }
}
