using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using numl;
using numl.Math.LinearAlgebra;
using numl.Model;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using numl.Supervised.KNN;

namespace MLRunner
{
    public class Step3
    {
        public static void Execute()
        {
            Console.WriteLine("Taking the fares into account");

            Console.WriteLine("Probability generator");
            var generator = (Generator)new numl.Supervised.NaiveBayes.NaiveBayesGenerator(10)
            {
                Descriptor = Descriptor.Create<Entry>(),
            };

            var learner = Learner.Learn(Entry.GetEntries(), 0.80, 10, generator);
            var model = learner.Model;
            Console.WriteLine(learner.Accuracy);
            using (var ms = new MemoryStream())
            {
                model.Save(ms);
                //Console.WriteLine(Encoding.UTF8.GetString(ms.ToArray()));
            }
            TestActuals(model);


            Console.WriteLine("Near generator");
            generator = new KNNGenerator()
            {
                Descriptor = Descriptor.Create<Entry>(),
            };

            learner = Learner.Learn(Entry.GetEntries(), 0.80, 10, generator); //Verhogen naar 100
            model = learner.Model;
            Console.WriteLine(learner.Accuracy);
            TestActuals(model);
        }

        private static void TestActuals(IModel model)
        {
            double count = 0;
            foreach (var prediction in Entry.GetEntries())
            {
                var expected = prediction.Survived;
                model.Predict(prediction);
                count += (expected == prediction.Survived ? 1.0 : 0.0);
            }
            double accuracy = count / (double)Entry.GetEntries().Count();
            Console.WriteLine($"Overall Accuracy => { accuracy }");
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
            public int Age { get; set; }
            [Feature]
            public string Title { get; set; }

            [Feature]
            public decimal FarePerPerson { get; set; }
            [Feature]
            public decimal FamilySize { get; set; }
            [Feature]
            public bool IsMother { get; set; }

            public static IEnumerable<Entry> GetEntries()
            {
                using (var con = new System.Data.SqlClient.SqlConnection(Program.ConnectionString))
                {
                    return con.Query<Entry>("SELECT * FROM STEP_2_MoreGeneralized ORDER BY NEWID()").ToArray();
                }
            }
        }
    }
}