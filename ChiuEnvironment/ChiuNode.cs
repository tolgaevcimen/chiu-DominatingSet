using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChiuDominatingSet
{
    public class ChiuNode
    {
        public int Id { get; set; }
        public ChiuState State { get; set; }

        int InNeighborCount
        {
            get
            {
                return Neighbours.Count(n => n.State == ChiuState.IN);
            }
        }
        bool NoDependentNeighbor
        {
            get
            {
                return !Neighbours.Any(n => n.State == ChiuState.OUT1);
            }
        }
        bool NoBetterNeighbor
        {
            get
            {
                return !Neighbours.Any(n => n.State == ChiuState.WAIT && n.Id < this.Id);
            }
        }
        List<ChiuNode> Neighbours { get; set; }
        int MoveCount { get; set; }
        bool FirstTime { get; set; }

        object Lock { get; set; }

        public ChiuNode(int id)
        {
            Id = id;
            Neighbours = new List<ChiuNode>();
            State = ChiuState.IN;
            Lock = new object();
            FirstTime = true;
        }

        public void RunRules()
        {
            lock (Lock)
            {
                if (State == ChiuState.WAIT && InNeighborCount == 0 && NoBetterNeighbor)
                {
                    SetState(ChiuState.IN);
                }
                else if (State == ChiuState.IN && InNeighborCount == 1 && NoDependentNeighbor)
                {
                    SetState(ChiuState.OUT1);
                }
                else if (State == ChiuState.IN && InNeighborCount > 1 && NoDependentNeighbor)
                {
                    SetState(ChiuState.OUT2);
                }
                else if (State == ChiuState.WAIT && InNeighborCount == 1)
                {
                    SetState(ChiuState.OUT1);
                }
                else if ((State == ChiuState.OUT1 || State == ChiuState.WAIT) && InNeighborCount > 1)
                {
                    SetState(ChiuState.OUT2);
                }
                else if ((State == ChiuState.OUT1 || State == ChiuState.OUT2) && InNeighborCount == 0)
                {
                    SetState(ChiuState.WAIT);
                }
                else
                {
                    Console.WriteLine("I'm {0}. My state is {1}, and does not change. rc: {2}", Id, State, MoveCount);
                    if (FirstTime)
                    {
                        FirstTime = false;
                        PokeNeighbors();
                    }
                }

                Thread.Sleep(1);
            }
        }

        public void AddNeighbor(ChiuNode node)
        {
            Neighbours.Add(node);
        }

        void SetState(ChiuState state)
        {
            Console.WriteLine("I'm {0}. My state is {1}, was {2}", Id, state, State);

            MoveCount++;
            Program.IncrementTotalMoveCount();

            State = state;

            PokeNeighbors();
        }

        void PokeNeighbors()
        {
            foreach (var neighbor in Neighbours.AsParallel())
            {
                neighbor.RunRules();
            }
        }
    }
}
