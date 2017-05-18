using ChiuDominatingSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChiuDominatingSet
{
    public class Program
    {
        static int TotalMoveCount { get; set; }

        static object Lock = new object();

        static int n = 10;

        public static void Main()
        {
            var nodes = new List<ChiuNode>();
            //LineGraph(nodes, n);
            //CircleGraph(nodes, n);
            //StarGraph(nodes, n);
            //CompleteGraph(nodes, n);
            BipartiteGraph(nodes, n);

            nodes.FirstOrDefault().RunRules();

            PrintIns(nodes);
            Console.WriteLine("n: {0}, TotalMoveCount: {1}", n, TotalMoveCount);

            Console.ReadLine();
        }


        public static void IncrementTotalMoveCount()
        {
            lock(Lock)
            {
                TotalMoveCount++;
            }
        }

        static void PrintIns(List<ChiuNode> nodes)
        {
            Console.WriteLine("In Nodes:");
            Console.WriteLine(string.Join(", ", nodes.Where(n => n.State == ChiuState.IN).Select(n => n.Id)));
        }

        static void LineGraph(List<ChiuNode> nodes, int n)
        {
            for (int i = 0; i < n; i++)
            {
                nodes.Add(new ChiuNode(i));
            }

            for (int i = 0; i < n; i++)
            {
                if (i != 0)
                {
                    nodes[i].AddNeighbor(nodes[i - 1]);
                }
                if (i != n - 1)
                {
                    nodes[i].AddNeighbor(nodes[i + 1]);
                }
            }
        }

        static void CircleGraph(List<ChiuNode> nodes, int n)
        {
            for (int i = 0; i < n; i++)
            {
                nodes.Add(new ChiuNode(i));
            }

            for (int i = 0; i < n; i++)
            {
                if (i != 0)
                {
                    nodes[i].AddNeighbor(nodes[i - 1]);
                }

                if (i != n - 1)
                {
                    nodes[i].AddNeighbor(nodes[i + 1]);
                }
                else
                {
                    nodes[i].AddNeighbor(nodes[0]);
                    nodes[0].AddNeighbor(nodes[i]);
                }
            }
        }

        static void StarGraph(List<ChiuNode> nodes, int n)
        {
            for (int i = 0; i < n; i++)
            {
                nodes.Add(new ChiuNode(i));
            }

            for (int i = 1; i < n; i++)
            {
                nodes[0].AddNeighbor(nodes[i]);
                nodes[i].AddNeighbor(nodes[0]);
            }
        }

        static void BipartiteGraph(List<ChiuNode> nodes, int n)
        {
            if (n % 2 != 0) throw new Exception("n must be even");

            for (int i = 0; i < n; i++)
            {
                nodes.Add(new ChiuNode(i));
            }

            for (int i = 0; i < n/2; i++)
            {
                for (int j = n/2; j < n; j++)
                {
                    nodes[i].AddNeighbor(nodes[j]);
                    nodes[j].AddNeighbor(nodes[i]);
                }
            }
        }

        static void CompleteGraph(List<ChiuNode> nodes, int n)
        {
            for (int i = 0; i < n; i++)
            {
                nodes.Add(new ChiuNode(i));
            }

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;

                    nodes[i].AddNeighbor(nodes[j]);
                }
            }
        }
    }
}
