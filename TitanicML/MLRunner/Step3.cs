using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using numl;
using numl.Model;
using numl.Supervised.DecisionTree;

namespace MLRunner
{
    public class Step3
    {
        public static void Execute()
        {
            var data = Entry.GetEntries();

            var generator = new numl.Supervised.NaiveBayes.NaiveBayesGenerator(10)
            {
                Descriptor = Descriptor.Create<Entry>(),
            };

            var learner = Learner.Learn(data, 0.8, 10, generator);
            var model = learner.Model;
            Console.WriteLine(learner.Accuracy);
            using (var ms = new MemoryStream())
            {
                model.Save(ms);
                //Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
            }
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
            public decimal FarePerPerson { get; set; }
            [Feature]
            public decimal FamilySize { get; set; }

            public static IEnumerable<Entry> GetEntries()
            {
                using (var con = new System.Data.SqlClient.SqlConnection(Program.ConnectionString))
                {
                    return con.Query<Entry>("SELECT * FROM STEP_2_MoreGeneralized").ToArray();
                }
            }
        }
    }
}