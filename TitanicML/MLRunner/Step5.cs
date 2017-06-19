using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class Step5
    {
        public static void Execute()
        {
            Console.WriteLine("");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("Multi-Classification");
            Console.WriteLine("+++++++++++++++++++++++++++++++++++++++");
            Console.WriteLine("");

            var data = Entry.GetEntries();
            var descriptor = new Func<Descriptor>(Descriptor.Create<Entry>);

            var learner = Learner.Learn(data, 0.8, 10, new KNNGenerator(5)
            {
                Descriptor = descriptor(),
            });

            var model = learner.Model;
            Console.WriteLine(model.GetType().Name);
            Console.WriteLine(learner.Accuracy);

            double count = 0;
            foreach (var prediction in Entry.GetEntries())
            {
                var expected = prediction.Title;
                model.Predict(prediction);
                count += (string.Equals(expected.Trim(),prediction.Title.Trim(), StringComparison.OrdinalIgnoreCase) ? 1.0 : 0.0);
                Console.WriteLine($"{prediction.Class} & {prediction.FamilySize} family & {prediction.Age} age ==> {prediction.Title}");
            }

            double accuracy = count / (double)Entry.GetEntries().Count();
            Console.WriteLine($"Model Accuracy: { learner.Accuracy }");
            Console.WriteLine($"Overall Accuracy => { accuracy }");
        }

        public class Entry
        {
            [Feature]
            public string Class { get; set; }
            [Feature]
            public string Sex { get; set; }
            [Label]
            public string Title { get; set; }
            [Feature]
            public int Age { get; set; }
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