using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using numl;
using numl.Model;
using numl.Supervised.DecisionTree;

namespace MLRunner
{
    public class Step1
    {
        public static void Execute()
        {
            Console.WriteLine("Raw");
            var data = Entry.GetEntries();

            DecisionTreeGenerator generator = new DecisionTreeGenerator()
            {
                Descriptor = Descriptor.Create<Entry>(),
            };

            var learner = Learner.Learn(data, 0.8, 10, generator);
            var model = learner.Model;
            Console.WriteLine(learner.Accuracy);
            Console.WriteLine(model);
        }

        public class Entry
        {
            [Label]
            public bool Survived { get; set; }
            [Feature]
            public string Class { get; set; }
            [Feature]
            public string Sex { get; set; }
            [Feature]
            public string Age { get; set; }

            public static IEnumerable<Entry> GetEntries()
            {
                using (var con = new System.Data.SqlClient.SqlConnection(Program.ConnectionString))
                {
                    return con.Query<Entry>("SELECT * FROM STEP_1_TypedAndCleaned ORDER BY newid()").ToArray();
                }
            }
        }
    }
}