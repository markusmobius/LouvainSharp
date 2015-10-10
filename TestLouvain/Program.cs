using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using LouvainCommunityPL;

namespace TestLouvain
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph g = new Graph();
            //test network
            int edgecounter = 0;
            using (StreamReader sr = new StreamReader("testnetwork.csv"))
            {
                string line = null;
                while ((line = sr.ReadLine()) != null)
                {
                    line=line.Trim();
                    if (line==""){continue;}
                    string[] d = line.Split(',');
                    int agent = Int32.Parse(d[0]);
                    for (int i = 1; i < d.Length; i++)
                    {
                        int friends = Int32.Parse(d[i]);
                        g.AddEdge(agent, friends, 1);
                        edgecounter++;
                    }
                }
            }
            Console.WriteLine("{0} edges added",edgecounter);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Restart();
            Dictionary<int, int> partition = Community.BestPartition(g);
            Console.WriteLine("BestPartition: {0}", stopwatch.Elapsed);
            var communities = new Dictionary<int, List<int>>();
            foreach (var kvp in partition)
            {
                List<int> nodeset;
                if (!communities.TryGetValue(kvp.Value, out nodeset)) {
                    nodeset = communities[kvp.Value] = new List<int>();
                }
                nodeset.Add(kvp.Key);
            }
            Console.WriteLine("{0} communities found", communities.Count);
            int counter = 0;
            foreach (var kvp in communities)
            {
                Console.WriteLine("community {0}: {1} people",counter,kvp.Value.Count);
                counter++;
            }
            Console.ReadLine();
        }
    }
}
