using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSA.GAMax {
    public class Population
    {
        Individual[] individuals;

        #region

        public Population(int populationSize, bool initialise)
        {
            individuals = new Individual[populationSize];
            if (initialise)
            {
                for (int i = 0; i<size();i++)
                {
                    Individual newIndividual = new Individual();
                    newIndividual.generateIndividual();
                    saveIndividual(i, newIndividual);
                }
            }
        }

        public Individual getIndividual(int index)
        {
            return individuals[index];
        }

        

        public int size()
        {
            return individuals.Length;
        }

        public Individual saveIndividual(int index, Individual indiv)
        {
            if(index < 0)
            {
                individuals[individuals.Length - 1] = indiv;
            } else
            {
                individuals[index] = indiv;
            }
            return indiv;  
        }

        #endregion
    }
}
