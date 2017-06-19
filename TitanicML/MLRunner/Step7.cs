using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using numl;
using numl.Math.Linkers;
using numl.Math.Metrics;
using numl.Model;
using numl.Supervised;
using numl.Supervised.DecisionTree;
using numl.Supervised.KNN;
using numl.Supervised.NaiveBayes;
using numl.Supervised.NeuralNetwork;
using numl.Supervised.Perceptron;
using numl.Supervised.Regression;
using numl.Unsupervised;

namespace MLRunner
{
    public class Step7
    {
        public static void Execute()
        {
            Console.WriteLine("");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("Unsupervised");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("");

            var cluster = new HClusterModel();
            var descriptor = Descriptor.Create<Entry>();
            var linker = new CentroidLinker(new EuclidianDistance());
            var root = cluster.Generate(descriptor, Entry.GetEntries(), linker);
            Console.WriteLine(root);
        }

       

        public class Entry
        {
            [Feature]
            public string Class { get; set; }
            [Feature]
            public string Sex { get; set; }
            [Feature]
            public string Age { get; set; }
            [Feature]
            public string Title { get; set; }
            [Label]
            public decimal FamilySize { get; set; }
            [Feature]
            public string Deck { get; set; }
            [Feature]
            public string Embarked { get; set; }

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