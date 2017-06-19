using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using numl;
using numl.Model;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using numl.Supervised.KNN;
using numl.Supervised.NaiveBayes;
using numl.Supervised.NeuralNetwork;
using numl.Supervised.Perceptron;
using numl.Supervised.Regression;

namespace MLRunner
{
    public class Step6
    {
        public static void Execute()
        {
            Console.WriteLine("");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("Regression");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("");

            foreach (var generator in SupervisedGenerators())
            {
                var data = Entry.GetEntries();
                var learner = Learner.Learn(data, 0.8, 10, generator);
                var model = learner.Model;
                Console.WriteLine(model.GetType().Name);
                Console.WriteLine(learner.Accuracy);

                var predicted = model.Predict(new Entry() {Class = "first", Sex = "female", Title = "Mrs", FamilySize = 10});
                Console.WriteLine(predicted.Age);
            }
        }

        public static IEnumerable<Generator> SupervisedGenerators()
        {
            var descriptor = new Func<Descriptor>(Descriptor.Create<Entry>);
            yield return new LinearRegressionGenerator()
            {
                Descriptor = descriptor(),
            };
            //yield return new LogisticRegressionGenerator()
            //{
            //    Descriptor = descriptor(),
            //};
        }

        public class Entry
        {
            [Feature]
            public string Class { get; set; }
            [Feature]
            public string Sex { get; set; }
            [Label]
            public int Age { get; set; }
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
                    return con.Query<Entry>("SELECT * FROM STEP_2_MoreGeneralized ORDER BY NEWID()").ToArray();
                }
            }
        }
    }
}