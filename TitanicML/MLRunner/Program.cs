using System.Text;
using System.Threading.Tasks;

namespace MLRunner
{
    class Program
    {
        public const string ConnectionString = @"Data Source=Onboarding;Integrated Security=true;Initial Catalog=ML_Titanic";
        static void Main(string[] args)
        {
            Step1.Execute();
            Step2.Execute();
            Step3.Execute();
        }
    }
}