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
    public class Step4
    {
        public static void Execute()
        {
            Console.WriteLine("");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("Classification");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("");

            foreach (var generator in SupervisedGenerators())
            {
                var data = Entry.GetEntries();
                var learner = Learner.Learn(data, 0.8, 10, generator);
                var model = learner.Model;
                Console.WriteLine(model.GetType().Name);
                Console.WriteLine(learner.Accuracy);
            }
        }

        public static IEnumerable<Generator> SupervisedGenerators()
        {
            var descriptor = new Func<Descriptor>(Descriptor.Create<Entry>);
            yield return new NeuralNetworkGenerator()
            {
                Descriptor = descriptor(),
                MaxIterations = 10
            };
            yield return new KNNGenerator()
            {
                Descriptor = descriptor(),
                K = 5
            };
            yield return new KNNGenerator()
            {
                Descriptor = descriptor(),
                K = 3
            };
            yield return new PerceptronGenerator()
            {
                Descriptor = descriptor()
            };
            yield return new LinearRegressionGenerator()
            {
                Descriptor = descriptor(),
                MaxIterations = 100
            };
            yield return new LogisticRegressionGenerator()
            {
                Descriptor = descriptor(),
                MaxIterations = 100
            };
            yield return new NaiveBayesGenerator(100)
            {
                Descriptor = descriptor()
            };
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
            public decimal Age { get; set; }
            [Feature]
            public string Title { get; set; }
            [Feature]
            public decimal FarePerPerson { get; set; }
            [Feature]
            public decimal FamilySize { get; set; }
            [Feature]
            public string Deck { get; set; }
            [Feature]
            public string Embarked { get; set; }
            [Feature]
            public bool IsMother { get; set; }

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