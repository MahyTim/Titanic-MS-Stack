using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using numl;
using numl.Math.LinearAlgebra;
using numl.Model;
using numl.Supervised.DecisionTree;

namespace MLRunner
{
    public class Step2
    {
        public static void Execute()
        {
            Console.WriteLine("Simple binary tree");

            var data = Entry.GetEntries();

            DecisionTreeGenerator generator = new DecisionTreeGenerator()
            {
                Descriptor = Descriptor.Create<Entry>(),
            };

            var learner = Learner.Learn(data, 0.8, 10, generator);
            var model = learner.Model;

            var classes = new []{"FIRST", "SECOND", "THIRD"};
            foreach (var @class in classes)
            {
                var predicted = model.Predict(new Entry()
                {
                    Class = @class,
                    Sex = "FEMALE",
                    Embarked = "CHERBOURG",
                    Age = "30",
                    IsMother = false,
                    Title = "Mrs",
                });
                Console.WriteLine($"{predicted.Class} ==> {predicted.Survived}");
            }

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
            [Feature]
            public string Title { get; set; }
            [Feature]
            public bool IsMother { get; set; }
            [Feature]
            public string Embarked { get; set; }

            //[Feature]
            //public string Deck { get; set; }

            //[Feature]
            //public decimal FarePerPerson { get; set; }

            public static IEnumerable<Entry> GetEntries()
            {
                using (var con = new System.Data.SqlClient.SqlConnection(Program.ConnectionString))
                {
                    return con.Query<Entry>("SELECT * FROM STEP_2_MoreGeneralized ORDER BY newid()").ToArray();
                }
            }
        }
    }
}